using Core.Domain;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
	public interface IExceptionLogRepository<TEntity>
	   where TEntity : ExceptionLog, new ()
	{
		Task AddLogAsync(TEntity input);
		Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

	}
}
