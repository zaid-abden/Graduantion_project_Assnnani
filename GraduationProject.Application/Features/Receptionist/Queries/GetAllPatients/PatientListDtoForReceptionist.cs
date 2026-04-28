namespace GraduationProject.Application.Features.Receptionist.Queries.GetAllPatients
{
    public class PatientListDtoForReceptionist
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public DateOnly? LastVisit { get; set; }
        public string DoctorName { get; set; }
    }
}
