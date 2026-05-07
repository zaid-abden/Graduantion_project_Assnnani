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

namespace GraduationProject.Application.Features.Doctors.Queries.GetMyStudentDoctors
{
    public class GetMyStudentDoctorsQueryHandler : IRequestHandler<GetMyStudentDoctorsQuery, Result<List<StudentDoctorListDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyStudentDoctorsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<StudentDoctorListDto>>> Handle(GetMyStudentDoctorsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<StudentDoctorListDto>>.Failure(
     ResultStatus.Unauthorized,
     "Please log in first.");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);
            if (doctor == null)
                return Result<List<StudentDoctorListDto>>.Failure(ResultStatus.Forbidden, "You do not have permission to perform this action.");
            var SuperVisingNumber = doctor.SupervisingNumber;
            var students = await unitOfWork.StudentDoctors.Query()
                .Include(x => x.User) 
                .Where(x => x.SupervisingNumber == SuperVisingNumber)
                .Select(x => new StudentDoctorListDto
                {
                    StudentDoctorId = x.StudentDoctorId,
                    StudentName = x.User.FullName,
                    NationalId = x.NationalId,
                    University = x.University,
                    Status = x.Status.ToString(),
                    YearsOfStudy = x.YearsOfStudy
                }).ToListAsync(cancellationToken);

            return Result<List<StudentDoctorListDto>>.Success(students);

        }
    }
}
