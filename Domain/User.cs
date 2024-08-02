using Core.Domain;
using HospitalManagement.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class User : BaseEntity
	{
		[Required]
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		
		[Required]
		public string PhoneNumber { get; set; }
		
		[Required]
		public string PasswordHash { get; set; }
		public virtual Patient Patient { get; set; }
		public virtual Doctor Doctor { get; set; }

		[Required]
		public UserRole Role { get; set; }

	}
}
