using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.User
{
    public class UserRegisterDto
    {

        public string Email { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		
		 
		public string Address { get; set; }
		public string Gender { get; set; }	
		public DateTime BirthDay { get; set; }
		public decimal Balance { get; set; }
		//public UserRole Role { get; set; }
		public string BloodType { get; set; }
		
		public int DepartmentId { get; set; }


    }
}
