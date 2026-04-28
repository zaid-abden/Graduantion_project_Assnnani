using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.MedicalRecords.Commands.AddMedicalRecord
{
    public class AddMedicalRecordCommandHandler : IRequestHandler<AddMedicalRecordCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public AddMedicalRecordCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(AddMedicalRecordCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");

            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Doctor profile not found");

            var appointment = await unitOfWork.Appointments.Query()
                .Include(x => x.ScheduleSlot)
                .FirstOrDefaultAsync(x =>
                    x.AppointmentId == request.AppointmentId &&
                    !x.IsDeleted &&
                    x.DoctorId == doctor.DoctorId,
                    cancellationToken);

            if (appointment is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Appointment not found");

            //if (appointment.AppointmentStatus != AppointmentStatus.Completed)
            //    return Result<int>.Failure(ResultStatus.Conflict, "Medical record can only be added for completed appointments");

            var exists = await unitOfWork.MedicalRecords.Query()
                .AnyAsync(x => x.AppointmentId == request.AppointmentId, cancellationToken);

            if (exists)
                return Result<int>.Failure(ResultStatus.Conflict, "Medical record already exists for this appointment");

            var medicalRecord = new medicalRecord
            {
                Title = request.Title,
                DoctorId = doctor.DoctorId,
                Diagnosis = request.Diagnosis,
                VisitDate = appointment.ScheduleSlot.Date,
                Notes = request.Notes,
                AppointmentId = request.AppointmentId
            };

            await unitOfWork.MedicalRecords.AddAsync(medicalRecord);
            await unitOfWork.SaveAsync();

            return Result<int>.Success(medicalRecord.RecordId);
        }
    }
}
