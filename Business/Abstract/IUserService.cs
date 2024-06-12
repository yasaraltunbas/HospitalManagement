using Core.Business.DTO.Patient;
using DataAccess.User;
using Domain;
using HospitalManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IUserService
	{
		//Task<Patient> RegisterPatient(PatientRegisterDto input);
		//Task<Doctor> RegisterDoctor(DoctorRegisterDto input);
		//Task<Patient> AuthenticatePatient(string email, string password);
		//Task<Doctor> AuthenticateDoctor(string email, string password);

		Task<string> RegisterAsync(UserRegisterDto createUserDto, UserRole role);
		Task<string> AuthenticateAsync(UserLoginDto loginDto);
	}
}
