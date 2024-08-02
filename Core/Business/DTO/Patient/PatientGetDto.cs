﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.Patient
{
    public class PatientGetDto:IEntityGetDto
    {
		public int Id { get; set; }

		public string UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDay { get; set;}

		public string Gender { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string BloodType { get; set; }
	}
}
