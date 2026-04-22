using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Commands.CreateSpecialization
{
    public class CreateSpecializationCommandHandler : IRequestHandler<CreateSpecializationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateSpecializationCommandHandler(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }
        public async Task<int> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
        {
            var specilaization = new Specialization
            {
                Name = request.Name
            };
            await _unitOfWork.Specialization.AddAsync(specilaization);
            await _unitOfWork.SaveAsync();      
            return specilaization.Id;
        }
    }
}
