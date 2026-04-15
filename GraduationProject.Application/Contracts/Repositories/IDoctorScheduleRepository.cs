using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Contracts.Repositories
{
    public interface IDoctorScheduleRepository: IGenericRepository<doctorSchedule>
    {
        public bool CheckClinicOverlap(
    WeekDay dayOfWeek,
    TimeSpan startTime,
    TimeSpan endTime,
    string location,
    int? scheduleId = null
);
        public bool checkOverlap(int doctorId, WeekDay dayOfWeek, TimeSpan startTime, TimeSpan endTime,string location, int? scheduleId = null);
    }
}
