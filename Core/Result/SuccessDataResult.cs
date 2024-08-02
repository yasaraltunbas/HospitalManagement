using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Result
{
	public class SuccessDataResult<TEntity> : DataResult<TEntity>
	{
		public SuccessDataResult(TEntity data) : base(data, true)
		{
		}

		public SuccessDataResult(TEntity data, string message) : base(data, true, message)
		{
		}

		public SuccessDataResult(string message) : base(default, true, message)
		{
		}
	
	}
	
}
