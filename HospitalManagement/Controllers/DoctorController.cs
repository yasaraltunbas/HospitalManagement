using Business.Abstract;
using Business.Concrete;
using Core.Business.DTO.Appointment;
using Core.Business.DTO.Doctor;
using Core.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
	[ApiController]
	[Route("api/doctor")]
	public class DoctorController:ControllerBase
	{
		
		private readonly IDoctorService _doctorService;
		private readonly ExceptionLogManager _exceptionLogManager;
		public DoctorController(IDoctorService doctorService, ExceptionLogManager exceptionLogManager)
		{
			_doctorService = doctorService;
			_exceptionLogManager = exceptionLogManager;
		}

		
		[HttpGet]
		//[Authorize (Roles = "Doctor")]
		public async Task<DataResult<ICollection<DoctorGetDto>>> GetAllDoctors()
		{
			try
			{
				var resultEntityDto = await _doctorService.GetAllAsync();
				return new SuccessDataResult<ICollection<DoctorGetDto>>(resultEntityDto, "Doctors listed successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<ICollection<DoctorGetDto>>("Doctors listed error");
			}
		}
		[HttpGet("{id}")]
       // [Authorize(Roles = "Doctor")]

        public async Task<DataResult<DoctorGetDto>> GetDoctorById(int id)
		{
			try
			{
				var resultEntityDto = await _doctorService.GetByIdAsync(id);
				return new SuccessDataResult<DoctorGetDto>(resultEntityDto, "Doctor listed successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<DoctorGetDto>("Doctor listed error");
			}
		}
		[HttpPut("{id}")]
		public async Task<DataResult<DoctorGetDto>> UpdateDoctor(int id, [FromBody] DoctorUpdateDto input)
		{
			try
			{
				var resultEntityDto = await _doctorService.UpdateAsync(id, input);
				return new SuccessDataResult<DoctorGetDto>(resultEntityDto, "Doctor updated successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<DoctorGetDto>("Doctor updated error");
			}
		}
		[HttpDelete("{id}")]
		public async Task<DataResult<DoctorGetDto>> DeleteDoctor(int id)
		{
			try
			{
				 await _doctorService.DeleteByIdAsync(id);
				return new SuccessDataResult<DoctorGetDto>( "Doctor deleted successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<DoctorGetDto>("Doctor deleted error");
			}
		}

        [HttpGet("appointments")]
        public async Task<ActionResult<List<AppointmentGetDto>>> GetAppointmentsByCurrentUser()
        {
            var appointments = await _doctorService.GetAppointmentsByCurrentDoctorAsync();
            return Ok(appointments);
        }
    }
}
