using Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Domain

{
	public class MedicalRecord: BaseEntity
    {
        
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
		[ForeignKey("Doctor")]
        public int DocterId { get; set; }
        public  Doctor Doctor { get; set; }
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Medication { get; set; }
        public string Notes { get; set; }
		[Required]
		public DateTime Date { get; set; }
    }
}
