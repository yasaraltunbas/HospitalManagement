using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.Admission
{
    public class AdmissionGetDto: IEntityGetDto
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public int PatientId { get; set; }
		public int DoctorId { get; set; }
		public int DepartmentId { get; set; }
		public string Reason { get; set; }
	}
}
