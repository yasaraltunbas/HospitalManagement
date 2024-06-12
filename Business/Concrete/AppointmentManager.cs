using AutoMapper;
using Business.Abstract;
using Business.Utils;
using Core.Business.DTO.Appointment;
using Core.DataAccess;
using DataAccess;
using HospitalManagement.DataAccess;
using HospitalManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class AppointmentManager : CrudEntityManager<Appointment, AppointmentGetDto, AppointmentCreateDto, AppointmentUpdateDto>, IAppointmentService
	{
		private readonly IEntityRepository<Doctor> _doctorRepository;
		private readonly IEntityRepository<Patient> _patientRepository;	


		public AppointmentManager(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{
			_doctorRepository = unitOfWork.GenerateRepository<Doctor>();
			_patientRepository = unitOfWork.GenerateRepository<Patient>();

		}

		public override async Task<AppointmentGetDto> AddAsync(AppointmentCreateDto input)
		{

			

			await UnitOfWork.BeginTransactionAsync();


			try
			{
				var newAppointment = new Appointment
				{
					CreationDate = DateTime.UtcNow,
					CreatedByUserId = input.PatientId,
					IsDeleted = false,
					DoctorId = input.DoctorId,
					PatientId = input.PatientId,
					Date = input.Date,
					Reason = input.Reason


				};
				await BaseEntityRepository.AddAsync(newAppointment);
				await UnitOfWork.CommitTransactionAsync();

				var appointmenDto = Mapper.Map<AppointmentCreateDto, AppointmentGetDto>(input);

				return appointmenDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();
				
				throw new Exception("Appointment could not be added", e);

			}
		}
		public  async Task<AppointmentGetDto> UpdateAsync(AppointmentUpdateDto input)
		{
			await UnitOfWork.BeginTransactionAsync();
			try
			{
				var existingAppointment = await BaseEntityRepository.GetByIdAsync(input.Id);

				if (existingAppointment == null)
				{
					throw new Exception("Appointment not found");
				}

				existingAppointment.Date = input.Date;
				existingAppointment.Reason = input.Reason;
				existingAppointment.UpdateDate = DateTime.UtcNow;
				await BaseEntityRepository.UpdateAsync(existingAppointment);
				await UnitOfWork.CommitTransactionAsync();

				var appointmentDto = Mapper.Map<AppointmentUpdateDto, AppointmentGetDto>(input);

				return appointmentDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();
				throw new Exception("Appointment could not be updated", e);
			}
		}
		public override async Task<AppointmentGetDto> DeleteByIdAsync(int id)
		{
			await UnitOfWork.BeginTransactionAsync();
			try
			{
				var existingAppointment = await BaseEntityRepository.GetAsync(x => x.Id == id);

				if (existingAppointment == null)
				{
					throw new Exception("Appointment not found");
				}
				existingAppointment.IsDeleted = true;
				existingAppointment.DeletedDate = DateTime.UtcNow;
				existingAppointment.DeletedByUserId = id;
				await BaseEntityRepository.UpdateAsync(existingAppointment);
				await UnitOfWork.CommitTransactionAsync();

				var appointmentDto = Mapper.Map<Appointment, AppointmentGetDto>(existingAppointment);

				return appointmentDto;
			}
			catch (Exception e)
			{
				await UnitOfWork.RollbackTransactionAsync();
				throw new Exception("Appointment could not be deleted", e);
			}
		}
		public async Task<ICollection<AppointmentGetDto>> GetAllAsync()
		{
			var appointments = await BaseEntityRepository.GetListAsync();
			var appointmentDtos = Mapper.Map<ICollection<Appointment>, ICollection<AppointmentGetDto>>(appointments);

			return appointmentDtos;
		}
	}
}
