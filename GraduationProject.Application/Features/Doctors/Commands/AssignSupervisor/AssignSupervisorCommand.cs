using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.AssignSupervisor
{
    public class AssignSupervisorCommand:IRequest<Result<string>>
    {
        public int StudentDoctorId { get; set; }

        public  string ClinicName { get; set; }
        public string ClinicLocation { get; set; }
        public string? Notes { get; set; }
    }
}
