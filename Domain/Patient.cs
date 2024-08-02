using Core.Domain;
using Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Domain

{
	public class Patient : BaseEntity
    {
        

		[Required]
		public string Gender { get; set; }
		public DateTime BirthDay { get; set; }


		[Required]
		public string BloodType { get; set; }

		[ForeignKey ("User")]
		[Required]
		public int UserId { get; set; }
		public virtual User User { get; set; }
		public decimal Balance { get; set; } // Bakiye
		public decimal Debt { get; set; } // Borç
	}
}
