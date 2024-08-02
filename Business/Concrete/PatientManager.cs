using AutoMapper;
using Business.Abstract;
using Core.Business.DTO.Appointment;
using Core.Business.DTO.Patient;
using HospitalManagement.DataAccess;
using HospitalManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrete
{
    public class PatientManager : IPatientService

    {
		private readonly HospitalDbContext _context;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;

		public PatientManager(HospitalDbContext context, IMapper mapper, IUserService userService) 
		{
			_context = context;
			_mapper = mapper;
			_userService = userService;


		}


        public async Task<List<AppointmentGetDto>> GetAppointmentsByCurrentUserAsync()
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            var userId = currentUser.Id;
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return new List<AppointmentGetDto>();
            }

            var appointments = await _context.Appointments
				.Include(a => a.Doctor)
				.Include(a => a.Doctor.User)
				.Include(a => a.Department)
                .Where(a => a.PatientId == patient.Id)
                .ToListAsync();

            var appointmentDtos = _mapper.Map<List<Appointment>, List<AppointmentGetDto>>(appointments);
            return appointmentDtos;
        }




        public async Task DeletePatientAsync(int id)
		{
			var patient = await _context.Patients.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
			try
			{
				if (patient == null)
				{
					throw new Exception("Patient not found");
				}

				patient.IsDeleted = true;
				patient.DeletedDate = DateTime.UtcNow;
				patient.User.IsDeleted = true;
				patient.User.DeletedDate = DateTime.UtcNow;


				await _context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				throw new Exception("Patient could not be deleted", e);
			}
		}

        public async Task<PatientGetDto> GetPatientByUserIdAsync(int userId)
        {
           var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return null;
            }

            var patientDto = _mapper.Map<Patient, PatientGetDto>(patient);
            return patientDto;


        }

        public async Task<PatientUpdateDto> UpdatePatientAsync(int id, PatientUpdateDto patientDto)
		{
			var patient = await _context.Patients.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);

			if (patient == null)
			{
				return null;
			}

			patient.Gender = patientDto.Gender;
			patient.BirthDay = patientDto.BirthDay;
			patient.BloodType = patientDto.BloodType;
			patient.User.Email = patientDto.Email;
			patient.User.FirstName = patientDto.FirstName;
			patient.User.LastName = patientDto.LastName;
			patient.User.PhoneNumber = patientDto.PhoneNumber;
			patient.User.Address = patientDto.Address;

			await _context.SaveChangesAsync();

			return patientDto;
		}
	}
}