using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Receptionist
    {
        public int ReceptionistId { get; set; }
        public ShiftType Shift { get; set; }= ShiftType.Morning;
       // public string? ImageUrl { get; set; }

     public bool IsActive { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public TimeOnly ShiftStart { get; set; }   
        public TimeOnly ShiftEnd { get; set; }    
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public doctor Doctor { get; set; }
    }

}
