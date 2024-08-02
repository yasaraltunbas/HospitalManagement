using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.Patient
{
	public class PatientRegisterDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDay { get; set; }
		
		public string gender { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string BloodType { get; set; }

	}
}
