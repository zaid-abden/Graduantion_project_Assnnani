//using GraduationProject.Application.Common.Results;
//using GraduationProject.Application.Contracts.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GraduationProject.Application.Features.Patients.Queries.GetAvaliableSlots
//{
//    public class GetAvailableSlotsHandler
//    : IRequestHandler<GetAvailableSlotsQuery, Result<List<AvailableDayDto>>>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

//        public GetPatientProfileHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
//        {
//            _unitOfWork = unitOfWork;
//            _userManager = userManager;
//        }

//        public async Task<Result<List<AvailableDayDto>>> Handle(
//            GetAvailableSlotsQuery request,
//            CancellationToken cancellationToken)
//        {
//            try
//            {
//                //var doctor = await _context.Doctors
//                //    .Include(d => d.DoctorSchedules)
//                //    .FirstOrDefaultAsync(d => d.DoctorId == request.DoctorId);

//                var doctor =  await _unitOfWork.Doctors.GetByIdAsync(request.Id);

//                if (doctor == null)
//                    return Result<List<AvailableDayDto>>.NotFound("Doctor not found");

//                //var appointments = await _context.Appointments
//                //    .Where(a => a.DoctorID == request.DoctorId && a.Status != "Cancelled")
//                //    .ToListAsync(cancellationToken);

//                var appointments = await _unitOfWork.Appointments.GetByIdAsync(doctor.id);

//                var result = new List<AvailableDayDto>();

//                foreach (var schedule in doctor.DoctorSchedules)
//                {
//                    var daySlots = new List<SlotDto>();

//                    var start = schedule.StartTime;
//                    var end = schedule.EndTime;

//                    // 🔥 تقسيم اليوم Slots (كل 30 دقيقة)
//                    while (start < end)
//                    {
//                        var slotEnd = start.Add(TimeSpan.FromMinutes(30));

//                        var isBooked = appointments.Any(a =>
//                            a.WorkingDay == schedule.DayOfWeek &&
//                            a.Time == start);

//                        if (!isBooked)
//                        {
//                            daySlots.Add(new SlotDto
//                            {
//                                StartTime = start,
//                                EndTime = slotEnd
//                            });
//                        }

//                        start = slotEnd;
//                    }

//                    result.Add(new AvailableDayDto
//                    {
//                        Day = schedule.DayOfWeek,
//                        Slots = daySlots
//                    });
//                }

//                return Result<List<AvailableDayDto>>.Success(result);
//            }
//            catch (Exception ex)
//            {
//                return Result<List<AvailableDayDto>>.InternalError(ex.Message);
//            }
//        }
//    }
//}
