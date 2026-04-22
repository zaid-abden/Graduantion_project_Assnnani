using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Doctors.Dtos;
using MediatR;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorStatistics
{
	public class GetDoctorStatisticsQuery : IRequest<Result<DoctorStatisticsDto>>
	{
		public int DoctorId { get; set; }
		public GetDoctorStatisticsQuery(int doctorId)
		{
			DoctorId = doctorId;
		}
	}

}
