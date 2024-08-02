using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
	public abstract class BaseEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public DateTime CreationDate { get; set; }
		public int CreatedByUserId { get; set; }
		public DateTime UpdateDate { get; set; }
		public int UpdateByUserId { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime DeletedDate { get; set; }
		public int DeletedByUserId { get; set; }



	}
}
