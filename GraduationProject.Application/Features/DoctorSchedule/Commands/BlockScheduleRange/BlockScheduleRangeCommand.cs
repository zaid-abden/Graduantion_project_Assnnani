using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.BlockScheduleRange
{
    public class BlockScheduleRangeCommand : IRequest<Result<string>>
    {
        public int Id { get; set; } 
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }
}
