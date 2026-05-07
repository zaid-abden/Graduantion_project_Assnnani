using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Notifications.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Notifications.Queries.GetNotifications
{
    public class GetNotificationsQuery
         : IRequest<Result<List<NotificationDto>>>
    {
    }
}
