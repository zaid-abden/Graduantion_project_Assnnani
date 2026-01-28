using AutoMapper;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.Queries.GetAllPatients
{
    public class GetAllPatientsHandler : IRequestHandler<GetAllPatientsQuery, Result<List<PatientDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        public GetAllPatientsHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        public async Task<Result<List<PatientDto>>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patientList = await unitOfWork.Patients.GetAllAsync();
            if(!patientList.Any())
            {
                return Result<List<PatientDto>>.Failure(ResultStatus.NotFound,"No patients found.");
            }
            var userIds = patientList.Select(x => x.UserId).ToList();
            var users = await userManager.Users
    .Where(u => userIds.Contains(u.Id))
    .ToListAsync(cancellationToken);

            var patientDtoList = patientList.Select(
                patient =>
                {
                    var dto = mapper.Map<PatientDto>(patient);
                    var userInfo = users.FirstOrDefault(x => x.Id == patient.UserId);
                    if (userInfo != null)
                        mapper.Map(userInfo, dto);
                    return dto;
                }

                ).ToList();
            return Result<List<PatientDto>>.Success(patientDtoList);
        }
    }
}
