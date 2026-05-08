using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.ActivateReceptionist
{
    public class ActivateReceptionistCommand:IRequest<Result<string>>
    {
        public int ReceptionistId { get; set; }
        public ActivateReceptionistCommand(int id)
        {
            ReceptionistId = id;
        }
    }
}
