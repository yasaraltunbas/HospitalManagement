using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
	public interface IEntityRepository<TEntity>
		where TEntity : BaseEntity, new()
	{
		Task AddAsync(TEntity input);
		Task<TEntity> GetByIdAsync(int id);
		Task UpdateAsync(TEntity input);
		Task DeleteAsync(TEntity input);
		Task DeleteByIdAsync(int id);
		Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
		Task<ICollection<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null);
	}
}
