using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class MedicalRecordAttachment
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int MedicalRecordId { get; set; }
        public medicalRecord MedicalRecord { get; set; }
    }
}
