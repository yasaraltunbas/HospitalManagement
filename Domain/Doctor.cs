using Core.Domain;
using Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Domain

{
	public class Doctor: BaseEntity
	{ 

		[ForeignKey ("User") ]
		[Required]
		public int UserId { get; set; }

		public virtual User User { get; set; }

		[ForeignKey("Department")]
		[Required]
		public int DepartmentId { get; set; }

		public virtual Department Department { get; set; }


	}
}
