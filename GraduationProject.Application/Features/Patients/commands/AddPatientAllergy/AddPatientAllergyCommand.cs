using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.commands.AddPatientAllergy
{
    public class AddPatientAllergyCommand : IRequest<Result<int>>   
    {
        public int PatientId { get; set; }
        public int AllergyId { get; set; }
        public string? Notes { get; set; }
    }
}
