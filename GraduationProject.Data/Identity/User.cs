using GraduationProject.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Identity
{
    public class User:IdentityUser
    {
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
       
        public DateTime? BirthDate { get; set; }

       public bool IsActive { get; set; }


        // Relations
        public Receptionist Receptionist { get; set; }
        public Patient Patient { get; set; }
         public Doctor Doctor { get; set; }
        public StudentDoctor StudentDoctor { get; set; }
       
    }
}
