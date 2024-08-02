using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.DTO.Doctor
{
    public class DoctorGetDto: IEntityGetDto
	{
		public int Id { get; set; }
		public int DepartmentId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
