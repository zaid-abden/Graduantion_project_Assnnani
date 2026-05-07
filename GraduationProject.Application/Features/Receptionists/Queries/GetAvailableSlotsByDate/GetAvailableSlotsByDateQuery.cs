using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetAvailableSlotsByDate
{
    public class GetAvailableSlotsByDateQuery:IRequest<Result<List<SlottsDto>>>
    {
        public DateOnly Date { get; set; }
    }
    public class SlottsDto
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
