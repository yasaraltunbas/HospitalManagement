using AutoMapper;
using Business.Abstract;
using Core.Business.DTO.Department;
using Core.Business.DTO.Doctor;
using Core.Business.DTO.MedicalRecord;
using Core.Business.DTO.Patient;
using Core.Result;
using DataAccess.User;
using Domain;
using HospitalManagement.DataAccess;
using HospitalManagement.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
		private readonly HospitalDbContext _context;
		private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserManager(HospitalDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}

	

		public async Task<string> AuthenticateAsync(UserLoginDto loginDto)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
			if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
			{
				throw new UnauthorizedAccessException("Invalid credentials.");
			}

			return GenerateJwtToken(user);


		}
       


        public async Task<string> RegisterAsync(UserRegisterDto createUserDto, UserRole role)
		{
			var user = new User
			{
				FirstName = createUserDto.FirstName,
				LastName = createUserDto.LastName,
				Role = role,
				PhoneNumber = createUserDto.PhoneNumber,
				Email = createUserDto.Email,
				Address = createUserDto.Address
			};

			user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			if (role == UserRole.Patient)
			{
				var patient = new Patient
				{
					BirthDay = createUserDto.BirthDay,
					UserId = user.Id,
					Balance = createUserDto.Balance,
					Gender = createUserDto.Gender,
					BloodType = createUserDto.BloodType,

				};
				_context.Patients.Add(patient);
			}
			else if (role == UserRole.Doctor)
			{
				var doctor = new Doctor
				{
                    DepartmentId = createUserDto.DepartmentId,
					UserId = user.Id
				};
				_context.Doctors.Add(doctor);
			}

			await _context.SaveChangesAsync();

			return GenerateJwtToken(user);
		}
		private string GenerateJwtToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role.ToString())
			}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

       
        public async Task<List<PatientGetDto>> GetAllPatientAsync()
		{
			var patients = await _context.Patients
			.Include(p => p.User)
			.Where(p => !p.IsDeleted)
			.Select(p => new PatientGetDto
			{
				Id = p.Id,
				////UserId = p.UserId,
				Email = p.User.Email,
				FirstName = p.User.FirstName,
				LastName = p.User.LastName,

				PhoneNumber = p.User.PhoneNumber,
				Address = p.User.Address,

				Gender = p.Gender,
				BloodType = p.BloodType
			})
			.ToListAsync();

			return patients;
		}

		public async Task<List<DoctorGetDto>> GetAllDoctorAsync()
		{
			var doctors = await _context.Doctors.Include(d => d.User).Where(d => !d.IsDeleted).Select(d => new DoctorGetDto
			{
				Id = d.Id,
				FirstName = d.User.FirstName,
				LastName = d.User.LastName,
                //DepartmentId = d.DepartmentId,
				
			})
				.ToListAsync();
			return doctors;
		}

        public async Task<UserGetDto> GetCurrentUserAsync()
        {

            var user = _httpContextAccessor?.HttpContext?.User;

            int id = Convert.ToInt32(user?.FindFirst(ClaimTypes.NameIdentifier).Value);
            string email = user?.FindFirst(ClaimTypes.Email).Value;

            var userEntity = await _context.Users
                .Include(u => u.Patient)
                .Include(u => u.Doctor)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (userEntity == null)
            {
                throw new Exception("User not found");
            }


            var userDto = new UserGetDto
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                PhoneNumber = userEntity.PhoneNumber,
                Address = userEntity.Address,
				Role = userEntity.Role,
                Gender = userEntity.Patient?.Gender,
                BloodType = userEntity.Patient?.BloodType,
               

            };

            return userDto;
        }

        public async Task DeleteUser(int id)
        {

           
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsDeleted = true;
            user.DeletedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();




        }

        public async Task<UserUpdateDto> UserUpdate(int id, UserUpdateDto updateUserDto, UserRole userRole)
        {
            var user = await _context.Users.Include(u => u.Patient).Include(u => u.Doctor).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.Email = updateUserDto.Email;
            user.Address = updateUserDto.Address;
			

            if (userRole == UserRole.Patient)
            {
                var patient = user.Patient;
                if (patient == null)
                {
                    throw new Exception("Patient not found");
                }


                patient.Gender = patient.Gender;
                patient.BloodType = patient.BloodType;
            }
            else if (userRole == UserRole.Doctor)
            {
                var doctor = user.Doctor;
                if (doctor == null)
                {
                    throw new Exception("Doctor not found");
                    

                }

                
                
            }

            await _context.SaveChangesAsync();

            var updatedUserDto = new UserUpdateDto
            {
              
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
				Address = user.Address,
			
				Gender = user.Patient?.Gender,
				BloodType = user.Patient?.BloodType
                
            };

            return updatedUserDto;
        }
    }

		

	

}

