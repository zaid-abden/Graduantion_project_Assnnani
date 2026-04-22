using GraduationProject.Application.Common.Results;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.commands.AddPatient
{
    public class AddPatientCommand:IRequest<Result<int>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        // Patient data
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? MedicalHistory { get; set; }
        public Data.Enums.Gender Gender { get; set; }
    }
    }

