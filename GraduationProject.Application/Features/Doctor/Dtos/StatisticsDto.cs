namespace GraduationProject.Application.Features.Doctors.Dtos
{
	public class DoctorStatisticsDto
	{
		public int TotalAppointments { get; set; }
		public int TotalPatients { get; set; }
		public int ScanReviews { get; set; }
		public int Satisfaction { get; set; }
	}
}
