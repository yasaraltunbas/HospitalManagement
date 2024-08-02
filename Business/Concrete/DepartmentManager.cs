using AutoMapper;
using Business.Abstract;
using Business.Utils;
using Core.Business.DTO.Department;
using Core.Business.DTO.Doctor;
using Core.Business.DTO.Patient;
using DataAccess;
using HospitalManagement.DataAccess;
using HospitalManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class DepartmentManager : CrudEntityManager<Department, DepartmentGetDto, DepartmentCreateDto, DepartmentUpdateDto>, IDepartmentService
	{
        private readonly HospitalDbContext _context;

        public DepartmentManager(IUnitOfWorks unitOfWork, IMapper mapper, HospitalDbContext context) : base(unitOfWork, mapper)
        {

            _context = context;

		}

		
		public override async Task<DepartmentGetDto> UpdateAsync(int id, DepartmentUpdateDto input)
		{
			await UnitOfWork.BeginTransactionAsync();
			try
			{
				var department = await BaseEntityRepository.GetAsync(x => x.Id == id);
				if (department == null)
				{
					throw new Exception("Department not found");
				}
				department.UpdateDate = DateTime.UtcNow;
				department.UpdateByUserId = id;
				department.DepartmentName = input.DepartmentName;
				department.Location = input.Location;
				await BaseEntityRepository.UpdateAsync(department);
				await UnitOfWork.CommitTransactionAsync();
				var departmentDto = Mapper.Map<Department, DepartmentGetDto>(department);

				return departmentDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();
				throw new Exception("Department could not be updated", e);
			}
		}
        public async Task<List<DoctorGetDto>> GetDoctorsByDepartmentAsync(int departmentId)
        {
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Where(d => d.DepartmentId == departmentId && !d.IsDeleted)
                .Select(d => new DoctorGetDto
                {
                    Id = d.Id,
                    FirstName = d.User.FirstName,
                    LastName = d.User.LastName,
                    DepartmentId = d.DepartmentId
                })
                .ToListAsync();

            return doctors;
        }
        public override async Task<DepartmentGetDto> DeleteByIdAsync(int id)
		{
			await UnitOfWork.BeginTransactionAsync();
			try
			{
				var existingDepartment = await BaseEntityRepository.GetAsync(x =>  x.Id == id);
				if (existingDepartment == null)
				{
					throw new Exception("Department not found");
				}
				existingDepartment.IsDeleted = true;
				await BaseEntityRepository.UpdateAsync(existingDepartment);
				await UnitOfWork.CommitTransactionAsync();
				var departmentDto = Mapper.Map<Department, DepartmentGetDto>(existingDepartment);

				return departmentDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();
				throw new Exception("Department could not be deleted", e);
			}
		}	

		

		public async Task<ICollection<DepartmentGetDto>> GetAllDepartmentsAsync()
		{
				var department = await BaseEntityRepository.GetListAsync();
			var departmentDto = Mapper.Map<ICollection<Department>, ICollection<DepartmentGetDto>>(department);

			return departmentDto;
		}

	}
}
