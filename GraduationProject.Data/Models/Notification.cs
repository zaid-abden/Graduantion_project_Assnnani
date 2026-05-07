using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Notification
    {
      

        public int Id { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

        public string? UserId { get; set; }
        public string? Role { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public NotificationType Type { get; set; }
    }
}
