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

namespace GraduationProject.Application.Features.Appointments.Queries.GetAvailableSlots
{
    public class GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, Result<List<AvailableSlotDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAvailableSlotsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<AvailableSlotDto>>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
        {
            var slots = await unitOfWork.ScheduleSlots.Query()
              .Include(s => s.DoctorSchedule)
              .Where(s =>
                  s.DoctorSchedule.DoctorId == request.DoctorId &&
                  s.Date == request.Date && s.Status == SlotStatus.Available)
              .Select(s => new AvailableSlotDto
              {
                  SlotId = s.Id,
                  StartTime = s.StartTime,
                  EndTime = s.EndTime,
                  IsAvailable = true
              })
              .ToListAsync(cancellationToken);
            return Result<List<AvailableSlotDto>>.Success(slots);
        }
    }
}
