using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.MedicalRecords.Commands.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetPatientMedicalHistory
{
    public class GetPatientMedicalHistoryQueryHandler : IRequestHandler<GetPatientMedicalHistoryQuery, Result<List<MedicalRecordForDoctorDashboardDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPatientMedicalHistoryQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<MedicalRecordForDoctorDashboardDto>>> Handle(GetPatientMedicalHistoryQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<MedicalRecordForDoctorDashboardDto>>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                , cancellationToken);
            if (doctor is null)
                return Result<List<MedicalRecordForDoctorDashboardDto>>.Failure(ResultStatus.NotFound, "Doctor profile not found");
            var patient = await unitOfWork.Patients.Query()
                .FirstOrDefaultAsync(x => x.PatientId == request.PatientId
                , cancellationToken);

            if (patient is null)
                return Result<List<MedicalRecordForDoctorDashboardDto>>.Failure(ResultStatus.NotFound, "Patient profile not found");
            if(patient.AssignedDoctorId != doctor.DoctorId)
                return Result<List<MedicalRecordForDoctorDashboardDto>>.Failure(ResultStatus.Forbidden, "You are not assigned to this patient");

            var medicalRecords = await unitOfWork.MedicalRecords.Query()
       .AsNoTracking()
       .Where(x => x.Appointment.PatientId == patient.PatientId
       && x.DoctorId == doctor.DoctorId)
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
