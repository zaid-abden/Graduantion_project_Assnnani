using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Commands.UpdateSpecialization
{
    public class UpdateSpecializationCommandHandler : IRequestHandler<UpdateSpecializationCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateSpecializationCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
        {
            var specilization = await unitOfWork.Specialization.GetByIdAsync(request.Id);
            if(specilization==null)
                return Result<int>.Failure(ResultStatus.NotFound,$"Specilization with Id {request.Id} not found");
            specilization.Name = request.Name;
             unitOfWork.Specialization.Update(specilization);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(specilization.Id);
        }
    }
}
