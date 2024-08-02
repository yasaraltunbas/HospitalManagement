using Business.Abstract;
using Business.Concrete;
using Core.Business.DTO.Appointment;
using Core.Business.DTO.Patient;
using Core.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
	[ApiController]
	[Route("api/patient")]
	public class PatientController : ControllerBase
	{
		
		private readonly IPatientService _patientService;
		private readonly ExceptionLogManager _exceptionLogManager;
		public PatientController(IPatientService patientService, ExceptionLogManager exceptionLogManager)
		{
			_patientService = patientService;
			_exceptionLogManager = exceptionLogManager;
		}


        [HttpGet("appointments")]
        public async Task<ActionResult<List<AppointmentGetDto>>> GetAppointmentsByCurrentUser()
        {
            var appointments = await _patientService.GetAppointmentsByCurrentUserAsync();
            return Ok(appointments);
        }


        [HttpPut("UpdatePatient/{id}")]
		public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientUpdateDto patientDto)
		{
			var updatedPatient = await _patientService.UpdatePatientAsync(id, patientDto);
			if (updatedPatient == null)
			{
				return NotFound();
			}
			return Ok(updatedPatient);
		}

		[HttpDelete]
		//[Route("DeletePatient/{id}")]
		public async Task<DataResult<PatientGetDto>> DeletePatient(int id)
		{
			try
			{
				await _patientService.DeletePatientAsync(id);
				return new SuccessDataResult<PatientGetDto>("Patient deleted success");
			}
			catch (Exception e) { 
			await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<PatientGetDto>("Patient Deleted Error");
			}
		}

	}
}
