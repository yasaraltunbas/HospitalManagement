using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Result
{
	public class DataResult<TEntity> : Result, IDataResult<TEntity>
	{
		public DataResult(TEntity data, bool success, string message) : base(success, message)
		{
			Data = data;
		}

		public DataResult(TEntity data, bool success) : base(success)
		{
			Data = data;
		}
		public TEntity Data { get; }
	}
}
