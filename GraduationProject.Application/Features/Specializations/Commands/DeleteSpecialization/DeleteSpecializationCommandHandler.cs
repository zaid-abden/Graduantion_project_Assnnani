using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Commands.DeleteSpecialization
{
    public class DeleteSpecializationCommandHandler : IRequestHandler<DeleteSpecializationCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteSpecializationCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteSpecializationCommand request, CancellationToken cancellationToken)
        {
            var specialization=await unitOfWork.Specialization.GetByIdAsync(request.Id);
            if(specialization==null)
            {
                return Result<int>.Failure(ResultStatus.NotFound,$"Specialization with Id {request.Id} not found");
            }
             unitOfWork.Specialization.Delete(specialization);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(specialization.Id);
        }
    }
}
