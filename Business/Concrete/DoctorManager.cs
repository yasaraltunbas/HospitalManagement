using AutoMapper;
using Business.Abstract;
using Business.Utils;
using Core.Business.DTO.Appointment;
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
    public class DoctorManager : CrudEntityManager<Doctor, DoctorGetDto, DoctorCreateDto, DoctorUpdateDto>, IDoctorService
    {
		private readonly IUserService _userService;
		private readonly HospitalDbContext _context;
		private readonly IMapper _mapper;
		public DoctorManager(IUnitOfWorks unitOfWork, IMapper mapper, IUserService userService, HospitalDbContext hospitalDbContext) : base(unitOfWork, mapper)
		{
			_userService = userService;
            _context = hospitalDbContext;
			_mapper = mapper;
		}

	
	
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

        public async Task<List<AppointmentGetDto>> GetAppointmentsByCurrentDoctorAsync()
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            var userId = currentUser.Id;
            var doctor = await _context.Doctors.FirstOrDefaultAsync(p => p.UserId == userId);

            if (doctor == null)
            {
                return new List<AppointmentGetDto>();
            }

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Patient.User)
                .Include(a => a.Department)
                .Where(a => a.DoctorId == doctor.Id)
                .ToListAsync();

            var appointmentDtos = _mapper.Map<List<Appointment>, List<AppointmentGetDto>>(appointments);
            return appointmentDtos;


        }
    }
}
