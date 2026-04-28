namespace GraduationProject.Application.Features.Receptionist.Queries.ReceptionistDashboard
{
    public class ReceptionistDashboardDto
    {
        public string ReceptionistName { get; set; }
        public DashboardStatsDto Stats { get; set; }
        public List<QueueDto> PatientQueue { get; set; }
        public List<AppointmentDto> TodayAppointments { get; set; }
        public List<DoctorStatusDto> Doctors { get; set; }
    }
}
