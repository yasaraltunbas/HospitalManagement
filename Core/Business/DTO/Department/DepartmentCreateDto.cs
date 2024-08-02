using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.Department
{
	public class DepartmentCreateDto: IDTO
	{
		public string DepartmentName { get; set; }
		public string Location { get; set; }

		public decimal Fee { get; set; }
	}
}
