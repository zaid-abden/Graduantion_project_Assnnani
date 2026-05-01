using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;

namespace GraduationProject.Infrastructure.Repositories
{
	public class DoctorScheduleRepository
		 : GenericRepository<doctorSchedule>, IDoctorScheduleRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public DoctorScheduleRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public bool CheckClinicOverlap(WeekDay dayOfWeek, TimeSpan startTime, TimeSpan endTime, string location, int? scheduleId = null)
		{
			return _dbContext.DoctorSchedules.Any(s =>
			s.DayOfWeek == dayOfWeek &&
			 
			s.Location == location &&
			s.ScheduleId != scheduleId &&
			s.IsActive
			);
		}

        //public bool checkOverlap(int doctorId, WeekDay dayOfWeek, TimeSpan startTime, TimeSpan endTime, string location, int? scheduleId = null)
        //{
        //	return _dbContext.DoctorSchedules.Any(s =>
        //		 s.DoctorId == doctorId &&
        //		  s.ScheduleId != scheduleId &&
        //		 s.DayOfWeek == (WeekDay)dayOfWeek &&
        //		 s.IsActive);
        //	 );

        //}
        //public bool CheckOverlap(int doctorId, WeekDay dayOfWeek, TimeSpan startTime, TimeSpan endTime, string location, int? scheduleId = null)
        //{
        //    return _dbContext.DoctorSchedules.Any(s =>
        //        s.DoctorId == doctorId &&
        //        s.ScheduleId != scheduleId &&
        //        s.DayOfWeek == dayOfWeek &&
        //        s.IsActive &&
        //        s.Location == location 

              
        //    );
        //}

        public bool checkOverlap(int doctorId, WeekDay dayOfWeek, TimeSpan startTime, TimeSpan endTime, string location, int? scheduleId = null)
        {
            return _dbContext.DoctorSchedules.Any(s =>
                s.DoctorId == doctorId &&
                s.ScheduleId != scheduleId &&
                s.DayOfWeek == dayOfWeek &&
                s.IsActive &&
                s.Location == location


                );
        }
    }
}
