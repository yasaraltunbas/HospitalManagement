using Business.Utils;
using Core.Business.DTO.Appointment;
using Core.Business.DTO.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Abstract
{
	public interface IPatientService 

	{
		Task<PatientUpdateDto> UpdatePatientAsync(int id, PatientUpdateDto patientDto);
		Task DeletePatientAsync(int id);
        Task<PatientGetDto> GetPatientByUserIdAsync(int userId);

		Task<List<AppointmentGetDto>> GetAppointmentsByCurrentUserAsync();

    }
}
