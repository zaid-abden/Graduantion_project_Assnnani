namespace GraduationProject.Application.Features.Receptionist.Queries.ReceptionistDashboard
{
    public class DoctorStatusDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Status { get; set; } // available / busy
    }
}
