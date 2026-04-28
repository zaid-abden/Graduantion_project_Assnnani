using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Scans.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Queries.GetScanDetails
{
    public class GetScanDetailsQueryHandler : IRequestHandler<GetScanDetailsQuery, Result<ScanDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetScanDetailsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<ScanDetailsDto>> Handle(GetScanDetailsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<ScanDetailsDto>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                , cancellationToken);
            if (doctor is null)
                return Result<ScanDetailsDto>.Failure(ResultStatus.NotFound, "Doctor profile not found");

            var scansDto = await unitOfWork.Scans.Query()
                .Where(x => x.Id == request.Id
                && x.DoctorId == doctor.DoctorId)
                .Select(x => new ScanDetailsDto
                {
                    Id = x.Id,
                    PatientName = x.Patient.User.FullName,
                    ScanType = x.ScanType.ToString(),
                    Status = x.Status.ToString(),
                    UploadedAt = x.UploadedAt,

                    AiConfidence = x.AIReport != null ? x.AIReport.ConfidenceScore : null,
                    Result = x.AIReport != null ? x.AIReport.PredictionResult : null,

                    ImageUrl = x.FileUrl,

                    Findings = x.Findings,
                    Recommendations = x.Recommendations
                }).FirstOrDefaultAsync(cancellationToken);
            if (scansDto is null)
                return Result<ScanDetailsDto>.Failure(ResultStatus.NotFound,"No scans found or scans are still awaiting review.");

            return Result<ScanDetailsDto>.Success(scansDto);
        }
    }
}
