using Business.Abstract;
using Business.Concrete;
using Core.Business.DTO.Department;
using Core.Result;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DepartmentController : ControllerBase
	{
		private readonly IDepartmentService _departmentService;
		private readonly ExceptionLogManager _exceptionLogManager;
		public DepartmentController(IDepartmentService departmentService, ExceptionLogManager exceptionLogManager)
		{
			_departmentService = departmentService;
			_exceptionLogManager = exceptionLogManager;
		}

		[HttpPost]
		public async Task<DataResult<DepartmentGetDto>> AddDepartment([FromBody] DepartmentCreateDto input)
		{
			
			try
			{
				var resultEntityDto = await _departmentService.AddAsync(input);
				return new SuccessDataResult<DepartmentGetDto>(resultEntityDto, "Department created successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<DepartmentGetDto>("Department created error");
			}

		}
		[HttpGet]
		public async Task<DataResult<ICollection<DepartmentGetDto>>> GetAllDepartments()
		{
			try
			{
				var resultEntityDto = await _departmentService.GetAllAsync();
				return new SuccessDataResult<ICollection<DepartmentGetDto>>(resultEntityDto, "Departments listed successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<ICollection<DepartmentGetDto>>("Departments listed error");
			}
		}
		[HttpGet("{id}")]
		public async Task<DataResult<DepartmentGetDto>> GetDepartmentById(int id)
		{
			try
			{
				var resultEntityDto = await _departmentService.GetByIdAsync(id);
				return new SuccessDataResult<DepartmentGetDto>(resultEntityDto, "Department listed successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<DepartmentGetDto>("Department listed error");
			}
		}
		[HttpPut("{id}")]
		public async Task<DataResult<DepartmentGetDto>> UpdateDepartment(int id, [FromBody] DepartmentUpdateDto input)
		{
			try
			{
				var resultEntityDto = await _departmentService.UpdateAsync(id, input);
				return new SuccessDataResult<DepartmentGetDto>(resultEntityDto, "Department updated successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<DepartmentGetDto>("Department updated error");
			}
		}
		[HttpDelete("{id}")]
		public async Task<DataResult<DepartmentGetDto>> DeleteDepartment(int id)
		{
			try
			{
				await _departmentService.DeleteByIdAsync(id);
				return new SuccessDataResult<DepartmentGetDto>("Department deleted successfuly");
			}
			catch (Exception e)
			{
				await _exceptionLogManager.LogExceptionAsync(e);
				return new ErrorDataResult<DepartmentGetDto>("Department deleted error");
			}
		}
	}
}
