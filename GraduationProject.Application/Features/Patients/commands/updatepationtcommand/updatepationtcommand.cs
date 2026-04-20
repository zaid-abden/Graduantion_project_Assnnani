using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.commands.updatepationtcommand
{
   public class updatepationtcommand  : IRequest<Result<string>>
   {
        public int userid { get;  set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string MedicalHistory { get; set; }
    }
}
