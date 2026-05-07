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

namespace GraduationProject.Application.Features.Notifications.Queries.GetUnreadNotificationsCount
{
    public class GetUnreadNotificationsCountQueryHandler
     : IRequestHandler<GetUnreadNotificationsCountQuery, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetUnreadNotificationsCountQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            GetUnreadNotificationsCountQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<int>.Failure(ResultStatus.Unauthorized, "Unauthorized");

            var userId = _currentUser.UserId;

            var count = await _unitOfWork.Notifications.Query()
                .CountAsync(x =>
                    x.UserId == userId &&
                    !x.IsRead,
                    cancellationToken);

            return Result<int>.Success(count);
        }
    }
}
