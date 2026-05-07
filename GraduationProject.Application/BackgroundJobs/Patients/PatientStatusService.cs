using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.BackgroundJobs.Patients
{
    public class PatientStatusService : IPatientStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task UpdateInactivePatients(CancellationToken cancellationToken)
        //{
        //    var oneDayAgo = DateTime.Now.AddDays(-1);

        //    var inactivePatients = await _unitOfWork.Patients.Query()
        //        .Where(p => p.Status != PatientStatus.InActive)
        //        .Where(p =>
        //            !_unitOfWork.Appointments.Query().Any(a =>
        //                a.PatientId == p.PatientId &&
        //                a.AppointmentStatus != AppointmentStatus.Cancelled &&
        //                a.CreatedAt >= oneDayAgo))
        //        .ToListAsync(cancellationToken);

        //    foreach (var patient in inactivePatients)
        //    {
        //        patient.Status = PatientStatus.InActive;
        //    }

        //    await _unitOfWork.SaveAsync();
        //}
        public async Task UpdateInactivePatients(CancellationToken cancellationToken)
        {
            var tenMinutesAgo = DateTime.Now.AddMinutes(-10);

            var inactivePatients = await _unitOfWork.Patients.Query()
                .Where(p => p.Status != PatientStatus.InActive)
                .Where(p =>
                    !_unitOfWork.Appointments.Query().Any(a =>
                        a.PatientId == p.PatientId &&
                        a.AppointmentStatus != AppointmentStatus.Cancelled &&
                        a.CreatedAt >= tenMinutesAgo))
                .ToListAsync(cancellationToken);

            foreach (var patient in inactivePatients)
            {
                patient.Status = PatientStatus.InActive;
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
