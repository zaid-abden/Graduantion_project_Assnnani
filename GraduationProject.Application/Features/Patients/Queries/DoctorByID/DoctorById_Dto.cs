using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.Queries.DoctorByID
{
    public class DoctorById_Dto
    {
        //public object DoctorId { get; internal set; }
        //public int FullName { get; internal set; }
        //public object Email { get; internal set; }
        //public object Phone { get; internal set; }
        //public object Specialization { get; internal set; }
        //public object YearsOfService { get; internal set; }
        //public object Degree { get; internal set; }
        //public object About { get; internal set; }
        //public object City { get; internal set; }
        //public object Country { get; internal set; }
        //public object Rating { get; internal set; }

        //public class DoctorProfileDto
        //{
            public int DoctorId { get; set; }

            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }

            public string Specialization { get; set; }
            public int YearsOfService { get; set; }
            public string Degree { get; set; }
            public string About { get; set; }

            public string City { get; set; }
            public string Country { get; set; }

            public double Rating { get; set; }
        //}
    }
}
