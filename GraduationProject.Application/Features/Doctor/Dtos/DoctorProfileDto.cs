using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Dtos
{
	public class DoctorProfileDto
	{
		public string FullName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string ImageUrl { get; set; }
		public string About { get; set; }
		public int YearsOfExperience { get; set; }
		public string FullAddress { get; set; } // دمج City, Street, Details
		public DoctorDegree Degree { get; set; } // Enum أو String حسب تفضيلك
		public double Rating { get; set; }
	}
}
