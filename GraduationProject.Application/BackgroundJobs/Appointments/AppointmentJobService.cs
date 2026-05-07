using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.BackgroundJobs.Appointments
{
    public class AppointmentJobService : IAppointmentJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService emailService;

        public AppointmentJobService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            this._unitOfWork = unitOfWork;
            this.emailService = emailService;
        }
        public async Task AutoConfirm(int appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.Query()
                .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
            if (appointment == null)
                return;
            if (appointment.AppointmentStatus != AppointmentStatus.Pending)
                return;
            appointment.AppointmentStatus = AppointmentStatus.Confirmed;
            await _unitOfWork.SaveAsync();


        }
        public async Task MarkNoShowAppointments(CancellationToken cancellationToken)
        {
            var now = DateTime.Now;

            var appointments = await _unitOfWork.Appointments.Query()
                .Include(x => x.ScheduleSlot)
                .Where(x =>
                    x.AppointmentStatus == AppointmentStatus.Confirmed &&
                    x.IsCheckedIn == false)
                .ToListAsync(cancellationToken);

            foreach (var appt in appointments)
            {
                var slotDateTime = appt.ScheduleSlot.Date
                    .ToDateTime(appt.ScheduleSlot.StartTime);

                if (slotDateTime.AddMinutes(5) < now)
                {
                    appt.AppointmentStatus = AppointmentStatus.NoShow;
                }
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task SendUpcomingAppointmentReminders(CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var targetTime = now.AddMinutes(3);

            var appointments = await _unitOfWork.Appointments.Query()
                .Include(x => x.Patient)
                    .ThenInclude(p => p.User)
                .Include(x => x.ScheduleSlot)
                .Where(x =>
                    x.AppointmentStatus == AppointmentStatus.Confirmed &&
                    x.IsReminderSent == false)
                .ToListAsync(cancellationToken);

            foreach (var appt in appointments)
            {
                var slotDateTime = appt.ScheduleSlot.Date
                    .ToDateTime(appt.ScheduleSlot.StartTime);

               
                if (slotDateTime <= targetTime && slotDateTime > now)
                {
                    var email = appt.Patient.User.Email;

                    await emailService.SendEmailAsync(
                        email,
                        "Appointment Reminder",
                        $"Your appointment is at {slotDateTime:hh:mm tt}");

                    appt.IsReminderSent = true;
                }
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
