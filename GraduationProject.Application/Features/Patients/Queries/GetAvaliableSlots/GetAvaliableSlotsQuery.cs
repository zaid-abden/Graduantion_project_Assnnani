using GraduationProject.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.Queries.GetAvaliableSlots
{
    public class GetAvaliableSlotsQuery
    : IRequest<Result<List<AvailableDayDto>>>
    {
        public int DoctorId { get; set; }
    }

}
