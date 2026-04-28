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

namespace GraduationProject.Application.Features.Scans.Commands.ReopenScan
{
    public class ReopenScanCommandHandler : IRequestHandler<ReopenScanCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ReopenScanCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(ReopenScanCommand request, CancellationToken cancellationToken)
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

           
            if (scan.Status != ScanStatus.Rejected)
                return Result<string>.Failure(ResultStatus.Conflict,
                    "Only rejected scans can be reopened");

          
            if (scan.Status == ScanStatus.Reviewed)
                return Result<string>.Failure(ResultStatus.Conflict,
                    "Reviewed scans cannot be reopened");

       
            scan.Status = ScanStatus.Pending;

          
            scan.Findings = null;
            scan.Recommendations = null;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Scan reopened successfully");
        }
    }
}
