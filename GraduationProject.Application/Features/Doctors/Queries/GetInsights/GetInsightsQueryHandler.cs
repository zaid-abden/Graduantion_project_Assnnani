using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetInsights
{

    public class GetInsightsQueryHandler : IRequestHandler<GetInsightsQuery, Result<InsightsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetInsightsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<InsightsDto>> Handle(GetInsightsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<InsightsDto>.Failure(ResultStatus.Unauthorized, "Unauthorized");

            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    x.DoctorId,
                    Price = x.price ?? 0
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (doctor == null)
                return Result<InsightsDto>.Failure(ResultStatus.NotFound, "Doctor not found");

            var now = DateTime.Now;

            var startOfCurrentMonth = new DateTime(now.Year, now.Month, 1);
            var startOfLastMonth = startOfCurrentMonth.AddMonths(-1);


            double CalcChange(decimal current, decimal last)
            {
                if (last == 0)
                    return current == 0 ? 0 : 100;

                return Math.Round(((double)(current - last) / (double)last) * 100, 2);
            }


            var completedAppointmentsQuery = unitOfWork.Appointments.Query()
                .Where(a => a.DoctorId == doctor.DoctorId &&
                            a.AppointmentStatus == AppointmentStatus.Completed);

         
            var patientsQuery = unitOfWork.Patients.Query()
                .Where(p => p.AssignedDoctorId == doctor.DoctorId);

            var totalPatients = await patientsQuery.CountAsync(cancellationToken);

            var currentMonthPatients = await patientsQuery
                .Where(p => p.CreatedAt >= startOfCurrentMonth)
                .CountAsync(cancellationToken);

            var lastMonthPatients = await patientsQuery
                .Where(p => p.CreatedAt >= startOfLastMonth &&
                            p.CreatedAt < startOfCurrentMonth)
                .CountAsync(cancellationToken);

           
            var totalAppointments = await completedAppointmentsQuery.CountAsync(cancellationToken);

            var currentMonthAppointments = await completedAppointmentsQuery
                .Where(a => a.CreatedAt >= startOfCurrentMonth)
                .CountAsync(cancellationToken);

            var lastMonthAppointments = await completedAppointmentsQuery
                .Where(a => a.CreatedAt >= startOfLastMonth &&
                            a.CreatedAt < startOfCurrentMonth)
                .CountAsync(cancellationToken);

 
            var scansQuery = unitOfWork.Scans.Query()
                .Where(s => s.DoctorId == doctor.DoctorId &&
                            s.Status == ScanStatus.Reviewed);

            var totalScans = await scansQuery.CountAsync(cancellationToken);

            var currentMonthScans = await scansQuery
                .Where(s => s.CreatedAt >= startOfCurrentMonth)
                .CountAsync(cancellationToken);

            var lastMonthScans = await scansQuery
                .Where(s => s.CreatedAt >= startOfLastMonth &&
                            s.CreatedAt < startOfCurrentMonth)
                .CountAsync(cancellationToken);

         
            var totalRevenue = await completedAppointmentsQuery.CountAsync(cancellationToken)
                               * (doctor.Price);

            var currentMonthRevenue = await completedAppointmentsQuery
                .Where(a => a.CreatedAt >= startOfCurrentMonth)
                .CountAsync(cancellationToken)
                * (doctor.Price);

            var lastMonthRevenue = await completedAppointmentsQuery
                .Where(a => a.CreatedAt >= startOfLastMonth &&
                            a.CreatedAt < startOfCurrentMonth)
                .CountAsync(cancellationToken)
                * (doctor.Price);

          
            var result = new InsightsDto
            {
                TotalPatients = new InsightItemDto
                {
                    Value = totalPatients,
                    Change = CalcChange(currentMonthPatients, lastMonthPatients)
                },
                Appointment = new InsightItemDto
                {
                    Value = totalAppointments,
                    Change = CalcChange(currentMonthAppointments, lastMonthAppointments)
                },
                ScanProcessed = new InsightItemDto
                {
                    Value = totalScans,
                    Change = CalcChange(currentMonthScans, lastMonthScans)
                },
                Revenue = new InsightItemDto
                {
                    Value = totalRevenue,
                    Change = CalcChange(currentMonthRevenue, lastMonthRevenue)
                }
            };

            return Result<InsightsDto>.Success(result);
        }
    }

}

