using Business.Abstract;
using Core.DataAccess;
using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class ExceptionLogManager : IExceptionLogService
	{
		protected readonly IExceptionLogRepository<ExceptionLog> _exceptionLogRepository;

		protected readonly IUnitOfWorks UnitOfWork;
		public ExceptionLogManager(IUnitOfWorks unitOfWorks  )
		{
			UnitOfWork = unitOfWorks;
			_exceptionLogRepository = UnitOfWork.GenerateExceptionLogRepository<ExceptionLog>();
			
		}
		
		public async Task LogExceptionAsync(Exception e)
		{
			await UnitOfWork.BeginTransactionAsync();
			try
			{
				ExceptionLog input = new ExceptionLog();
				input.ExceptionMessage = e.Message;
				input.StackTrace = e.StackTrace ?? string.Empty;
				input.LogDateTime = DateTime.UtcNow;
				await _exceptionLogRepository.AddLogAsync(input);
				await UnitOfWork.CommitTransactionAsync();
			}
			catch (Exception ex)
			{
				await UnitOfWork.RollbackTransactionAsync();
				throw new Exception("ExceptionLog could not be added", ex);
			}
		}
		public async Task<ExceptionLog> GetAsync(int id)
		{
			var entity = await _exceptionLogRepository.GetAsync(x => x.Id == id);
			return entity;
		}

	}
}
