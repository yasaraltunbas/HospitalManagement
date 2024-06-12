using AutoMapper;
using Business.Abstract;
using Business.Utils;
using Core.Business.DTO.Doctor;
using Core.Business.DTO.Patient;
using DataAccess;
using HospitalManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class DoctorManager : CrudEntityManager<Doctor, DoctorGetDto, DoctorCreateDto , DoctorUpdateDto>, IDoctorService
	{
		public DoctorManager(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{
		}

		//public override async Task<DoctorGetDto> AddAsync(DoctorCreateDto input)
		//{
		//	await UnitOfWork.BeginTransactionAsync();
		//	try
		//	{
		//		var newDoctor = new Doctor
		//		{
		//			CreationDate = DateTime.UtcNow,
		//			IsDeleted = false,
		//			FirstName = input.FirstName,
		//			LastName = input.LastName,
		//			Email = input.Email,
		//			PhoneNumber = input.PhoneNumber,
		//			Address = input.Address,
		//			Speciality = input.Speciality
		//		};
		//		await BaseEntityRepository.AddAsync(newDoctor);
		//		await UnitOfWork.CommitTransactionAsync();
		//		var doctorDto = Mapper.Map<DoctorCreateDto, DoctorGetDto>(input);

		//		return doctorDto;
		//	}
		//	catch (Exception e)
		//	{
		//		await UnitOfWork.RollbackTransactionAsync();
		//		throw new Exception("Doctor could not be added", e);
		//	}
		//}
		//public override async Task<DoctorGetDto> UpdateAsync(int id, DoctorUpdateDto input)
		//{
		//	await UnitOfWork.BeginTransactionAsync();
		//	try
		//	{
		//		var doctor = await BaseEntityRepository.GetAsync(x => x.Id == id);
		//		if (doctor == null)
		//		{
		//			throw new Exception("Doctor not found");
		//		}
		//		doctor.FirstName = input.FirstName;
		//		doctor.LastName = input.LastName;
		//		doctor.Email = input.Email;
		//		doctor.PhoneNumber = input.PhoneNumber;
		//		doctor.Address = input.Address;
		//		doctor.Speciality = input.Speciality;
		//		await BaseEntityRepository.UpdateAsync(doctor);
		//		await UnitOfWork.CommitTransactionAsync();
		//		var doctorDto = Mapper.Map<Doctor, DoctorGetDto>(doctor);

		//		return doctorDto;
		//	}
		//	catch (Exception e)
		//	{
		//		await UnitOfWork.RollbackTransactionAsync();
		//		throw new Exception("Doctor could not be updated", e);
		//	}
		//}
		public override async Task<DoctorGetDto> DeleteByIdAsync(int id)
		{
			await UnitOfWork.BeginTransactionAsync();
			try
			{
				var doctor = await BaseEntityRepository.GetAsync(x =>  x.Id == id);
				if (doctor == null)
				{
					throw new Exception("Doctor not found");
				}
				doctor.IsDeleted = true;
				await BaseEntityRepository.UpdateAsync(doctor);
				await UnitOfWork.CommitTransactionAsync();
				var doctorDto = Mapper.Map<Doctor, DoctorGetDto>(doctor);

				return doctorDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();
				throw new Exception("Doctor could not be deleted", e);
			}
		}
		public async Task<ICollection<DoctorGetDto>> GetAllAsync()
		{
			var doctor = await BaseEntityRepository.GetListAsync();
			var doctorDto = Mapper.Map<ICollection<Doctor>, ICollection<DoctorGetDto>>(doctor);

			return doctorDto;
		}

	}
}
