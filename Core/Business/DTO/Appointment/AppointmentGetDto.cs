using Core.Business.DTO.Department;
using Core.Business.DTO.Doctor;
using Core.Business.DTO.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.Appointment
{
    public class AppointmentGetDto : IEntityGetDto
	{
		public int Id { get; set; }

		public DateTime Date { get; set; }
		public string Reason { get; set; }

		public int PatientId { get; set; }
		public DoctorGetDto Doctor { get; set; }
		public DepartmentGetDto Department { get; set; }
		public PatientGetDto Patient { get; set; }
	}
}
