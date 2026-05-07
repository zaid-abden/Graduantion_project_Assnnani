using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.BackgroundJobs.Appointments
{
    public interface IAppointmentJobService
    {
        Task AutoConfirm(int appointmentId);
        Task SendUpcomingAppointmentReminders(CancellationToken cancellationToken);
        Task MarkNoShowAppointments(CancellationToken cancellationToken);
    }
}
