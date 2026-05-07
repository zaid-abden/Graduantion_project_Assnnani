using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Contracts.Services
{
    public interface INotificationService
    {
        Task SendToUserAsync(
            string userId,
            string title,
            string message,
            NotificationType type);

        Task SendToRoleAsync(
            string role,
            string title,
            string message,
            NotificationType type);
    }
}
