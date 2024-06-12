using Core.Domain;
using Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Domain

{
	public class Doctor: BaseEntity
	{
		[Required]
		public string Specialty { get; set; }

		[ForeignKey ("User") ]
		[Required]
		public int UserId { get; set; }

		public virtual User User { get; set; }


	}
}
