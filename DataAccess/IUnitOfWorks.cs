using Core.DataAccess;
using Core.Domain;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public interface IUnitOfWorks
	{
		IEntityRepository<TEntity> GenerateRepository<TEntity>() where TEntity : BaseEntity, new();
		IExceptionLogRepository<TEntity> GenerateExceptionLogRepository<TEntity>() where TEntity : ExceptionLog, new();

		Task BeginTransactionAsync();
		Task RollbackTransactionAsync();
		Task CommitTransactionAsync();
	}
}
