using AutoMapper;
using Business.Abstract;
using Core.Business.DTO.Patient;
using DataAccess.User;
using Domain;
using HospitalManagement.DataAccess;
using HospitalManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class UserManager : IUserService
	{
		private readonly HospitalDbContext _context;
		private readonly IConfiguration _configuration;
		

		public UserManager(HospitalDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
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

					Gender = createUserDto.Gender,
					BloodType = createUserDto.BloodType,

				};
				_context.Patients.Add(patient);
			}
			else if (role == UserRole.Doctor)
			{
				var doctor = new Doctor
				{
					Specialty = createUserDto.Specialty,
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
				new Claim(ClaimTypes.Name, user.Email),
				new Claim(ClaimTypes.Role, user.Role.ToString())
			}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
		public async Task<List<UserGetDto>> GetAllUsersAsync()
		{
			var users = await _context.Users
	
				.Include(u => u.Patient)
				.Include(u => u.Doctor)
				.Select(u => new UserGetDto
				{
					Id = u.Id,
					Email = u.Email,
					PhoneNumber = u.PhoneNumber,
					Address = u.Address,
					BloodType = u.Patient != null ? u.Patient.BloodType : null,
					BirthDay = u.Patient != null ? u.Patient.BirthDay : null,
					Gender = u.Doctor != null ? u.Doctor.Gender : null,
					Specialty = u.Doctor != null ? u.Doctor.DoctorSpecificProperty2 : null,
				})
				.ToListAsync();

			return users;
		}
	}


	

}

