using AutoMapper;
using Business.Abstract;
using Business.Utils;
using Core.Business.DTO.Appointment;
using Core.DataAccess;
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
	public class AppointmentManager : CrudEntityManager<Appointment, AppointmentGetDto, AppointmentCreateDto, AppointmentUpdateDto>, IAppointmentService
	{
		private readonly IEntityRepository<Doctor> _doctorRepository;
		private readonly IEntityRepository<Patient> _patientRepository;
		private readonly HospitalDbContext _context;
		private readonly IPatientService _patientService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public AppointmentManager(IUnitOfWorks unitOfWork, IMapper mapper, HospitalDbContext context, IPatientService patientService, IUserService userService) : base(unitOfWork, mapper)
		{
			_doctorRepository = unitOfWork.GenerateRepository<Doctor>();
			_patientRepository = unitOfWork.GenerateRepository<Patient>();

			_context = context;
			_patientService = patientService;
			_userService = userService;
			_mapper = mapper;
		}
		public async Task<AppointmentGetDto> UpdateAsync(AppointmentUpdateDto input)
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
		public async Task<bool> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto)
		{
			var patientId = appointmentCreateDto.PatientId;

			if (patientId == null)
			{
				var currentUser = await _userService.GetCurrentUserAsync();
				var userId = currentUser.Id;
				var currentpatient = await _patientService.GetPatientByUserIdAsync(userId);
				patientId = currentpatient?.Id;


			}
			var patient = await _context.Patients.FindAsync(patientId);
			var department = await _context.Departments.FindAsync(appointmentCreateDto.DepartmentId);

			if (patient == null || department == null)
			{
				return false;
			}

			if (patient.Balance >= department.Fee)
			{
				patient.Balance -= department.Fee;
			}
			else
			{
				patient.Debt += (department.Fee - patient.Balance);
				patient.Balance = 0;
			}

			var appointment = new Appointment
			{
				PatientId = patientId.Value,
				DoctorId = appointmentCreateDto.DoctorId,
				DepartmentId = appointmentCreateDto.DepartmentId,
				Date = appointmentCreateDto.Date.ToUniversalTime(),
				Reason = appointmentCreateDto.Reason

			};

			_context.Appointments.Add(appointment);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<AppointmentUpdateDto> UpdateAppointmentAsync(int id, AppointmentUpdateDto appointmentUpdateDto)
		{

			try
			{
				var existingAppointment = await _context.Appointments.FindAsync(id);

				if (existingAppointment == null)
				{
					throw new Exception("Appointment not found");
				}

				existingAppointment.Date = appointmentUpdateDto.Date.ToUniversalTime();
				existingAppointment.Reason = appointmentUpdateDto.Reason;
				existingAppointment.UpdateDate = DateTime.UtcNow;

				_context.Appointments.Update(existingAppointment);
				await _context.SaveChangesAsync();

				var updatedAppointmentDto = _mapper.Map<Appointment, AppointmentUpdateDto>(existingAppointment);

				return updatedAppointmentDto;
			}
			catch (Exception e)
			{
				throw new Exception("Appointment could not be updated", e);
			}
		}

		public async Task DeleteAppointmentAsync(int id)
		{
			var appointment = _context.Appointments.Find(id);

		try
            {
			if (appointment == null)
			{
				throw new Exception("Appointment not found");
			}

			appointment.IsDeleted = true;
			appointment.DeletedDate = DateTime.UtcNow;

			await _context.SaveChangesAsync();

		}
			catch (Exception e)
			{
                throw new Exception("Appointment could not be deleted", e);
            }
        }
    }
}
