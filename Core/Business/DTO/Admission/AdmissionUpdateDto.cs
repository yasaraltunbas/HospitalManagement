using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.Admission
{
	public class AdmissionUpdateDto:IDTO
	{

		public int Id { get; set; }
		public DateTime DischargeDate { get; set; }
		public DateTime Date { get; set; }
		public string Reason { get; set; }
	}
}
