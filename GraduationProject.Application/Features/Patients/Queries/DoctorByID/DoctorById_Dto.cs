namespace GraduationProject.Application.Features.Patients.Queries.DoctorByID
{
    public class DoctorById_Dto
    {

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

    }
}
