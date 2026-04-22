using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.UpdateSchedule
{
    public class UpdateScheduleCommand:IRequest<Result<DoctorScheduleDto>>
    {
        public int ScheduleId { get; set; }         
        public WeekDay DayOfWeek { get; set; }       
        public TimeSpan StartTime { get; set; }      
        public TimeSpan EndTime { get; set; }       
        public string? Location { get; set; }       
        public int MaxAppointments { get; set; }     
    }
}
