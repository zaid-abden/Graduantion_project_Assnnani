using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Dtos
{
    //public class DoctorProfileToPatientDto
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Specialty { get; set; }
    //    public double Rating { get; set; }
    //    public int ReviewsCount { get; set; }
    //    public int YearsOfExperience { get; set; }
    //    public decimal ConsultationPrice { get; set; }

    //    public string About { get; set; }

    //    public string Education { get; set; }
    //    public List<string> Languages { get; set; }

    //    public string ClinicName { get; set; }
    //    public string ClinicLocation { get; set; }

    //    public bool IsAvailable { get; set; }

    //    public List<TimeSlotDto> TimeSlots { get; set; }

    //    public List<ReviewDto> Reviews { get; set; }
    //}
    //public class TimeSlotDto
    //{
    //    public DateOnly Date { get; set; }
    //    public TimeOnly StartTime { get; set; }
    //    public TimeOnly EndTime { get; set; }
    //}

    //public class ReviewDto
    //{
    //    public string PatientName { get; set; }
    //    public int Rating { get; set; }
    //    public string Comment { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //}
    //public class TimeSlotDtto
    //{
    //    public TimeOnly StartTime { get; set; }
    //    public TimeOnly EndTime { get; set; }
    //}
    public class DoctorProfileToPatientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal ConsultationPrice { get; set; }

        public string About { get; set; }

        public string Education { get; set; }
        public List<string> Languages { get; set; }

        public string ClinicName { get; set; }
        public string ClinicLocation { get; set; }

        public bool IsAvailable { get; set; }

        public List<TimeSlotDto> TimeSlots { get; set; }

        public List<ReviewDto> Reviews { get; set; }
    }

    public class TimeSlotDto
    {
        public DateOnly Date { get; set; }
        public List<SlotTimeDto> Times { get; set; }
    }

    public class SlotTimeDto
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsAvailable { get; set; }   
    }

    public class ReviewDto
    {
        public string PatientName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
