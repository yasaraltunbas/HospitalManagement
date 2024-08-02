using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.MedicalRecord
{
    public class MedicalRecordGetDto:IEntityGetDto
	{
			
		public int Id { get; set; }
		public string Description { get; set; }

		public int PatientId { get; set; }
		public int DoctorId { get; set; }
		public int AppointmentId { get; set; }
		public DateTime Date { get; set; }
		public string Diagnosis { get; set; }
		public string Treatment { get; set; }
		public string Notes { get; set; }
		 
	}
}
