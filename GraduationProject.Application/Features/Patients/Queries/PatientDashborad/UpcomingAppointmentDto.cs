namespace GraduationProject.Application.Features.Patients.Queries.PatientDashborad
{
    public class UpcomingAppointmentDto
    {
        public string DoctorName { get; set; }
        public string Specialty { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Starttime { get; set; }
        public string Status { get; set; } // confirmed / pending
    }
}
