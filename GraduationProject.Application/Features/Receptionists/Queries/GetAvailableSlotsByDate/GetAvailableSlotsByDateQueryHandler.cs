using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetAvailableSlotsByDate
{
    public class GetAvailableSlotsByDateQueryHandler
       : IRequestHandler<GetAvailableSlotsByDateQuery, Result<List<SlottsDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAvailableSlotsByDateQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<SlottsDto>>> Handle(
            GetAvailableSlotsByDateQuery request,
            CancellationToken cancellationToken)
        {
            
            if (!currentUserService.IsAuthenticated)
                return Result<List<SlottsDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized access. Please log in");

            var userId = currentUserService.UserId;

            var now = TimeOnly.FromDateTime(DateTime.Now);
            var today = DateOnly.FromDateTime(DateTime.Today);

           
            var doctorId = await unitOfWork.Receptionists.Query()
                .Where(c => c.UserId == userId)
                .Select(c => c.DoctorId)
                .FirstOrDefaultAsync(cancellationToken);

           
           
            if (request.Date < today)
                return Result<List<SlottsDto>>.Failure(
                    ResultStatus.Conflict,
                    "Cannot get slots for past date");

        
            var slots = await unitOfWork.ScheduleSlots.Query()
                .Where(x =>
                    x.Date == request.Date &&
                    x.DoctorSchedule.DoctorId == doctorId &&
                    x.Status == SlotStatus.Available &&
                    (request.Date > today || x.StartTime > now)) 
                .OrderBy(x => x.StartTime) 
                .Select(x => new SlottsDto
                {
                    Id = x.Id,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime
                })
                .ToListAsync(cancellationToken);

            return Result<List<SlottsDto>>.Success(slots);
        }
    }
}
