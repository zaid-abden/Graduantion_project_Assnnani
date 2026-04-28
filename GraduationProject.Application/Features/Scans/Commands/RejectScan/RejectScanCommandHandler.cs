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

namespace GraduationProject.Application.Features.Scans.Commands.RejectScan
{
    public class RejectScanCommandHandler : IRequestHandler<RejectScanCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RejectScanCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(RejectScanCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");

            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor profile not found");

            var scan = await unitOfWork.Scans.Query()
                .FirstOrDefaultAsync(x => x.Id == request.ScanId
                                       && x.DoctorId == doctor.DoctorId
                                       && !x.IsDeleted,
                                       cancellationToken);

            if (scan is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Scan not found");

         
            if (scan.Status == ScanStatus.Reviewed)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot reject a reviewed scan");

            if (scan.Status == ScanStatus.Rejected)
                return Result<string>.Failure(ResultStatus.Conflict, "Scan already rejected");

        
            if (scan.Status != ScanStatus.Pending &&
                scan.Status != ScanStatus.InProgress)
            {
                return Result<string>.Failure(ResultStatus.Conflict, "Scan cannot be rejected in its current state");
            }

         
            scan.Status = ScanStatus.Rejected;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Scan rejected successfully");
        }
    }
}
