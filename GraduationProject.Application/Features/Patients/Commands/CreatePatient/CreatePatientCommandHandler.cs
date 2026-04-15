using AutoMapper;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Application.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, PatientDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        public CreatePatientCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<PatientDto> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(request);
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create patient user: {errors}");
            }

            await userManager.AddToRoleAsync(user, "Patient");

            var patient = mapper.Map<Patient>(request);
            patient.UserId = user.Id;
            await unitOfWork.Patients.AddAsync(patient);
            await unitOfWork.SaveAsync();
            var patientDto = mapper.Map<PatientDto>(patient);
            patientDto.Email = user.Email;
            patientDto.FullName = user.FullName;
            patientDto.PhoneNumber = user.PhoneNumber;
            return patientDto;
        }
    }
}
