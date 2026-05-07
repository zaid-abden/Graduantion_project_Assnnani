using GraduationProject.Application.Common.Results;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.RegisterPatient
{
    public class RegisterPatientCommand:IRequest<Result<string>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        // Patient data
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
       
        public Gender Gender { get; set; }
        public BloodType? BloodType { get; set; }
    }
}
