using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Notifications.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Notifications.Queries.GetNotifications
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Result<List<NotificationDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetNotificationsQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<NotificationDto>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<NotificationDto>>.Failure(ResultStatus.Unauthorized, "User is not authenticated");
            var userId = currentUserService.UserId;
            var roles = currentUserService.Roles;
            var notifications = await unitOfWork.Notifications.Query()
     .Where(x =>
         x.UserId == userId ||
         roles.Contains(x.Role))
     .OrderByDescending(x => x.CreatedAt)
     .Take(10)
     .Select(x => new NotificationDto
     {
         Id = x.Id,
         Title = x.Title,
         Message = x.Message,
         Type = x.Type.ToString(),
         CreatedAt = x.CreatedAt,
         IsRead = x.IsRead,
         TimeAgo = GetTimeAgo(x.CreatedAt)
     })
     .ToListAsync(cancellationToken);
            return Result<List<NotificationDto>>.Success(notifications);
        }
        private static string GetTimeAgo(DateTime dateTime)
        {
            var span = DateTime.Now - dateTime;

            if (span.TotalMinutes < 1)
                return "Just now";

            if (span.TotalMinutes < 60)
                return $"{(int)span.TotalMinutes} min ago";

            if (span.TotalHours < 24)
                return $"{(int)span.TotalHours} hours ago";

            return $"{(int)span.TotalDays} days ago";
        }
    }
}

