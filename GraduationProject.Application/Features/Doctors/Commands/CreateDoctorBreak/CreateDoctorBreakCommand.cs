using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateDoctorBreak
{
    public class CreateDoctorBreakCommand : IRequest<Result<string>>
    {

        public DateTime StartTime { get; set; }
        public int DurationInMinutes { get; set; } = 10;
        public string? Note { get; set; }
    }
}
