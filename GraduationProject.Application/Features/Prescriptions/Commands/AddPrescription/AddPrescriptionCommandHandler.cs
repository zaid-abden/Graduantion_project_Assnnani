using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
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

namespace GraduationProject.Application.Features.Prescriptions.Commands.AddPrescription
{
    public class AddPrescriptionCommandHandler : IRequestHandler<AddPrescriptionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public AddPrescriptionCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public  async Task<Result<int>> Handle(AddPrescriptionCommand request, CancellationToken cancellationToken)
        {
            
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(ResultStatus.Unauthorized, "Unauthorized access");
           

           
            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Doctor not found");
          

          
            var patient = await unitOfWork.Patients.Query()
                .FirstOrDefaultAsync(x => x.PatientId == request.PatientId, cancellationToken);

            if (patient is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Patient not found");
            

          
            var isAssigned = await unitOfWork.Patients.Query()
                .AnyAsync(x =>
                    x.PatientId == request.PatientId &&
                    x.AssignedDoctorId == doctor.DoctorId &&
                    !x.IsDeleted &&
                    x.Status == PatientStatus.Active,
                    cancellationToken);

            if (!isAssigned)
                return Result<int>.Failure(ResultStatus.Forbidden,
                    "You can only prescribe to your assigned patients");
           

          
            if (request.Items == null || !request.Items.Any())
                return Result<int>.Failure(ResultStatus.Failure,
                    "Prescription must contain at least one item");
           

           
            var patientAllergies = await unitOfWork.PatientAllergies.Query()
                .Include(x => x.Allergy)
                .Where(x => x.PatientId == request.PatientId)
                .ToListAsync(cancellationToken);

            foreach (var item in request.Items)
            {
                var isAllergic = patientAllergies.Any(a =>
                    a.Allergy.Name.Trim().ToLower() ==
                    item.MedicationName.Trim().ToLower());

                if (isAllergic)
                    return Result<int>.Failure(ResultStatus.Conflict,
                        $"Patient is allergic to {item.MedicationName}");
            }
          

        
            var prescription = new Prescription
            {
                PatientId = request.PatientId,
                DoctorId = doctor.DoctorId,
                Date = DateTime.UtcNow,
                Items = request.Items.Select(item => new PrescriptionItem
                {
                    MedicationName = item.MedicationName,
                    Dosage = item.Dosage,
                    Frequency = item.Frequency,
                    DurationInDays = item.DurationInDays
                }).ToList()
            };
            

          
            await unitOfWork.Prescriptions.AddAsync(prescription);
            await unitOfWork.SaveAsync();
          

            return Result<int>.Success(prescription.Id);



        }
    }
}
