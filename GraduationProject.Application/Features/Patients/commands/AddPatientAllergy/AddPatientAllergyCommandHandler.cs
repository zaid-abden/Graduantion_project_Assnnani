using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.commands.AddPatientAllergy
{
    public class AddPatientAllergyCommandHandler : IRequestHandler<AddPatientAllergyCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public AddPatientAllergyCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(AddPatientAllergyCommand request, CancellationToken cancellationToken)
        {
            var patient = await unitOfWork.Patients.Query()
                .Include(x => x.PatientAllergies)
                .FirstOrDefaultAsync(x => x.PatientId == request.PatientId
                && !x.IsDeleted 
                && x.Status ==PatientStatus.Active
                , cancellationToken);
            var allergy = await unitOfWork.Allergies.Query()
                .FirstOrDefaultAsync(x => x.Id == request.AllergyId, cancellationToken);
            if(allergy is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Allergy not found");
            if (patient is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Patient not found or inactive");
            var checkAllergyExist =  await unitOfWork.PatientAllergies.Query()
                .AnyAsync(x => x.PatientId == request.PatientId
                          && x.AllergyId == request.AllergyId
                , cancellationToken);
            if(checkAllergyExist)
                return Result<int>.Failure(ResultStatus.Conflict, "Allergy already exists for this patient");

            var patientAllergy = new PatientAllergy
            {
                PatientId = request.PatientId,
                AllergyId = request.AllergyId,
                Notes = request.Notes,
                NotedAt = DateTime.Now
            };
            await unitOfWork.PatientAllergies.AddAsync(patientAllergy);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(patientAllergy.PatientId);
        }
    }
}
