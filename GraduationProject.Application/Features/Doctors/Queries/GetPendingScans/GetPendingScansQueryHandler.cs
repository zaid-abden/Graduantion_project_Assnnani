using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Scans.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetPendingScans
{
    public class GetPendingScansQueryHandler : IRequestHandler<GetPendingScansQuery, Result<List<ScanDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPendingScansQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<ScanDto>>> Handle(GetPendingScansQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<ScanDto>>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                , cancellationToken);
            if (doctor is null)
                return Result<List<ScanDto>>.Failure(ResultStatus.NotFound, "Doctor profile not found");

            var pendingScansDto = await unitOfWork.Scans.Query()
                .Where(x => x.DoctorId == doctor.DoctorId
                && !x.IsDeleted
                && x.Status == ScanStatus.Pending)
                .Select(x => new ScanDto
                {
                    Id = x.Id,
                    PatientName = x.Patient.User.FullName,
                    ScanType = x.ScanType.ToString(),
                    UploadedAt = x.UploadedAt
                }).ToListAsync(cancellationToken);
            return Result<List<ScanDto>>.Success(pendingScansDto);  
        }
    }
}
