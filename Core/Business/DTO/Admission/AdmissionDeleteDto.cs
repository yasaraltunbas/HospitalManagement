using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.Admission
{
	public class AdmissionDeleteDto: IEntityDeleteDto
	{
		public int Id { get; set; }
	}
}
