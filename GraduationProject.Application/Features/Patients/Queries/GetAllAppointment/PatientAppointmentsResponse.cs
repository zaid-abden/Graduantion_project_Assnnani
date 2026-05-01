using GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion;

namespace GraduationProject.Application.Features.Patients.Queries.GetAllAppointment
{
    public class PatientAppointmentsResponse
    {
        public int TotalAppointments { get; set; }

        public int UpcomingAppointments { get; set; }

        public int CompletedAppointments { get; set; }

        public int CancelledAppointments { get; set; }

        public PagedResult<PatientAppointmentDto> Appointments { get; set; }
    }
}
