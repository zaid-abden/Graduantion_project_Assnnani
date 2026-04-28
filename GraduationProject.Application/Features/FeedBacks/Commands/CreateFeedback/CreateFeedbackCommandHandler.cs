using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.FeedBacks.Commands.CreateFeedback
{
    public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateFeedbackCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
           this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var patient = await unitOfWork.Patients.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (patient is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Patient profile not found");
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.DoctorId == request.DoctorId
                ,cancellationToken);
            if (doctor is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Doctor profile not found");
          
            var hasCompletedVisit = await unitOfWork.Appointments.Query()
    .AnyAsync(x =>
        x.PatientId == patient.PatientId &&
        x.DoctorId == request.DoctorId 
        && x.AppointmentStatus == AppointmentStatus.Completed
        ,
        cancellationToken);


       
            
            
            if (!hasCompletedVisit)
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "You cannot submit feedback because you have no completed appointments with this doctor."
                );

            var exists = await unitOfWork.Feedbacks.Query()
          .AnyAsync(x => x.PatientId == patient.PatientId
                      && x.DoctorId == request.DoctorId);

            if (exists)
                return Result<int>.Failure(ResultStatus.Conflict,"Feedback already exists");

            var feedback = new Feedback
            {
                DoctorId = request.DoctorId,
                Comment = request.Comment,
                CreatedAt = DateTime.Now,
                PatientId = patient.PatientId,
                Rating = request.Rating,
            };
            var newRatingCount = doctor.RatingCount + 1;

          
            doctor.Rating =
       ((doctor.Rating * doctor.RatingCount) + request.Rating)
       / newRatingCount;

            doctor.RatingCount = newRatingCount;

            await unitOfWork.Feedbacks.AddAsync(feedback);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(feedback.FeedbackId);
        }
    }
    
}
