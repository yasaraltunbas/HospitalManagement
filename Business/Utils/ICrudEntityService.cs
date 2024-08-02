using Core.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utils
{
    public interface ICrudEntityService<TEntityGetDto, TEntityCreateDto, TEntityUpdateDto>
		where TEntityGetDto : IEntityGetDto, new()
		where TEntityCreateDto : IDTO, new()
		where TEntityUpdateDto : IDTO, new()
	{
		Task<TEntityGetDto> AddAsync(TEntityCreateDto input);

		Task<TEntityGetDto?> UpdateAsync(int id, TEntityUpdateDto input);
		Task DeleteByIdAsync(int id);
		Task<TEntityGetDto> GetByIdAsync(int id);
		Task<ICollection<TEntityGetDto>> GetAllAsync();
	}
}
