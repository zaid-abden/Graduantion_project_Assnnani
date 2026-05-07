using GraduationProject.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.StudentDoctors.Commands.CompleteStudentDoctorProfile
{
    public class CompleteStudentDoctorProfileCommand : IRequest<Result<string>>
    {
       
        public string Email { get; set; }
        public string NationalId { get; set; }
        public int YearsOfStudy { get; set; }

        public string SupervisingNumber { get; set; }
        public IFormFile File { get; set; }
    }
}
