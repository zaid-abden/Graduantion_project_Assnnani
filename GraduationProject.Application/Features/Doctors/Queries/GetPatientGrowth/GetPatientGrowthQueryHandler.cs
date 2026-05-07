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

namespace GraduationProject.Application.Features.Doctors.Queries.GetPatientGrowth
{
    public class GetPatientGrowthQueryHandler
     : IRequestHandler<GetPatientGrowthQuery, Result<PatientGrowthDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPatientGrowthQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PatientGrowthDto>> Handle(GetPatientGrowthQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<PatientGrowthDto>.Failure(ResultStatus.Unauthorized, "Unauthorized access");

            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);

            if (doctor == null)
                return Result<PatientGrowthDto>.Failure(ResultStatus.NotFound, "Doctor not found");

            var now = DateTime.Now;

            var startDate = new DateTime(now.Year, now.Month, 1).AddMonths(-6);
            var endDate = new DateTime(now.Year, now.Month, 1).AddMonths(1);

           
            var completedAppointments = await unitOfWork.Appointments.Query()
                .Where(a => a.DoctorId == doctor.DoctorId &&
                            a.AppointmentStatus == AppointmentStatus.Completed &&
                            a.CreatedAt >= startDate &&
                            a.CreatedAt < endDate)
                .Select(a => new
                {
                    a.PatientId,
                    a.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var months = new List<string>();
            var values = new List<int>();

            for (int i = 6; i >= 0; i--)
            {
                var date = now.AddMonths(-i);

                var count = completedAppointments
                    .Where(a => a.CreatedAt.Month == date.Month &&
                                a.CreatedAt.Year == date.Year)
                    .Select(a => a.PatientId)
                    .Distinct()
                    .Count();

                months.Add(date.ToString("MMMM"));
                values.Add(count);
            }

            var result = new PatientGrowthDto
            {
                Months = months,
                Values = values
            };

            return Result<PatientGrowthDto>.Success(result);
        }
    }
}
