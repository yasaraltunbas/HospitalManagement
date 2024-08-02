using Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Domain
{
	public class Admission: BaseEntity
	{
		
		[ForeignKey("Patient")]
		public int PatientId { get; set; }
		public Patient Patient { get; set; }
		[ForeignKey("Doctor")]
		public int DoctorId { get; set; }
		public Doctor Doctor { get; set; }
		[ForeignKey("Department")]
        public int DepartmentId { get; set; }
		public Department Department { get; set; }

		[Required]
		public DateTime Date { get; set; }
		public string Reason { get; set; }
		public DateTime DischargeDate { get; set; }
		
    }
}
