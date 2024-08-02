using Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain

{
	public class Department: BaseEntity
    {
		public string DepartmentName { get; set; }
        public string Location { get; set; }
		public decimal Fee { get; set; }

	}
}
