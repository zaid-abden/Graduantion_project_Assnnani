using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Application.Features.Patients.Queries.PatientDashborad
{
    internal class PatientDashboardQueryHandler : IRequestHandler<PatientDashboardQuery, Result<PatientDashboardDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public PatientDashboardQueryHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<Result<PatientDashboardDto>> Handle(PatientDashboardQuery request, CancellationToken cancellationToken)
        {
            var paitent = await _unitOfWork.Patients.GetpatientByIdAsync(request.PatientId);
            if (paitent == null) return Result<PatientDashboardDto>.Failure(ResultStatus.NotFound, "Patient Not Found");
            var paitentresult = await _userManager.FindByIdAsync(paitent.UserId)!;
            var patientname = paitentresult!.FullName;


            var upcomingAppointmentsCount = await _unitOfWork.Appointments.upcomingAppointmentsCount(request.PatientId);
            var prescriptionsCount = await _unitOfWork.MedicalRecords.prescriptionsCount(request.PatientId);
            var recordsCount = await _unitOfWork.MedicalRecords.recordsCount(request.PatientId);
            var labResultsCount = await _unitOfWork.AI_Reports.labResultsCount(request.PatientId);

            var upcomingresult = await _unitOfWork.Appointments.upcomingAppointment(request.PatientId);
            var avalibleDoctors = await _unitOfWork.Doctors.GetthreeDoctors();

            var Results = new PatientDashboardDto
            {
                PatientName = patientname,
                AvailableDoctors = avalibleDoctors,
                LabResultsCount = labResultsCount,
                PrescriptionsCount = prescriptionsCount,
                RecordsCount = recordsCount,
                UpcomingAppointments = upcomingresult,
                UpcomingAppointmentsCount = upcomingAppointmentsCount
            };

            return Result<PatientDashboardDto>.Success(Results);




        }
    }
}
