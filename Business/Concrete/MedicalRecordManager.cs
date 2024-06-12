using AutoMapper;
using Business.Abstract;
using Business.Utils;
using Core.Business.DTO.Appointment;
using Core.Business.DTO.MedicalRecord;
using Core.Business.DTO.Patient;
using Core.DataAccess;
using DataAccess;
using HospitalManagement.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class MedicalRecordManager : CrudEntityManager<MedicalRecord, MedicalRecordGetDto, MedicalRecordCreateDto, MedicalRecordUpdateDto>, IMedicalRecordService
	{
		private readonly IEntityRepository<Doctor> _doctorRepository;
		private readonly IEntityRepository<Patient> _patientRepository;
		private readonly IEntityRepository<Appointment> _appointmentRepository;

		public MedicalRecordManager(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{
			_doctorRepository = unitOfWork.GenerateRepository<Doctor>();
			_patientRepository = unitOfWork.GenerateRepository<Patient>();
			_appointmentRepository = unitOfWork.GenerateRepository<Appointment>();
		}

		public override async Task<MedicalRecordGetDto> AddAsync(MedicalRecordCreateDto input)
		{

		
			
			await UnitOfWork.BeginTransactionAsync();
			
			try
			{
				var newMedicalRecord = new MedicalRecord
				{
					CreationDate = DateTime.UtcNow,
					PatientId = input.PatientId,
					DocterId = input.DoctorId,

					AppointmentId = input.AppointmentId,
					Diagnosis = input.Diagnosis,
					Treatment = input.Treatment,
					IsDeleted = false,
					Notes = input.Notes,
					Medication = input.Medication


				};
				await BaseEntityRepository.AddAsync(newMedicalRecord);
				await UnitOfWork.CommitTransactionAsync();
				var medicalRecordDto = Mapper.Map<MedicalRecordCreateDto, MedicalRecordGetDto>(input);

				return medicalRecordDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();
				throw new Exception("Medical Record could not be added", e);
			}
		}

		public async Task<MedicalRecordGetDto> UpdateAsync(MedicalRecordUpdateDto input)
		{
			await UnitOfWork.BeginTransactionAsync();
			
				var existingMedicalRecord = await BaseEntityRepository.GetByIdAsync(input.Id);

				if (existingMedicalRecord == null)
				{
					throw new Exception("Medical Record not found");
				}
				try
				{

					existingMedicalRecord.UpdateDate = DateTime.UtcNow;
					existingMedicalRecord.UpdateByUserId = input.Id;
					existingMedicalRecord.Diagnosis = input.Diagnosis;
					existingMedicalRecord.Treatment = input.Treatment;
					existingMedicalRecord.Notes = input.Notes;
					existingMedicalRecord.Medication = input.Medication;
					await BaseEntityRepository.UpdateAsync(existingMedicalRecord);
					await UnitOfWork.CommitTransactionAsync();
					var medicalRecordDto = Mapper.Map<MedicalRecordUpdateDto, MedicalRecordGetDto>(input);

					return medicalRecordDto;
				}
				catch (Exception e)
				{
					await UnitOfWork.RollbackTransactionAsync();
					throw new Exception("Medical Record could not be updated", e);
				}
			
	}

		public override async Task<MedicalRecordGetDto> DeleteByIdAsync(int id)
		{
			await UnitOfWork.BeginTransactionAsync();
			try
			{
				var existingMedicalRecord = await BaseEntityRepository.GetAsync(x => x.Id == id);

				if (existingMedicalRecord == null)
				{
					throw new Exception("Medical Record not found");
				}
				existingMedicalRecord.IsDeleted = true;
				await BaseEntityRepository.UpdateAsync(existingMedicalRecord);
				await UnitOfWork.CommitTransactionAsync();
				var medicalRecordDto = Mapper.Map<MedicalRecord, MedicalRecordGetDto>(existingMedicalRecord);

				return medicalRecordDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();
				throw new Exception("Medical Record could not be deleted", e);
			}
		}

		public override async Task<MedicalRecordGetDto> GetByIdAsync(int id)
		{
			var medicalRecord = await BaseEntityRepository.GetByIdAsync(id);
			if (medicalRecord == null)
			{
				throw new Exception("Medical Record not found");
			}
			var medicalRecordDto = Mapper.Map<MedicalRecord, MedicalRecordGetDto>(medicalRecord);
			return medicalRecordDto;
		}

		public override async Task<ICollection<MedicalRecordGetDto>> GetAllAsync()
		{
			var medicalRecord = await BaseEntityRepository.GetListAsync();
			var medicalRecordDto = Mapper.Map<ICollection<MedicalRecord>, ICollection<MedicalRecordGetDto>>(medicalRecord);

			return medicalRecordDto;
		}

		
		}
	} 
