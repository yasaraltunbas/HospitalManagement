using Business.Utils;
using Core.Business.DTO.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IAppointmentService: ICrudEntityService<AppointmentGetDto, AppointmentCreateDto, AppointmentUpdateDto>
	{
		
		Task<bool> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto);
		Task<AppointmentUpdateDto> UpdateAppointmentAsync(int id, AppointmentUpdateDto appointmentUpdateDto);
		Task DeleteAppointmentAsync(int id);
	}
}

