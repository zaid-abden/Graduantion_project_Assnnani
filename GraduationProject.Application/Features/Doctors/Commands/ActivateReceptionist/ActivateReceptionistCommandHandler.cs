using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Queries.GetReceptionistAccessControl;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.ActivateReceptionist
{
    public class ActivateReceptionistCommandHandler : IRequestHandler<ActivateReceptionistCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ActivateReceptionistCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(ActivateReceptionistCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access");

            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);

            if (doctor == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor not found");
            var receptionst = await unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.ReceptionistId == request
                .ReceptionistId, cancellationToken);
            if(receptionst is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Receptionist not found");
            if(receptionst.DoctorId != doctor.DoctorId)
                return Result<string>.Failure(ResultStatus.Forbidden, "You do not have permission to perform this action");
            if(receptionst.IsActive)
                return Result<string>.Failure(ResultStatus.Conflict, "Receptionist is already active");
            receptionst.IsActive = true;
            await unitOfWork.SaveAsync();
            return Result<string>.Success("Receptionist activated successfully");
        }
    }
}
