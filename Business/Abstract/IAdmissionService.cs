using Business.Utils;
using Core.Business.DTO.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IAdmissionService : ICrudEntityService<AdmissionGetDto, AdmissionCreateDto, AdmissionUpdateDto>
	{
	}
}
