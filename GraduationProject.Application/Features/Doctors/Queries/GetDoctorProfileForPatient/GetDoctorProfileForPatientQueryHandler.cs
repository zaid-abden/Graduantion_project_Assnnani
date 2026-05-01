using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorProfileForPatient
{
    public class GetDoctorProfileForPatientQueryHandler : IRequestHandler<GetDoctorProfileForPatientQuery, Result<DoctorProfileToPatientDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetDoctorProfileForPatientQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<DoctorProfileToPatientDto>> Handle(GetDoctorProfileForPatientQuery request, CancellationToken cancellationToken)
        {

            //var today = DateOnly.FromDateTime(DateTime.Today);

            //var doctor = await unitOfWork.Doctors.Query()
            //    .Where(x => x.DoctorId == request.DoctorId)
            //    .Select(x => new DoctorProfileToPatientDto
            //    {
            //        Id = x.DoctorId,

            //        Name = x.FullName ?? string.Empty,

            //        Specialty = x.Specialization != null
            //            ? x.Specialization.Name
            //            : string.Empty,

            //        About = x.About ?? string.Empty,

            //        ClinicName = x.ClinicName ?? string.Empty,
            //        ClinicLocation = x.ClinicLocation ?? string.Empty,

            //        Education = x.Education ?? string.Empty,
            //        Languages = x.Languages ?? null!,

            //        Rating = x.Rating,

            //        IsAvailable = x.DoctorSchedules
            //            .SelectMany(d => d.Slots)
            //            .Any(s =>
            //                s.Date >= today &&
            //                s.Status == SlotStatus.Available
            //            ),

            //        ConsultationPrice = x.price ?? 0,

            //        YearsOfExperience = x.YearsOfExperience,

            //        ReviewsCount = x.Feedbacks.Count,

            //        TimeSlots = x.DoctorSchedules
            //            .SelectMany(d => d.Slots)
            //            .Where(s =>
            //                s.Date >= today &&
            //                s.Status == SlotStatus.Available
            //            )
            //            .Select(s => new TimeSlotDto
            //            {
            //                Date = s.Date,
            //                StartTime = s.StartTime,
            //                EndTime = s.EndTime
            //            })
            //            .ToList(),

            //        Reviews = x.Feedbacks
            //            .Select(f => new ReviewDto
            //            {
            //                PatientName =
            //                    (f.Patient != null && f.Patient.User != null)
            //                    ? (f.Patient.User.FirstName + " " + f.Patient.User.LastName)
            //                    : "Anonymous",

            //                Comment = f.Comment ?? string.Empty,
            //                Rating = f.Rating,
            //                CreatedAt = f.CreatedAt
            //            })
            //            .ToList()
            //    })
            //    .FirstOrDefaultAsync(cancellationToken);

            //return doctor == null
            //    ? Result<DoctorProfileToPatientDto>.Failure(ResultStatus.NotFound, "Doctor not found")
            //    : Result<DoctorProfileToPatientDto>.Success(doctor, "Doctor profile retrieved successfully");


            var today = DateOnly.FromDateTime(DateTime.Today);

            var doctor = await unitOfWork.Doctors.Query()
                .Where(x => x.DoctorId == request.DoctorId)
                .Select(x => new DoctorProfileToPatientDto
                {
                    Id = x.DoctorId,

                    Name = x.FullName ?? string.Empty,

                    Specialty = x.Specialization != null
                        ? x.Specialization.Name
                        : string.Empty,

                    About = x.About ?? string.Empty,

                    ClinicName = x.ClinicName ?? string.Empty,
                    ClinicLocation = x.ClinicLocation ?? string.Empty,

                    Education = x.Education ?? string.Empty,




                    Rating = x.Rating,

                    IsAvailable = x.DoctorSchedules
                        .SelectMany(d => d.Slots)
                        .Any(s =>
                            s.Date >= today &&
                            s.Status == SlotStatus.Available
                        ),
                    Languages = x.Languages ?? null!,
                    ConsultationPrice = x.price ?? 0,

                    YearsOfExperience = x.YearsOfExperience,

                    ReviewsCount = x.Feedbacks.Count,

                   
                    TimeSlots = x.DoctorSchedules
                        .SelectMany(d => d.Slots)
                        .Where(s =>
                            s.Date >= today &&
                            s.Status == SlotStatus.Available
                        )
                        .GroupBy(s => s.Date)
                        .OrderBy(g => g.Key)
                        .Select(g => new TimeSlotDto
                        {
                            Date = g.Key,
                            Times = g
                                .OrderBy(s => s.StartTime)
                                .Select(s => new SlotTimeDto
                                {
                                    StartTime = s.StartTime,
                                    EndTime = s.EndTime,
                                    IsAvailable = s.Status == SlotStatus.Available
                                })
                                .ToList()
                        })
                        .ToList(),

                    Reviews = x.Feedbacks
                        .Select(f => new ReviewDto
                        {
                            PatientName =
                                (f.Patient != null && f.Patient.User != null)
                                ? (f.Patient.User.FirstName + " " + f.Patient.User.LastName)
                                : "Anonymous",

                            Comment = f.Comment ?? string.Empty,
                            Rating = f.Rating,
                            CreatedAt = f.CreatedAt
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return doctor == null
                ? Result<DoctorProfileToPatientDto>.Failure(ResultStatus.NotFound, "Doctor not found")
                : Result<DoctorProfileToPatientDto>.Success(doctor, "Doctor profile retrieved successfully");





        }
    }
}
