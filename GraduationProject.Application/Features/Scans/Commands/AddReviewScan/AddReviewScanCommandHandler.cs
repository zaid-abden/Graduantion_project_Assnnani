using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Commands.AddReviewScan
{
    public class AddReviewScanCommandHandler : IRequestHandler<AddReviewScanCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public AddReviewScanCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(AddReviewScanCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                , cancellationToken);
            if (doctor is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor profile not found");

            var scan = await unitOfWork.Scans.Query()
                .FirstOrDefaultAsync(x => x.Id == request.ScanId
                && x.DoctorId == doctor.DoctorId
                && !x.IsDeleted
                ,cancellationToken);
            if(scan is null)
               return Result<string>.Failure(ResultStatus.NotFound, "Scan not found");

            switch (scan.Status)
            {
                case ScanStatus.Pending:
                    return Result<string>.Failure(ResultStatus.Conflict,
                        "Scan must be started first before reviewing");

                case ScanStatus.InProgress:
               
                    break;

                case ScanStatus.Reviewed:
                    return Result<string>.Failure(ResultStatus.Conflict,
                        "Scan already reviewed");

                case ScanStatus.Rejected:
                    return Result<string>.Failure(ResultStatus.Conflict,
                        "Rejected scan cannot be reviewed. Please request re-upload");

                default:
                    return Result<string>.Failure(ResultStatus.Failure,
                        "Invalid scan status");
            }

            scan.Findings = request.Findings;
            scan.Recommendations = request.Recommendations;
            scan.Status = ScanStatus.Reviewed;
            await unitOfWork.SaveAsync();

            return Result<string>.Success("Scan reviewed successfully");
        }
    }
}
