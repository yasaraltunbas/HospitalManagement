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
	[Route("api/[controller]")]
	public class PatientController : ControllerBase
	{
		
		private readonly IPatientService _patientService;
		private readonly ExceptionLogManager _exceptionLogManager;
		public PatientController(IPatientService patientService, ExceptionLogManager exceptionLogManager)
		{
			_patientService = patientService;
			_exceptionLogManager = exceptionLogManager;
		}

		
		[HttpGet]
		[Authorize (Roles = "Patient")]
		public async Task<DataResult<ICollection<PatientGetDto>>> GetAllPatients()
		{
			try
			{
				var resultEntityDto = await _patientService.GetAllAsync();
				return new SuccessDataResult<ICollection<PatientGetDto>>(resultEntityDto, "Patients listed successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<ICollection<PatientGetDto>>("Patients listed error");
			}
		}
		[HttpGet("{id}")]
        [Authorize(Roles = "Doctor")]

        public async Task<DataResult<PatientGetDto>> GetPatientById(int id)
		{
			try
			{
				var resultEntityDto = await _patientService.GetByIdAsync(id);
				return new SuccessDataResult<PatientGetDto>(resultEntityDto, "Patient listed successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<PatientGetDto>("Patient listed error");
			}
		}
		[HttpPut("{id}")]
		public async Task<DataResult<PatientGetDto>> UpdatePatient(int id, [FromBody] PatientUpdateDto input)
		{
			try
			{
				var resultEntityDto = await _patientService.UpdateAsync(id, input);
				return new SuccessDataResult<PatientGetDto>(resultEntityDto, "Patient updated successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<PatientGetDto>("Patient updated error");
			}
		}
		[HttpDelete("{id}")]
		public async Task<DataResult<PatientGetDto>> DeletePatient(int id)
		{
			try
			{
				await _patientService.DeleteByIdAsync(id);
				return new SuccessDataResult<PatientGetDto>("Appointment deleted successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<PatientGetDto>("Appointmeny deleted error");
			}
		}

	}
}
