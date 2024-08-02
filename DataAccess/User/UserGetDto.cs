using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.User
{
	public class UserGetDto
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }

		public UserRole Role { get; set; }

		//Patient
		public string Gender { get; set; }
		public string BloodType { get; set; }
		public DateTime BirthDay { get; set; }

		//Doctor
		public string Specialty { get; set; }

	}
}
