using Business.Abstract;
using Business.Concrete;
using Core.Business.DTO.Appointment;
using Core.Business.DTO.Patient;
using Core.Result;
using DataAccess.User;
using Domain;
using HospitalManagement.Domain;
using Microsoft.AspNetCore.Mvc;
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

		public AuthController(IUserService userService, ExceptionLogManager exceptionLogManager, IConfiguration configuration)
		{
			_userService = userService;
			_exceptionLogManager = exceptionLogManager;
			_configuration = configuration;

		}

		//[HttpPost("registerPatient")]
		//public async Task<IActionResult> RegisterPatient([FromBody] PatientRegisterDto input)
		//{
		//	var registeredPatient = await _userService.RegisterPatient(input);

		//	if (registeredPatient == null)
		//		return BadRequest(new { message = "Username is already taken" });

		//	return Ok(new { message = "Patient registered successfully" });
		//}
		//[HttpPost("registerDoctor")]
		//public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegisterDto input)
		//{
		//	var registeredDoctor = await _userService.RegisterDoctor(input);

		//	if (registeredDoctor == null)
		//		return BadRequest(new { message = "Username is already taken" });

		//	return Ok(new { message = "Doctor registered successfully" });
		//}

		//[HttpPost("authenticatePatient")]
		//public async Task<IActionResult> AuthenticatePatient([FromBody] PatientLoginDto model)
		//{

		//		var authenticatedPatient = await _userService.AuthenticatePatient(model.Email, model.Password);

		//		if (authenticatedPatient == null)
		//			return Unauthorized(new { message = "Invalid credentials" });

		//		var token = GenerateJwtToken(authenticatedPatient.Id.ToString(), "Patient");


		//	return Ok(new { Token = token });

		//}

		//[HttpPost("authenticateDoctor")]
		//public async Task<IActionResult> AuthenticateDoctor([FromBody] DoctorLoginDto input)
		//{
		//	var authenticatedDoctor = await _userService.AuthenticatePatient(input.Email, input.Password);

		//	if (authenticatedDoctor == null)
		//		return Unauthorized(new { message = "Invalid credentials" });

		//	var token = GenerateJwtToken(authenticatedDoctor.Id.ToString(), "Doctor");


		//	return Ok(new { Token = token });
		//}

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
