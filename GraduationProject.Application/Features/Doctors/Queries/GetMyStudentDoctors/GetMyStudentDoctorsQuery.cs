using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetMyStudentDoctors
{
    public class GetMyStudentDoctorsQuery:IRequest<Result<List<StudentDoctorListDto>>>
    {
    }
    public class StudentDoctorListDto
    {
        public int StudentDoctorId { get; set; }

        public string StudentName { get; set; }

        public string University { get; set; }

        public int YearsOfStudy { get; set; }

        public string NationalId { get; set; }

        public string Status { get; set; }
    }
}
