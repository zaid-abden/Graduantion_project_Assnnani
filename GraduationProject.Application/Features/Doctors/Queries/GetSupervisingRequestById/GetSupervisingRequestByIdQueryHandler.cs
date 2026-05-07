using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Queries.GetMyStudentDoctors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetSupervisingRequestById
{
    public class GetSupervisingRequestByIdQueryHandler : IRequestHandler<GetSupervisingRequestByIdQuery, Result<SupervisingRequestDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetSupervisingRequestByIdQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<SupervisingRequestDto>> Handle(GetSupervisingRequestByIdQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<SupervisingRequestDto>.Failure(
     ResultStatus.Unauthorized,
     "Please log in first.");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);
            if (doctor == null)
                return Result<SupervisingRequestDto>.Failure(ResultStatus.Forbidden, "You do not have permission to perform this action.");
            var studentDoctor = await unitOfWork.StudentDoctors.Query()
                .Where(x => x.StudentDoctorId == request.Id)
                .Select(x => new SupervisingRequestDto
                {
                    Id = x.StudentDoctorId,
                    FullName = x.User.FullName,
                    NationalId = x.NationalId,
                    AcademicYear = $"{x.YearsOfStudy} years",
                    University = x.University,
                    Status = x.Status.ToString(),
                    ProofImageUrl = x.CertificationDocument!,

                }).FirstOrDefaultAsync(cancellationToken);
            if(studentDoctor == null)
                return Result<SupervisingRequestDto>.Failure(ResultStatus.NotFound, "Supervising request not found.");
            return Result<SupervisingRequestDto>.Success(studentDoctor);
        }
    }
}
