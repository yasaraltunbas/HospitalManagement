using Business.Abstract;
using Business.Concrete;
using Core.Business.DTO.Appointment;
using Core.Result;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
	[ApiController]
	[Route("api/appointment")]
	public class AppointmentController:ControllerBase
	{
		private readonly IAppointmentService _appointmentService;
		private readonly ExceptionLogManager _exceptionLogManager;
		private readonly IUserService _userService;

		public AppointmentController(IAppointmentService appointmentService, ExceptionLogManager exceptionLogManager, IUserService userService)
		{
			_appointmentService = appointmentService;
			_exceptionLogManager = exceptionLogManager;
			_userService = userService;

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

		[HttpPut("AppointmentUpdate")]
		public async Task<DataResult<AppointmentUpdateDto>> UpdateAppointment( [FromBody] AppointmentUpdateDto input)
		{
			try
			{  var appointment = await _userService.GetCurrentUserAsync();
				var resultEntityDto = await _appointmentService.UpdateAppointmentAsync(appointment.Id, input);
				return new SuccessDataResult<AppointmentUpdateDto>(resultEntityDto, "Appointment updated successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<AppointmentUpdateDto>("Appointmeyn updated error");

			}
		}
		
		
		 [HttpPost]
		 [Route("create")]
		public async Task<DataResult<AppointmentGetDto>> CreateAppointment(AppointmentCreateDto appointmentCreateDto)
    {
        var result = await _appointmentService.CreateAppointmentAsync(appointmentCreateDto);

			try
			{
				return new SuccessDataResult<AppointmentGetDto>("Appointment create successfuly ");
			}
			catch (Exception e)
			{

				return new ErrorDataResult<AppointmentGetDto>("Appointment create error");
			}
       
    }

		[HttpDelete("DeleteAppointment")]
		public async Task<DataResult<AppointmentGetDto>> DeleteAppointment()
        {
            try
            {
                var appointment = await _userService.GetCurrentUserAsync();
                await _appointmentService.DeleteByIdAsync(appointment.Id);
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
