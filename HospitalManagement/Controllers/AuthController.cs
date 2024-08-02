using Business.Abstract;
using Business.Concrete;
using Core.Business.DTO.Appointment;
using Core.Business.DTO.Patient;
using Core.Result;
using DataAccess.User;
using Domain;
using HospitalManagement.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController: ControllerBase
	{
		private readonly IUserService _userService;
		private readonly ExceptionLogManager _exceptionLogManager;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IUserService userService, ExceptionLogManager exceptionLogManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _exceptionLogManager = exceptionLogManager;
            _configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
        }

		[Authorize]
        [HttpGet]
		[Route("user")]
        public async Task<ActionResult<UserGetDto>> GetCurrentUser()
        {
            try
            {
                var currentUser = await _userService.GetCurrentUserAsync();
                return Ok(currentUser);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut]
		[Route("UpdateUser")]
        public async Task<ActionResult<UserUpdateDto>> UpdateUser([FromBody] UserUpdateDto updateUserDto, [FromQuery] UserRole userRole)
        {
            try
            {
				var user = await _userService.GetCurrentUserAsync();
                var updatedUser = await _userService.UserUpdate(user.Id, updateUserDto, userRole);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete]
		[Route("DeleteMe")]

		public async Task<IActionResult> DeleteMe()
		{
			try
			{
				var user = await _userService.GetCurrentUserAsync();
				await _userService.DeleteUser(user.Id);
				return Ok(new { message = "User deleted successfully" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}



		}

        [HttpGet("GetAllPatients")]
		public async Task<IActionResult> GetAllPatients()
		{
			var patients = await _userService.GetAllPatientAsync();
			return Ok(patients);
		}

		[HttpGet("GetAllDoctors")]
		//[Authorize(Roles = "Patient")]
		public async Task<IActionResult> GetAllDoctors()
		{
			var doctors = await _userService.GetAllDoctorAsync();
			return Ok(doctors);
		}
		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] UserRegisterDto createUserDto, [FromQuery] UserRole role)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var token = await _userService.RegisterAsync(createUserDto, role);
				return Ok(new { message = "User registery succesfuly"});
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Route("login")]
		public async Task<DataResult<string>> Login([FromBody] UserLoginDto loginDto)
		{
			

			try
			{
				var token = await _userService.AuthenticateAsync(loginDto);
				return new SuccessDataResult<string>(token, "Token created");
				
			}
			catch (UnauthorizedAccessException ex)
			{
				return new ErrorDataResult<string>("Token created error");
			}
		}

	
	}
}
