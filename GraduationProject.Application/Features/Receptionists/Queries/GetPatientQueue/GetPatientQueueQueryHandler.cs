using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Receptionists.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetPatientQueue
{
    public class GetPatientQueueQueryHandler
       : IRequestHandler<GetPatientQueueQuery, Result<List<PatientQueueDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPatientQueueQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<PatientQueueDto>>> Handle(
            GetPatientQueueQuery request,
            CancellationToken cancellationToken)
        {
           
            if (!currentUserService.IsAuthenticated)
                return Result<List<PatientQueueDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "You must be logged in to access this resource.");

            var userId = currentUserService.UserId;

            
            var receptionist = await unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (receptionist is null)
                return Result<List<PatientQueueDto>>.Failure(
                    ResultStatus.NotFound,
                    "No receptionist profile found for this user.");

            var today = DateOnly.FromDateTime(DateTime.Today);


            //    var queue = await unitOfWork.Appointments.Query()
            //.Include(x => x.Patient)
            //    .ThenInclude(p => p.User)
            //.Include(x => x.Doctor)
            //.Include(x => x.ScheduleSlot)
            //.Where(x =>
            //    x.DoctorId == receptionist.DoctorId &&
            //    x.ScheduleSlot.Date == today &&
            //    x.AppointmentStatus == AppointmentStatus.Confirmed &&
            //    x.IsCheckedIn && x.QueueStatus != QueueStatus.Complete)
            //.OrderBy(x => x.QueueNumber)
            //.Select(x => new PatientQueueDto
            //{
            //    Id = x.AppointmentId,
            //    Name = x.Patient.User.FirstName + " " + x.Patient.User.LastName,
            //    DoctorName = x.Doctor.FullName!,
            //    ArrivalTime = x.ArrivedAt,
            //    QueueNumber = x.QueueNumber ?? 0,
            //    Status = x.QueueStatus.ToString()!
            //})
            //.ToListAsync(cancellationToken);
            var queue = await unitOfWork.Appointments.Query()
        .Include(x => x.Patient)
            .ThenInclude(p => p.User)
        .Include(x => x.Doctor)
        .Include(x => x.ScheduleSlot)
        .Where(x =>
            x.DoctorId == receptionist.DoctorId &&
            x.ScheduleSlot.Date >= today &&
            x.ScheduleSlot.Date < today.AddDays(1) &&
            x.AppointmentStatus != AppointmentStatus.Cancelled &&
            x.QueueStatus != QueueStatus.Complete
            && x.QueueNumber.HasValue)
        .OrderBy(x => x.QueueNumber)
        .Select(x => new PatientQueueDto
        {
            Id = x.AppointmentId,
            Name = x.Patient.User.FirstName + " " + x.Patient.User.LastName,
            DoctorName = x.Doctor.FullName!,
            ArrivalTime = x.ArrivedAt,
            QueueNumber = x.QueueNumber ?? 0,
            Status = x.QueueStatus.ToString()!
        })
        .ToListAsync(cancellationToken);

            return Result<List<PatientQueueDto>>.Success(queue);
        }
    }
}
