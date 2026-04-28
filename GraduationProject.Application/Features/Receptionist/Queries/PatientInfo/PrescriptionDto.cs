namespace GraduationProject.Application.Features.Receptionist.Queries.PatientInfo
{
    public class PrescriptionnDto
    {
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string DoctorName { get; set; }
        public DateTime PrescribedDate { get; set; }
    }
}
