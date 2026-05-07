using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Queries.GetMyStudentDoctors;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.AssignSupervisor
{
    public class AssignSupervisorCommandHandler : IRequestHandler<AssignSupervisorCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IEmailService emailService;
        private readonly UserManager<User> userManager;

        public AssignSupervisorCommandHandler(IUnitOfWork unitOfWork
            , ICurrentUserService currentUserService,
            IEmailService emailService,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.emailService = emailService;
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(AssignSupervisorCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
     ResultStatus.Unauthorized,
     "Please log in first.");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);
            if (doctor == null)
                return Result<string>.Failure(ResultStatus.Forbidden, "You do not have permission to perform this action.");
            var studentDoctor = await unitOfWork.StudentDoctors.Query()
                .FirstOrDefaultAsync(x => x.StudentDoctorId ==
                request.StudentDoctorId, cancellationToken);
           
            if (studentDoctor == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Student doctor not found.");
            if (studentDoctor.SupervisingNumber != doctor.SupervisingNumber)
                return Result<string>.Failure(ResultStatus.Forbidden, "You are not the supervisor of this student doctor.");
            studentDoctor.DoctorId = doctor.DoctorId;
            var user = await userManager.FindByIdAsync(studentDoctor.UserId.ToString());
            if (user == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Associated user not found.");

            await userManager.AddToRoleAsync(user, "StudentDoctor");

            studentDoctor.ClinicLocation = request.ClinicLocation;
            studentDoctor.ClinicName = request.ClinicName;
            studentDoctor.Status = StudentDoctorStatus.Active;
            studentDoctor.Note = request.Notes;
           
            studentDoctor.VerificationStatus = DoctorVerificationStatus.Approved;
            await unitOfWork.SaveAsync();
            await emailService.SendEmailAsync(user.Email!, "Student Doctor Approved", $"Dear {user.FirstName},\n\nYour student doctor profile has been approved by Dr. {doctor.FullName}. You can now access your account and start using the platform.\n\nBest regards,\nGraduation Project Team");
            return Result<string>.Success("Student doctor assigned successfully.");

        }
    }
}
