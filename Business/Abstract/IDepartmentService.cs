using Business.Utils;
using Core.Business.DTO.Department;
using Core.Business.DTO.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IDepartmentService : ICrudEntityService<DepartmentGetDto, DepartmentCreateDto, DepartmentUpdateDto>
	{
        Task<List<DoctorGetDto>> GetDoctorsByDepartmentAsync(int departmentId);
    }
}
