using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Application.Features.Receptionist.Queries.GetAllPatients
{
    //internal class GetPatientsListQueryHandler : IRequestHandler<GetPatientsListQuery, Result<List<PatientListDtoForReceptionist>>>
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly UserManager<User> _userManager;

    //    public GetPatientsListQueryHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _userManager = userManager;
    //    }
    //    public async Task<Result<List<PatientListDtoForReceptionist>>> Handle(GetPatientsListQuery request, CancellationToken cancellationToken)
    //    {

    //        var Result = await _unitOfWork.Receptionists.patientListDtoForReceptionist(request.ReceptionistId, request.DoctorId, request.Search, request.Status);
    //        if (Result is null) return Result<List<PatientListDtoForReceptionist>>.Failure(ResultStatus.NotFound, "process field");

    //        return Result<List<PatientListDtoForReceptionist>>.Success(Result);
    //    }
    //}
}
