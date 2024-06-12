using Business.Abstract;
using Business.Concrete;
using Core.Business.DTO.Appointment;
using Core.Result;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AppointmentController:ControllerBase
	{
		private readonly IAppointmentService _appointmentService;
		private readonly ExceptionLogManager _exceptionLogManager;

		public AppointmentController(IAppointmentService appointmentService, ExceptionLogManager exceptionLogManager)
		{
			_appointmentService = appointmentService;
			_exceptionLogManager = exceptionLogManager;

		}

		[HttpPost]
		public async Task<DataResult<AppointmentGetDto>> AddAppointment([FromBody] AppointmentCreateDto input)
		{
			try
			{
				var resultEntityDto = await _appointmentService.AddAsync(input);
				return new SuccessDataResult<AppointmentGetDto>(resultEntityDto, "Appointment created successfuly");

			}
			catch (Exception e)
			{
				
				await _exceptionLogManager.LogExceptionAsync(e);
				
				return new ErrorDataResult<AppointmentGetDto>("Appointment created error");
			}
		}
		[HttpGet]
		public async Task<DataResult<ICollection<AppointmentGetDto>>> GetAllAppointments()
		{
			try
			{
				var resultEntityDto = await _appointmentService.GetAllAsync();
				return new SuccessDataResult<ICollection<AppointmentGetDto>>(resultEntityDto, "Appointments listed successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<ICollection<AppointmentGetDto>>("Appointment created error");


			}
		}
		[HttpGet("{id}")]
		public async Task<DataResult<AppointmentGetDto>> GetAppointmentById(int id)
		{
			try
			{
				var resultEntityDto = await _appointmentService.GetByIdAsync(id);
				return new SuccessDataResult<AppointmentGetDto>(resultEntityDto, "Appointment listed successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<AppointmentGetDto>("Appointment created error");

			}
		}
		[HttpPut("{id}")]
		public async Task<DataResult<AppointmentGetDto>> UpdateAppointment(int id, [FromBody] AppointmentUpdateDto input)
		{
			try
			{
				var resultEntityDto = await _appointmentService.UpdateAsync(id, input);
				return new SuccessDataResult<AppointmentGetDto>(resultEntityDto, "Appointment updated successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<AppointmentGetDto>("Appointmeyn updated error");

			}
		}
		[HttpDelete("{id}")]
		public async Task<DataResult<AppointmentGetDto>> DeleteAppointment(int id)
		{
			try
			{
				await _appointmentService.DeleteByIdAsync(id);
				return new SuccessDataResult<AppointmentGetDto>("Appointment deleted successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);	
				return new ErrorDataResult<AppointmentGetDto>("Appointment deleted error");
			}
		}
	}
}
