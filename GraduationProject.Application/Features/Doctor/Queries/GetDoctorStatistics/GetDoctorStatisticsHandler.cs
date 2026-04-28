
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using MediatR;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorStatistics
{
	//public class GetDoctorStatisticsHandler : IRequestHandler<GetDoctorStatisticsQuery, Result<DoctorStatisticsDto>>
	//{
	//	private readonly IDoctorRepository _doctorRepository;
	//	private readonly IMapper _mapper; // إضافة الـ Mapper

	//	public GetDoctorStatisticsHandler(IDoctorRepository doctorRepository, IMapper mapper)
	//	{
	//		_doctorRepository = doctorRepository;
	//		_mapper = mapper;
	//	}

	//	public async Task<Result<DoctorStatisticsDto>> Handle(GetDoctorStatisticsQuery request, CancellationToken cancellationToken)
	//	{
	//		// جلب البيانات من الريبوزتوري
	//		var statsData = await _doctorRepository.GetDoctorStatsAsync(request.DoctorId);

	//		if (statsData == null)
	//		{
	//			return Result<DoctorStatisticsDto>.Failure(ResultStatus.NotFound, "الطبيب غير موجود");
	//		}

		
	//		return Result<DoctorStatisticsDto>.Success(statsData);
	//	}
	//}
}
