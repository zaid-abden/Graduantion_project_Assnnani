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

namespace GraduationProject.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommandHandler
       : IRequestHandler<MarkNotificationAsReadCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public MarkNotificationAsReadCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> Handle(
            MarkNotificationAsReadCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<string>.Failure(
     ResultStatus.Unauthorized,
     "Authentication failed. Please login and try again."
 );

            var userId = _currentUser.UserId;

            var notification = await _unitOfWork.Notifications.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.NotificationId &&
                    x.UserId == userId,
                    cancellationToken);

            if (notification == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Notification not found");

            if (notification.IsRead)
                return Result<string>.Success("Already marked as read");

            notification.IsRead = true;

            await _unitOfWork.SaveAsync();

            return Result<string>.Success("Marked as read successfully");
        }
    }
}
