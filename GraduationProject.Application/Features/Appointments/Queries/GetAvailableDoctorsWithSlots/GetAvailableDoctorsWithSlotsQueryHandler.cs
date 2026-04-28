using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Appointments.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAvailableDoctorsWithSlots
{
    public class GetAvailableDoctorsWithSlotsQueryHandler : IRequestHandler<GetAvailableDoctorsWithSlotsQuery, Result<List<AvailableDoctorWithSlotsDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAvailableDoctorsWithSlotsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<AvailableDoctorWithSlotsDto>>> Handle(GetAvailableDoctorsWithSlotsQuery request, CancellationToken cancellationToken)
        {

            //  var query = unitOfWork.Doctors.Query()

            //      .Include(v => v.Specialization)
            //.Include(d => d.DoctorSchedules)
            //    .ThenInclude(s => s.Slots)
            //.AsQueryable();

            //  if (!string.IsNullOrWhiteSpace(request.Location))
            //  {
            //      query = query.Where(d =>
            //          d.DoctorSchedules.Any(s => s.Location == request.Location));
            //  }

            //  if (request.SpecializationId.HasValue)
            //  {
            //        query = query
            //         .Where(d => d.SpecializationId == request.SpecializationId
            //         );
            //  }

            //  var availableDoctors = await query.Select(x => new AvailableDoctorWithSlotsDto
            //  {
            //      DoctorId = x.DoctorId,
            //      DoctorName = x.FullName!,
            //      Specialization = x.Specialization.Name,
            //      Location = request.Location,
            //      AvailableSlots = x.DoctorSchedules.SelectMany(x => x.Slots)
            //      .Where(x => x.Status == SlotStatus.Available)
            //      .Select(x => new SlotDto
            //      {
            //          SlotId = x.Id,
            //          StartTime = x.StartTime,
            //          EndTime = x.EndTime,
            //      }).ToList()

            //  }).ToListAsync(cancellationToken);

            //  if(!availableDoctors.Any())
            //      return Result<List<AvailableDoctorWithSlotsDto>>.Failure(
            //      ResultStatus.NotFound,
            //      "No available doctors found.");
            //  return Result<List<AvailableDoctorWithSlotsDto>>.Success(availableDoctors);

            var query = unitOfWork.Doctors.Query()
               .Include(x => x.Specialization)
    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Location))
            {
                query = query.Where(d =>
                    d.DoctorSchedules.Any(s => s.Location == request.Location));
            }

            if (request.SpecializationId.HasValue)
            {
                query = query.Where(d =>
                    d.SpecializationId == request.SpecializationId);
            }

            var availableDoctors = await query.Select(x => new AvailableDoctorWithSlotsDto
            {
                DoctorId = x.DoctorId,
                DoctorName = x.FullName!,
                Specialization = x.Specialization != null ? x.Specialization.Name : "",

                Location = x.DoctorSchedules.AsQueryable().Include(x => x.Slots)
                    .Where(s => s.Slots.Any(sl => sl.Status == SlotStatus.Available))
                    .Select(s => s.Location)
                    .FirstOrDefault(),

                AvailableSlots = x.DoctorSchedules.AsQueryable().Include(x => x.Slots)
                    .SelectMany(s => s.Slots)
                    .Where(sl => sl.Status == SlotStatus.Available)
                    .Select(sl => new SlotDto
                    {
                        SlotId = sl.Id,
                        StartTime = sl.StartTime,
                        EndTime = sl.EndTime,
                    }).ToList()

            }).ToListAsync(cancellationToken);

            if (!availableDoctors.Any())
            {
                return Result<List<AvailableDoctorWithSlotsDto>>.Failure(
                    ResultStatus.NotFound,
                    "No available doctors found for the given filters."
                );
            }

            return Result<List<AvailableDoctorWithSlotsDto>>.Success(availableDoctors);
        }
    }
}
