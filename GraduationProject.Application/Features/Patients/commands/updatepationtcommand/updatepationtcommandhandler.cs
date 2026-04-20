
using AutoMapper;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.commands.updatepationtcommand;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class UpdatePatientProfileHandler
    : IRequestHandler<updatepationtcommand, Result<string>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UpdatePatientProfileHandler(IUnitOfWork patientRepository, IMapper mapper, UserManager<User> userManager)
    {
        unitOfWork = patientRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result<string>> Handle(
        updatepationtcommand request,
        CancellationToken cancellationToken)
    {
        
        var patient = await unitOfWork.Patients.GetByIdAsync(request.userid);
        if (patient == null)
            return Result<string>.Failure(ResultStatus.NotFound, "Patient not found.");

        var user = await _userManager.FindByIdAsync(patient.UserId); 


        user.FirstName = request.FName;
        user.LastName = request.LName;
        user.PhoneNumber = request.Phone;
        patient.Address = request.Address;
        patient.MedicalHistory = request.MedicalHistory;

         unitOfWork.Patients.Update(patient);
        var result= await _userManager.UpdateAsync(user);
        if(!result.Succeeded)
        {
            return Result<string>.Failure(ResultStatus.NotFound, "Error happend.");

        }
        return Result<string>.Success("updated");
    }
}