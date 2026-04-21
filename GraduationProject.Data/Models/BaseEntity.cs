using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
