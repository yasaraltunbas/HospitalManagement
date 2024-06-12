using AutoMapper;
using Business.Abstract;
using Business.Utils;
using Core.Business.DTO.Appointment;
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
	public class PatientManager : CrudEntityManager<Patient, PatientGetDto, PatientCreateDto, PatientUpdateDto>, IPatientService

	{
		public PatientManager(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{


		}

	//	public override async Task<PatientGetDto> AddAsync(PatientCreateDto input)
	//	{
	//		await UnitOfWork.BeginTransactionAsync();
	//		try {
	//			var newPatient = new Patient
	//			{
	//				CreationDate = DateTime.UtcNow,
	//				FirstName = input.FirstName,
	//				LastName = input.LastName,
	//				Email = input.Email,
	//				gender = input.gender,
	//				PhoneNumber = input.PhoneNumber,
	//				Address = input.Address,
	//				BirthDay = input.BirthDay,
	//				BloodType = input.BloodType
	//			};
	//				await BaseEntityRepository.AddAsync(newPatient);
	//				await UnitOfWork.CommitTransactionAsync();
	//			var patientDto = Mapper.Map<PatientCreateDto, PatientGetDto>(input);

	//			return patientDto;
	//		}
	//		catch (Exception e)
	//		{
	//			await UnitOfWork.RollbackTransactionAsync();
	//			throw new Exception("Patient could not be added", e);

	//		}

	//}
		//public override async  Task<PatientGetDto> UpdateAsync(int id, PatientUpdateDto input)
		//{
		//	await UnitOfWork.BeginTransactionAsync();
		//	try
		//	{
		//		var patient = await BaseEntityRepository.GetAsync(x => x.Id == id);
		//		if (patient == null)
		//		{
		//			throw new Exception("Patient not found");
		//		}
		//		patient.FirstName = input.FirstName;
		//		patient.LastName = input.LastName;
		//		patient.Email = input.Email;
		//		patient.PhoneNumber = input.PhoneNumber;
		//		patient.Address = input.Address;
		//		patient.BirthDay = input.BirthDay;
		//		patient.BloodType = input.BloodType;
				
		//		await BaseEntityRepository.UpdateAsync(patient);
		//		await UnitOfWork.CommitTransactionAsync();
		//		var patientDto = Mapper.Map<Patient, PatientGetDto>(patient);

		//		return patientDto;
		//	}
		//	catch (Exception e)
		//	{
		//		await UnitOfWork.RollbackTransactionAsync();
		//		throw new Exception("Patient could not be updated", e);
		//	}
		//}
		
		public override async Task DeleteByIdAsync(int id)
		{
			await UnitOfWork.BeginTransactionAsync();
			try
			{
				var existingPatient = await BaseEntityRepository.GetAsync(x =>  x.Id == id);

				if (existingPatient == null)
				{
					throw new Exception("Patient not found");
				}

				existingPatient.IsDeleted = true;
				existingPatient.DeletedDate = DateTime.UtcNow;

				await BaseEntityRepository.UpdateAsync(existingPatient);
				await UnitOfWork.CommitTransactionAsync();
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();
				throw new Exception("Patient could not be deleted", e);
			}
		}

		public override async Task<ICollection<PatientGetDto>> GetAllAsync()
		{
			var patients = await BaseEntityRepository.GetListAsync();
			var patientDtos = Mapper.Map<ICollection<Patient>, ICollection<PatientGetDto>>(patients);

			return patientDtos;
		}
	}
}