using Core.DataAccess;
using Core.Domain;
using Domain;
using HospitalManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public class UnitOfWorks : IUnitOfWorks
	{
		private readonly HospitalDbContext _context;

		public UnitOfWorks(HospitalDbContext context)
		{
			_context = context;
		}

		public IEntityRepository<TEntity> GenerateRepository<TEntity>() where TEntity : BaseEntity, new()
		{
			return new EntityRepositoryBase<TEntity, HospitalDbContext>(_context);
		}

		public async Task BeginTransactionAsync()
		{
			await _context.Database.BeginTransactionAsync();
		}

		public async Task RollbackTransactionAsync()
		{
			await _context.Database.RollbackTransactionAsync();
		}

		public async Task CommitTransactionAsync()
		{
			await _context.Database.CommitTransactionAsync();
		}

		public IExceptionLogRepository<TEntity> GenerateExceptionLogRepository<TEntity>() where TEntity : ExceptionLog, new()
		{
			return new ExceptionLogRepository<TEntity, HospitalDbContext>(_context);
		}
	}
	
	
}
