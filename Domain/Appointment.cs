using Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Domain

{
	public class Appointment: BaseEntity
    {
       
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        [ForeignKey("Department")]
		public int DepartmentId { get; set; }
		public virtual Department Department { get; set; }
		[Required]
		public DateTime Date { get; set; }
        public string Reason { get; set; }

    }
}
