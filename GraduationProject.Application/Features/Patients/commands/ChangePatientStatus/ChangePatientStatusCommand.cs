using GraduationProject.Application.Common.Results;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.commands.ChangePatientStatus
{
    public class ChangePatientStatusCommand : IRequest<Result<string>>
    {
        public int PatientId { get; set; }
        public PatientStatus Status { get; set; }
    }
}
