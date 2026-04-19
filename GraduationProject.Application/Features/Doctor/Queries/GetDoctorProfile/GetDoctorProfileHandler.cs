using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using MediatR;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorProfile
{
	public class GetDoctorProfileHandler : IRequestHandler<GetDoctorProfileQuery, Result<DoctorProfileDto>>
	{
		private readonly IDoctorRepository _doctorRepository;

		public GetDoctorProfileHandler(IDoctorRepository doctorRepository)
		{
			_doctorRepository = doctorRepository;
		}

		public async Task<Result<DoctorProfileDto>> Handle(GetDoctorProfileQuery request, CancellationToken cancellationToken)
		{
			var profile = await _doctorRepository.GetProfileAsync(request.DoctorId);

			if (profile == null)
			{
				return Result<DoctorProfileDto>.Failure(ResultStatus.NotFound, "لا يوجد بروفايل لهذا الطبيب");
			}

			return Result<DoctorProfileDto>.Success(profile);
		}
	}
}
