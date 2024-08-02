using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
	public class ExceptionLogRepository<TEntity,TContext> : IExceptionLogRepository<TEntity>
		where TEntity : ExceptionLog, new()
	   where TContext : DbContext
	{
		protected readonly TContext Context;
		protected readonly DbSet<TEntity> DbSet;

		public ExceptionLogRepository(TContext context)
		{
			Context = context;
			DbSet = context.Set<TEntity>();
		}
		

		public async Task AddLogAsync(TEntity input)
		{
			await DbSet.AddAsync(input);
			await Context.SaveChangesAsync();
		}

		

		public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await DbSet.Where(predicate).FirstOrDefaultAsync();
		}
	}

}
