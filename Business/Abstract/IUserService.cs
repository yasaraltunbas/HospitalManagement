using Core.Business.DTO.Doctor;
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
		

		Task<string> RegisterAsync(UserRegisterDto createUserDto, UserRole role);
		Task<string> AuthenticateAsync(UserLoginDto loginDto);
		Task<List<PatientGetDto>> GetAllPatientAsync();
		Task<List<DoctorGetDto>>  GetAllDoctorAsync();
		Task<UserGetDto> GetCurrentUserAsync();
        Task DeleteUser(int id);
		Task<UserUpdateDto> UserUpdate(int id, UserUpdateDto updateUserDto, UserRole userRole);
       



    }
}
