using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Queries.GetSchedulesByDoctor_Day
{
    public class GetSchedulesByDoctorDayQuery : IRequest<Result<List<DoctorScheduleDto>>>
    {
        public int DoctorId { get; set; }
        public WeekDay Day { get; set; }
        public GetSchedulesByDoctorDayQuery(int doctorId, WeekDay day)
        {
            DoctorId = doctorId;
            Day = day;
        }
    }
}
