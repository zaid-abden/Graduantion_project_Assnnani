using AutoMapper;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.Queries.GetPatientById
{
    public class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery, Result<PatientDto>>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        public GetPatientByIdHandler(IMapper mapper, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<Result<PatientDto>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await unitOfWork.Patients.GetByIdAsync(request.Id);
            if (patient == null)
                return Result<PatientDto>.Failure(ResultStatus.NotFound, "Patient not found.");

            var userInfo = await userManager.FindByIdAsync(patient.UserId);
            if (userInfo == null)
                return Result<PatientDto>.Failure(ResultStatus.NotFound, "User information not found for the patient.");

            var patientDto = mapper.Map<PatientDto>(patient);
            mapper.Map(userInfo, patientDto);

            return Result<PatientDto>.Success(patientDto);
        }
    }
}
