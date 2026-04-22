using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Patients.Queries.GetAvaliableSlots
{
    public class GetAvaliableSlotsQuery
    : IRequest<Result<List<AvailableDayDto>>>
    {
        public int DoctorId { get; set; }
    }

}
