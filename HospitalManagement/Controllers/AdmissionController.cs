using Business.Abstract;
using Business.Concrete;
using Core.Business.DTO.Admission;
using Core.Result;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AdmissionController : ControllerBase
	{
		private readonly IAdmissionService _admissionService;
		private readonly ExceptionLogManager _exceptionLogManager;
		public AdmissionController(IAdmissionService admissionService, ExceptionLogManager exceptionLogManager)
		{
			_admissionService = admissionService;
			_exceptionLogManager = exceptionLogManager;
		}

		[HttpPost]
		public async Task<DataResult<AdmissionGetDto>> AddAdmission([FromBody] AdmissionCreateDto input)
		{
			try
			{
				var resultEntityDto = await _admissionService.AddAsync(input);
				return new SuccessDataResult<AdmissionGetDto>(resultEntityDto, "Admission Created Success");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<AdmissionGetDto>("Admission Created Failed");
				
			}


		}
		[HttpGet]
		public async Task<DataResult<ICollection<AdmissionGetDto>>> GetAllAdmissions()
		{
			try
			{
				var resultEntityDto = await _admissionService.GetAllAsync();
				return new SuccessDataResult<ICollection<AdmissionGetDto>>(resultEntityDto, "Admissions Listed Success");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<ICollection<AdmissionGetDto>>("Admissions Listed Failed");
			}
			
		}
		[HttpGet("{id}")]
		public async Task<DataResult<AdmissionGetDto>> GetAdmissionById(int id)
		{
			try
			{
				var resultEntityDto = await _admissionService.GetByIdAsync(id);
				return new SuccessDataResult<AdmissionGetDto>(resultEntityDto, "Admission Listed Success");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<AdmissionGetDto>("Admission Listed Failed");
			}
			
		}
		[HttpPut("{id}")]
		public async Task<DataResult<AdmissionGetDto>> UpdateAdmission(int id, [FromBody] AdmissionUpdateDto input)
		{
			try
			{
				var resultEntityDto = await _admissionService.UpdateAsync(id, input);
				return new SuccessDataResult<AdmissionGetDto>(resultEntityDto, "Admission Updated Success");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<AdmissionGetDto>("Admission Updated Failed");
			}
		}
		[HttpDelete("{id}")]
		public async Task<DataResult<AdmissionGetDto>> DeleteAdmission(int id)
		{
			try
			{
				await _admissionService.DeleteByIdAsync(id);
				return new SuccessDataResult<AdmissionGetDto>( "Admission Deleted Success");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<AdmissionGetDto>("Admission Deleted Failed");
			}
		}
	}
}
