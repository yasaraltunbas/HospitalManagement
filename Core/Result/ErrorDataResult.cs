using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Result
{
	public class ErrorDataResult<TEntity> : DataResult<TEntity>
	{
		public ErrorDataResult() : base(default, false)
		{
		}

		public ErrorDataResult(string message) : base(default, false, message)
		{
		}

		public ErrorDataResult(TEntity data) : base(data, false)
		{
		}

		public ErrorDataResult(TEntity data, string message) : base(data, false, message)
		{
		}
	}
	
}
