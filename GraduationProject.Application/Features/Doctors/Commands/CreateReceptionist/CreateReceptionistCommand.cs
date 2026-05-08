using GraduationProject.Application.Common.Results;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateReceptionist
{
    public class CreateReceptionistCommand : IRequest<Result<string>>
    {
        public IFormFile Image { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Clinic { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public TimeOnly ShiftStart { get; set; }
        public TimeOnly ShiftEnd { get; set; }

        public ShiftType Shift { get; set; }
    }
}
