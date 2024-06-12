using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.MedicalRecord
{
	public class MedicalRecordUpdateDto: IDTO
	{

		public int Id { get; set; }

			public string Diagnosis { get; set; }
		public string Notes { get; set; }
		public string Treatment { get; set; }

		public DateTime Date { get; set; }
		public string Medication { get; set; }

	}
}
