namespace GraduationProject.Application.Features.Receptionist.Queries.ReceptionistDashboard
{
    public class DashboardStatsDto
    {
        public int TodayAppointments { get; set; }
        public int ActiveQueue { get; set; }
        public int TotalPatientsToday { get; set; }
        public int ActiveDoctors { get; set; }
    }
}
