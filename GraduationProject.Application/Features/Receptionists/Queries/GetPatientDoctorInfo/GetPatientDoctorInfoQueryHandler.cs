using GraduationProject.Application.Common.PaginatedResults;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Appointments.Dtos;
using GraduationProject.Application.Features.MedicalRecord.Dtos;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Application.Features.Prescriptions.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetPatientDoctorInfo
{
    public class GetPatientDoctorInfoQueryHandler : IRequestHandler<GetPatientDoctorInfoQuery, Result<PatientDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPatientDoctorInfoQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<PatientDetailsDto>> Handle(GetPatientDoctorInfoQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<PatientDetailsDto>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;

            var receptionstDoctor = await unitOfWork.Receptionists.Query()
                .Include(c => c.Doctor)
                .Where(x => x.UserId == userId)
                .Select(c => c.Doctor)
                .FirstOrDefaultAsync(cancellationToken);
            ;
            if (receptionstDoctor == null)
                return Result<PatientDetailsDto>
                    .Failure(ResultStatus.Forbidden, "You do not have permission to perform this action");

            var patient = await unitOfWork.Patients.Query()
                .Where(x => x.PatientId == request.Id
                && x.AssignedDoctorId == receptionstDoctor.DoctorId)
                .Select(x => new PatientDetailsDto
                {
                    PersonalInfo = new PersonalInfoDto
                    {
                        FullName = x.User.FullName,
                        Email = x.User.Email!,
                        Phone = x.User.PhoneNumber!,
                        Address = x.Address!,
                        Age = DateTime.Today.Year - x.DateOfBirth.Year - (DateTime.Today.DayOfYear < x.DateOfBirth.DayOfYear ? 1 : 0),
                        BloodType = x.BloodType.ToString()!,
                        Gender = x.Gender.ToString()!
                    }
                    ,
                    Allergies = x.PatientAllergies.Where(x => x.PatientId
                    == request.Id).Select(a => a.Allergy.Name).ToList(),
                    Prescriptions = x.Prescriptions.SelectMany(x => x.Items)
                    .Where(x => x.Prescription.DoctorId == receptionstDoctor.DoctorId
                    ).Select(c => new PrescriptionDto
                    {
                        Dosage = c.Dosage,
                        DoctorName = x.AssignedDoctor!.User.FullName,
                        Frequency = c.Frequency,
                        MedicationName = c.MedicationName,
                        Date = c.Prescription.Date,


                    }).ToList()
                    ,
                    Appointments = x.Appointments
                    .Where(a => a.DoctorId == receptionstDoctor.DoctorId)
                    .OrderByDescending(c => c.ScheduleSlot.Date)
                    .Take(5)
                    .Select(a => new AppointmentDtoForDoctorDashboard
                    {
                        DoctorName = x.AssignedDoctor!.User.FullName,
                        Status = a.AppointmentStatus.ToString(),
                        Date = a.ScheduleSlot.Date,
                        Title = a.AppointmentType.ToString(),

                    }).ToList(),
                    // .Where(x => x.DoctorId ==doctor.DoctorId)
                    MedicalHistories = x.AssignedDoctor!.MedicalRecords
                    .Where(m => m.DoctorId == receptionstDoctor.DoctorId)
                    .Select(m => new MedicalHistoryDto
                    {
                        Diagnosis = m.Diagnosis!,
                        DiagnosedDate = m.VisitDate,
                        CreatedBy = currentUserService.UserName!,
                        DoctorNotes = m.Notes!,
                    }).ToList()
                    ,


                }).FirstOrDefaultAsync(cancellationToken);

            if (patient is null)
                return Result<PatientDetailsDto>.Failure(ResultStatus.NotFound, "Patient not found or not assigned to you");
            return Result<PatientDetailsDto>.Success(patient);
        }
    }
}
