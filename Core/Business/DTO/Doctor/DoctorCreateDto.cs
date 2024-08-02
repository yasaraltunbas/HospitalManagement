using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.Doctor
{
	public class DoctorCreateDto : IDTO
	{

		
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		public string Speciality { get; set; }

		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }


	}
}
