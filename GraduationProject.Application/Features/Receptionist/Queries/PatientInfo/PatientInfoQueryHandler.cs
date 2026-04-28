using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Application.Features.Receptionist.Queries.PatientInfo
{
    internal class PatientInfoQueryHandler : IRequestHandler<GetPatientInfoQuery, Result<PatientInfoDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public PatientInfoQueryHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<Result<PatientInfoDto>> Handle(GetPatientInfoQuery request, CancellationToken cancellationToken)
        {
            var patint = await _unitOfWork.Patients.GetByIdAsync(request.PatientId);
            if (patint == null) return Result<PatientInfoDto>.Failure(ResultStatus.NotFound, "patient Not Found");
            var result = await _unitOfWork.Receptionists.GetPatientInfo(request.PatientId);
            return Result<PatientInfoDto>.Success(result);
        }
    }
}
