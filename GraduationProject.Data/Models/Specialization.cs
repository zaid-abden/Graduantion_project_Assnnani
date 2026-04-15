using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<doctor> Doctors { get; set; }
    }

}
