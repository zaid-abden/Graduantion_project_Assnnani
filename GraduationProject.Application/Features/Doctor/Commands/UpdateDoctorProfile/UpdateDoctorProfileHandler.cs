
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace GraduationProject.Application.Features.Doctors.Commands.UpdateDoctorProfile
{
	public class UpdateDoctorProfileHandler : IRequestHandler<UpdateDoctorProfileCommand, Result<string>>
	{
		
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateDoctorProfileHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
		
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

		public async Task<Result<string>> Handle(UpdateDoctorProfileCommand request, CancellationToken cancellationToken)
		{
			if (!currentUserService.IsAuthenticated)
			{
				return Result<string>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
			}
			var userId = currentUserService.UserId;
			var doctor = await unitOfWork.Doctors.Query()
				.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
			if (doctor is null)
				return Result<string>.Failure(ResultStatus.NotFound, "Doctor profile not found");

			doctor.FullName = request.FirstName + request.LastName;

			doctor.ClinicPhoneNumber = request.PhoneNumber;
			doctor.About = request.About;
			doctor.City = request.City;
			doctor.Street = request.Street;
			doctor.Country = request.Country;
			doctor.YearsOfExperience = request.YearsOfExperience;
			await unitOfWork.SaveAsync();
			 return Result<string>.Success("Doctor profile updated sucessfull");



		}
	}
}

