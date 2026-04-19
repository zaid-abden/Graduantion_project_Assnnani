using AutoMapper;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using MediatR;

namespace GraduationProject.Application.Features.Doctors.Commands.UpdateDoctorProfile
{
	public class UpdateDoctorProfileHandler : IRequestHandler<UpdateDoctorProfileCommand, Result<bool>>
	{
		private readonly IDoctorRepository _doctorRepository;
		private readonly IMapper _mapper;

		public UpdateDoctorProfileHandler(IDoctorRepository doctorRepository, IMapper mapper)
		{
			_doctorRepository = doctorRepository;
			_mapper = mapper;
		}

		public async Task<Result<bool>> Handle(UpdateDoctorProfileCommand request, CancellationToken cancellationToken)
		{
			// 1. Fetch existing entity
			var doctor = await _doctorRepository.GetDoctorWithUserAsync(request.DoctorId);

			if (doctor == null)
				return Result<bool>.Failure(ResultStatus.NotFound, "Doctor profile not found.");

			_mapper.Map(request, doctor);

			_doctorRepository.Update(doctor);
			var success = await _doctorRepository.SaveChangesAsync();

			if (!success)
				return Result<bool>.Failure(ResultStatus.Failure, "An error occurred while updating the profile.");

			return Result<bool>.Success(true);
		}
	}
}
