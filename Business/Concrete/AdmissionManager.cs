using AutoMapper;
using Business.Abstract;
using Business.Utils;
using Core.Business.DTO.Admission;
using Core.Business.DTO.Appointment;
using Core.DataAccess;
using DataAccess;
using HospitalManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class AdmissionManager : CrudEntityManager<Admission, AdmissionGetDto, AdmissionCreateDto, AdmissionUpdateDto>, IAdmissionService
	{
		private readonly IEntityRepository<Doctor> _doctorRepository;
		private readonly IEntityRepository<Patient> _patientRepository;
		private readonly IEntityRepository<Department> _departmentRepository;

		public AdmissionManager(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{
			_doctorRepository = unitOfWork.GenerateRepository<Doctor>();
			_patientRepository = unitOfWork.GenerateRepository<Patient>();
			_departmentRepository = unitOfWork.GenerateRepository<Department>();
		}

		public override async Task<AdmissionGetDto> AddAsync(AdmissionCreateDto input)
		{
			await UnitOfWork.BeginTransactionAsync();

			var existingDoctor = await _doctorRepository.GetByIdAsync(input.DoctorId);

			var existingPatient = await _patientRepository.GetByIdAsync(input.PatientId);

			var existingDepartment = await _departmentRepository.GetByIdAsync(input.DepartmentId);

			if (existingDoctor == null || existingPatient == null || existingDepartment == null)
			{
				throw new Exception("Doctor or Patient or Department not found");
			}
			try
			{
				var newAdmission = new Admission
				{
					CreationDate = DateTime.UtcNow,
					PatientId = input.PatientId,
					DoctorId = input.DoctorId,
					DepartmentId = input.DepartmentId,
					Date = input.Date,
					DischargeDate = input.DischargeDate,
					Reason = input.Reason,
					IsDeleted = false
				};
				await BaseEntityRepository.AddAsync(newAdmission);
				await UnitOfWork.CommitTransactionAsync();

				var admissionDto = Mapper.Map<AdmissionCreateDto, AdmissionGetDto>(input);

				return admissionDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();

				throw new Exception("Appointment could not be added", e);

			}
		}

		public  async Task<AdmissionGetDto> UpdateAsync(AdmissionUpdateDto input)
		{
			await UnitOfWork.BeginTransactionAsync();
			try
			{
				var existingAdmission = await BaseEntityRepository.GetByIdAsync(input.Id);

				if (existingAdmission == null)
				{
					throw new Exception("Admission not found");
				}

		
				existingAdmission.Date = input.Date;
				existingAdmission.DischargeDate = input.DischargeDate;
				existingAdmission.Reason = input.Reason;

				await BaseEntityRepository.UpdateAsync(existingAdmission);
				await UnitOfWork.CommitTransactionAsync();

				var admissionDto = Mapper.Map<AdmissionUpdateDto, AdmissionGetDto>(input);

				return admissionDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();

				throw new Exception("Admission could not be updated", e);

			}
		}

		public async Task<AdmissionGetDto> GetAdmissionByIdAsync(int id)
		{
			var admission = await BaseEntityRepository.GetByIdAsync(id);

			if (admission == null)
			{
				throw new Exception("Admission not found");
			}

			var admissionDto = Mapper.Map<Admission, AdmissionGetDto>(admission);

			return admissionDto;
		}

		public override async Task<AdmissionGetDto> DeleteByIdAsync(int id)
		{
			await UnitOfWork.BeginTransactionAsync();
			try
			{
				var existingAdmission = await BaseEntityRepository.GetAsync(x => x.Id == id);

				if (existingAdmission == null)
				{
					throw new Exception("Admission not found");
				}
				existingAdmission.IsDeleted = true;
				existingAdmission.DeletedDate = DateTime.UtcNow;
				existingAdmission.DeletedByUserId = id;
				await BaseEntityRepository.UpdateAsync(existingAdmission);
				await UnitOfWork.CommitTransactionAsync();

				var admissionDto = Mapper.Map<Admission, AdmissionGetDto>(existingAdmission);

				return admissionDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();
				throw new Exception("Admission could not be deleted", e);
			}
		}

		public async Task<ICollection<AdmissionGetDto>> GetAllAsync()
		{
			var admission = await BaseEntityRepository.GetListAsync();
			var admissionDto = Mapper.Map<ICollection<Admission>, ICollection<AdmissionGetDto>>(admission);

			return admissionDto;
		}
	}
}
