using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadCommandHandler
        : IRequestHandler<MarkAllNotificationsAsReadCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public MarkAllNotificationsAsReadCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> Handle(
            MarkAllNotificationsAsReadCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized");

            var userId = _currentUser.UserId;

            var notifications = await _unitOfWork.Notifications.Query()
                .Where(x => x.UserId == userId && !x.IsRead)
                .ToListAsync(cancellationToken);

            if (!notifications.Any())
                return Result<string>.Success("There are no unread notifications.");

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _unitOfWork.SaveAsync();

            return Result<string>.Success("All notifications marked as read");
        }
    }
}
