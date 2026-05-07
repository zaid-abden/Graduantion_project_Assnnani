using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.Queries.GetPatientProfile;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class GetPatientProfileHandler
        : IRequestHandler<GetPatientProfileQuery, Result<PatientProfileDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

    public GetPatientProfileHandler(IUnitOfWork unitOfWork,UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<Result<PatientProfileDto>> Handle(
        GetPatientProfileQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var patient = await _unitOfWork.Patients.GetpatientByIdAsync(request.UserId);

            if (patient == null)
                return Result<PatientProfileDto>.Failure(ResultStatus.NotFound,"Patient not found");

            var dto = new PatientProfileDto
            {
                PatientId = patient.PatientId,
                FullName = patient.User.FirstName + " " + patient.User.LastName,
                Email = patient.User.Email,
                Phone = patient.User.PhoneNumber,
                Address = patient.Address,
              
            };

            return Result<PatientProfileDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<PatientProfileDto>.Failure(ResultStatus.Failure,ex.Message);
        }
    }
}