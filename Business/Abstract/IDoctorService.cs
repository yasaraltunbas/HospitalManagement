using Business.Utils;
using Core.Business.DTO.Appointment;
using Core.Business.DTO.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IDoctorService: ICrudEntityService<DoctorGetDto, DoctorCreateDto, DoctorUpdateDto>
	{
        Task<List<AppointmentGetDto>> GetAppointmentsByCurrentDoctorAsync();

    }
}
