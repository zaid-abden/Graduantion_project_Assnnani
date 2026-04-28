namespace GraduationProject.Application.Features.Receptionist.Queries.PatientInfo
{
    public class PatientInfoDto
    {
        public int PatientId { get; set; }

        // Personal Details
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BloodType { get; set; }

        // Emergency Contact
        public string EmergencyContactName { get; set; } = "John Johnson";
        public string EmergencyContactRelation { get; set; } = "Spouse";
        public string EmergencyContactPhone { get; set; } = "+1 (555) 987-6543";

        // Lists

        public List<MedicalHistoryDtoo> MedicalHistory { get; set; }
        public List<AppointmentDtoPatientInfo> RecentAppointments { get; set; }
    }
}
