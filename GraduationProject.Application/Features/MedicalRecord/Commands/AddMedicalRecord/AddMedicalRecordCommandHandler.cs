//using GraduationProject.Application.Common.Results;
//using GraduationProject.Application.Contracts.Identity;
//using GraduationProject.Application.Contracts.Repositories;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GraduationProject.Application.Features.MedicalRecord.Commands.AddMedicalRecord
//{
//    public class AddMedicalRecordCommandHandler : IRequestHandler<AddMedicalRecordCommand, Result<int>>
//    {
//        private readonly IUnitOfWork unitOfWork;
//        private readonly ICurrentUserService currentUserService;

//        public AddMedicalRecordCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
//        {
//            this.unitOfWork = unitOfWork;
//            this.currentUserService = currentUserService;
//        }
//        public async Task<Result<int>> Handle(AddMedicalRecordCommand request, CancellationToken cancellationToken)
//        {
//            if (!currentUserService.IsAuthenticated)
//                return Result<int>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
//            var userId = currentUserService.UserId;
//            var doctor = await unitOfWork.Doctors.Query()
//                .FirstOrDefaultAsync(x => x.UserId == userId
//                , cancellationToken);
//            if (doctor is null)
//                return Result<int>.Failure(ResultStatus.NotFound, "Doctor profile not found");
//            var patient = await unitOfWork.Patients.Query()
//                .FirstOrDefaultAsync(x => x.PatientId == request.PatientId
//                , cancellationToken);

//            if (patient is null)
//                return Result<int>.Failure(ResultStatus.NotFound, "Patient profile not found");
//            if(patient.AssignedDoctorId != doctor.DoctorId)
//                return Result<int>.Failure(ResultStatus.Unauthorized, "You are not assigned to this patient");
           
//        }
//    }
//}
