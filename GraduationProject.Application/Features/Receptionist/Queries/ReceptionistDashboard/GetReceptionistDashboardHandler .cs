using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Application.Features.Receptionist.Queries.ReceptionistDashboard
{
    //public class GetReceptionistDashboardHandler : IRequestHandler<GetReceptionistDashboardQuery, Result<ReceptionistDashboardDto>>
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly UserManager<User> _userManager;

    //    public GetReceptionistDashboardHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _userManager = userManager;
    //    }

    //    public async Task<Result<ReceptionistDashboardDto>> Handle(GetReceptionistDashboardQuery request, CancellationToken cancellationToken)
    //    {
    //        var Receptionist = await _unitOfWork.Receptionists.GetReceptionistbyId(request.ReceptionistId);
    //        if (Receptionist == null)
    //            return Result<ReceptionistDashboardDto>.Failure(ResultStatus.NotFound, "Receptionist Not Found");
    //        var ReceptionistName = Receptionist.User.FullName;

    //        var Queue = await _unitOfWork.Receptionists.GetQueue(request.ReceptionistId);
    //        var QueueCount = Queue.Count();

    //        var AppointmentsToday = await _unitOfWork.Receptionists.GetAppointmentToday(request.ReceptionistId);
    //        var AppointmentsCount = AppointmentsToday.Count();

    //        var totalpatient = await _unitOfWork.Receptionists.TotalPatient(request.ReceptionistId);

    //        var result = new ReceptionistDashboardDto
    //        {
    //            TodayAppointments = AppointmentsToday,
    //            PatientQueue = Queue,
    //            ReceptionistName = ReceptionistName,
    //            Stats = new DashboardStatsDto
    //            {
    //                TotalPatientsToday = totalpatient,
    //                TodayAppointments = AppointmentsCount,
    //                ActiveQueue = QueueCount
    //            }
    //        };
    //        return Result<ReceptionistDashboardDto>.Success(result);
    //    }
    //}
}
