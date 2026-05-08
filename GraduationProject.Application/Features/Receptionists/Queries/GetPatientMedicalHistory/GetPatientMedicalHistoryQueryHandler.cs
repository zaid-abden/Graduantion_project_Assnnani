using GraduationProject.Application.Common.PaginatedResults;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Application.Features.MedicalRecords.Commands.Dtos;
using GraduationProject.Application.Features.Patients.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetPatientMedicalHistory
{
    public class GetPatientMedicalHistoryQueryHandler : IRequestHandler<GetPatientMedicalHistoryQuery, Result<List<MedicalRecordForDoctorDashboardDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly INotificationService notificationService;

        public GetPatientMedicalHistoryQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, INotificationService notificationService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.notificationService = notificationService;
        }
        public async Task<Result<List<MedicalRecordForDoctorDashboardDto>>> Handle(GetPatientMedicalHistoryQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<MedicalRecordForDoctorDashboardDto>>
                    .Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");

            var userId = currentUserService.UserId;
            var receptionstDoctor = await unitOfWork.Receptionists.Query()
                .Include(c => c.Doctor)
                .Where(x => x.UserId == userId)
                .Select(c => c.Doctor)
                .FirstOrDefaultAsync(cancellationToken);
            ;
            if (receptionstDoctor == null)
                return Result<List<MedicalRecordForDoctorDashboardDto>>
                    .Failure(ResultStatus.Forbidden, "You do not have permission to perform this action");


            var patient = await unitOfWork.Patients.Query()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.PatientId == request.PatientId
                , cancellationToken);

            if (patient is null)
                return Result<List<MedicalRecordForDoctorDashboardDto>>.Failure(ResultStatus.NotFound, "Patient profile not found");
            //var doctorsId = await unitOfWork.Appointments.Query()
            //    .Where(Appointments => Appointments.PatientId == request.PatientId)
            //    .Select(Appointments => Appointments.DoctorId)
            //    .Distinct().ToListAsync(cancellationToken);
            //if(doctorsId == null)
            //    return Result<List<MedicalRecordForDoctorDashboardDto>>.Failure(ResultStatus.NotFound, "No medical records found for this patient");
            //if (!doctorsId.Contains(patient.PatientId))
            //    return Result<List<MedicalRecordForDoctorDashboardDto>>.Failure(ResultStatus.Forbidden, "You are not assigned to this patient");





            var medicalRecords = await unitOfWork.MedicalRecords.Query()
       .AsNoTracking()
       .Where(x => x.Appointment.PatientId == patient.PatientId
       && x.DoctorId == receptionstDoctor.DoctorId)
       .OrderByDescending(x => x.Appointment.ScheduleSlot.Date)
       .Select(x => new MedicalRecordForDoctorDashboardDto
       {
           Id = x.RecordId,
           DoctorName = x.Doctor.FullName!,
           Date = x.Appointment.ScheduleSlot.Date,
           Title = x.Title,
           Description = x.Diagnosis!,
           Type = x.Appointment.AppointmentType.ToString(),

           Attachments = x.Attachments.Select(a => new AttachmentDto
           {
               FileName = a.FileName,
               Url = a.FilePath
           }).ToList()
       })
       .ToListAsync(cancellationToken);

            return Result<List<MedicalRecordForDoctorDashboardDto>>.Success(medicalRecords);
        }
    }
}
