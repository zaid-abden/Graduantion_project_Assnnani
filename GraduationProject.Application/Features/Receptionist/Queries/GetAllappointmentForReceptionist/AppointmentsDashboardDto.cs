namespace GraduationProject.Application.Features.Receptionist.Queries.GetAllappointmentForReceptionist
{
    public class AppointmentsDashboardDto
    {
        public int Total { get; set; }
        public int Upcoming { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }

        public List<AppointmentCardDto> Appointments { get; set; }
    }
}
