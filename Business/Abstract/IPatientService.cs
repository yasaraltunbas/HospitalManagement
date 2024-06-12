using Business.Utils;
using Core.Business.DTO.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Abstract
{
	public interface IPatientService : ICrudEntityService<PatientGetDto, PatientCreateDto, PatientUpdateDto>

	{
	}
}
