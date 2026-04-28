namespace GraduationProject.Application.Features.Receptionist.Queries.PatientInfo
{
    public class AppointmentDtoPatientInfo
    {
        public string Title { get; set; }
        public string DoctorName { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; }
    }
}
