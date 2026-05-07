using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.Queries.GetMyDoctors
{
    public class GetMyDoctorsQueryHandler : IRequestHandler<GetMyDoctorsQuery, Result<List<DoctorrDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyDoctorsQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<DoctorrDto>>> Handle(GetMyDoctorsQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var patient = await unitOfWork.Patients.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            var dtos = await unitOfWork.Appointments.Query()
                .Where(x => x.PatientId == patient.PatientId)
                .Select(x => new DoctorrDto
                {
                   Id = x.DoctorId,
                   Name = x.Doctor.User.FullName
                }).ToListAsync(cancellationToken);
            return Result<List<DoctorrDto>>.Success(dtos);
        }
    }
}
