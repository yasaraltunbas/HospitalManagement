using Business.Abstract;
using Business.Concrete;
using Core.Business.DTO.MedicalRecord;
using Core.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MedicalRecordController:ControllerBase
	{
		private readonly IMedicalRecordService _medicalRecordService;
		private readonly ExceptionLogManager _exceptionLogManager;
		public MedicalRecordController(IMedicalRecordService medicalRecordService, ExceptionLogManager exceptionLogManager)
		{
			_medicalRecordService = medicalRecordService;
			_exceptionLogManager = exceptionLogManager;
		}

		[HttpPost]
        [Authorize(Roles = "Doctor")]

        public async Task<DataResult<MedicalRecordGetDto>> AddMedicalRecord([FromBody] MedicalRecordCreateDto input)
		{
			try
			{
				var resultEntityDto = await _medicalRecordService.AddAsync(input);
				return new SuccessDataResult<MedicalRecordGetDto>(resultEntityDto, "MedicalRecord created successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<MedicalRecordGetDto>("MedicalRecord created error");
			}
		}
		[HttpGet]

		public async Task<DataResult<ICollection<MedicalRecordGetDto>>> GetAllMedicalRecords()
		{
			try
			{
				var resultEntityDto = await _medicalRecordService.GetAllAsync();
				return new SuccessDataResult<ICollection<MedicalRecordGetDto>>(resultEntityDto, "MedicalRecords listed successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<ICollection<MedicalRecordGetDto>>("MedicalRecords listed error");
			}
		}
		[HttpGet("{id}")]
		public async Task<DataResult<MedicalRecordGetDto>> GetMedicalRecordById(int id)
		{
			try
			{
				var resultEntityDto = await _medicalRecordService.GetByIdAsync(id);
				return new SuccessDataResult<MedicalRecordGetDto>(resultEntityDto, "MedicalRecord listed successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<MedicalRecordGetDto>("MedicalRecord listed error");
			}
		}
		[HttpPut("GetByPatientId{id}")]
        [Authorize(Roles = "Doctor")]

        public async Task<DataResult<MedicalRecordGetDto>> UpdateMedicalRecord(int id, [FromBody] MedicalRecordUpdateDto input)
		{
			try
			{
				var resultEntityDto = await _medicalRecordService.UpdateAsync(id, input);
				return new SuccessDataResult<MedicalRecordGetDto>(resultEntityDto, "MedicalRecord updated successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<MedicalRecordGetDto>("MedicalRecord updated error");
			}
		}
		[HttpDelete("{id}")]
		public async Task<DataResult<MedicalRecordGetDto>> DeleteMedicalRecord(int id)
		{
			try
			{
				await _medicalRecordService.DeleteByIdAsync(id);
				return new SuccessDataResult<MedicalRecordGetDto>("MedicalRecord deleted successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<MedicalRecordGetDto>("MedicalRecord deleted error");
			}
		}
	}
}
