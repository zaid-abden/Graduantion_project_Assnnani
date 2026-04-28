namespace GraduationProject.Application.Features.Patients.Queries.PatientDashborad
{
    public class DoctorCardDto
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public string Status { get; set; } // available / busy
    }
}
