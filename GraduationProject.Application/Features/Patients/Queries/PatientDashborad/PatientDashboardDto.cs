namespace GraduationProject.Application.Features.Patients.Queries.PatientDashborad
{
    public class PatientDashboardDto
    {
        public string PatientName { get; set; }
        public int UpcomingAppointmentsCount { get; set; }
        public int PrescriptionsCount { get; set; }
        public int RecordsCount { get; set; }
        public int LabResultsCount { get; set; }

        public List<UpcomingAppointmentDto> UpcomingAppointments { get; set; }
        public List<DoctorCardDto> AvailableDoctors { get; set; }
    }
}
