﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.Appointment
{
	public class AppointmentCreateDto : IDTO
	{
		public int? PatientId { get; set; }
		public int DoctorId { get; set; }

		public int DepartmentId { get; set; }

		public DateTime Date { get; set; }
		public string Reason { get; set; }
	
	}
}
