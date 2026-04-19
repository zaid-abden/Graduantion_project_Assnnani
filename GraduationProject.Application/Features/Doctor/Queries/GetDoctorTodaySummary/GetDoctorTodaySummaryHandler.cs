using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using MediatR;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorTodaySummary
{
	public class GetDoctorTodaySummaryHandler : IRequestHandler<GetDoctorTodaySummaryQuery, Result<DoctorTodaySummaryDto>>
	{
		private readonly IDoctorRepository _doctorRepository;

		public GetDoctorTodaySummaryHandler(IDoctorRepository doctorRepository)
		{
			_doctorRepository = doctorRepository;
		}

		public async Task<Result<DoctorTodaySummaryDto>> Handle(GetDoctorTodaySummaryQuery request, CancellationToken cancellationToken)
		{
			// استدعاء الريبوزتوري لجلب بيانات اليوم
			var summary = await _doctorRepository.GetTodaySummaryAsync(request.DoctorId);

			if (summary == null)
			{
				return Result<DoctorTodaySummaryDto>.Failure(ResultStatus.NotFound, "لم يتم العثور على بيانات لهذا الطبيب");
			}

			return Result<DoctorTodaySummaryDto>.Success(summary);
		}
	}
}
