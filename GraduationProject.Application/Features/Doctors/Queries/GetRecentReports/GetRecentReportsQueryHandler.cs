using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetRecentReports
{
    public class GetRecentReportsQueryHandler
      : IRequestHandler<GetRecentReportsQuery, Result<List<RecentReportDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetRecentReportsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<RecentReportDto>>> Handle(GetRecentReportsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<RecentReportDto>>.Failure(ResultStatus.Unauthorized, "Unauthorized");

            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor == null)
                return Result<List<RecentReportDto>>.Failure(ResultStatus.NotFound, "Doctor not found");

         
            var scansList = await unitOfWork.Scans.Query()
                .Where(s => s.DoctorId == doctor.DoctorId && s.Status == ScanStatus.Reviewed)
                .Select(s => new RecentReportDto
                {
                    Title = s.FileName,
                    Date = s.ReviewedAt ?? s.CreatedAt,
                    Type = "Scan",
                    Size = (s.FileSize / (1024.0 * 1024.0)).ToString("0.00") + " MB",
                    FileUrl = s.FileUrl
                })
                .ToListAsync(cancellationToken);

           
            var recordsList = await unitOfWork.MedicalRecords.Query()
                .Include(m => m.Attachments)
                .Where(m => m.DoctorId == doctor.DoctorId)
                .Select(m => new RecentReportDto
                {
                    Title = m.Title,
                    Date = m.VisitDate.ToDateTime(TimeOnly.MinValue),
                    Type = "MedicalRecord",
                    Size = m.Attachments.Count + " file(s)",
                    FileUrl = m.Attachments
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? ""
                })
                .ToListAsync(cancellationToken);

         
            var aiList = await unitOfWork.AI_Reports.Query()
                .Include(a => a.Scan)
                .Where(a => a.DoctorId == doctor.DoctorId)
                .Select(a => new RecentReportDto
                {
                    Title = "AI Report",
                    Date = a.CreatedAt,
                    Type = "AI",
                    Size = a.ConfidenceScore.HasValue
                        ? a.ConfidenceScore.Value.ToString("0.0") + "%"
                        : "-",
                    FileUrl = a.Scan.FileUrl
                })
                .ToListAsync(cancellationToken);

          
            var result = scansList
                .Concat(recordsList)
                .Concat(aiList)
                .OrderByDescending(x => x.Date)
                .Take(10)
                .ToList();

            return Result<List<RecentReportDto>>.Success(result);
        }
    }
}
