using Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain

{
	public class Department: BaseEntity
    {
		[Required]
		public string DepartmentName { get; set; }
        public string Location { get; set; }
    }
}
