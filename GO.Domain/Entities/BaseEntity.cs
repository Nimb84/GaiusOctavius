using System;

namespace GO.Domain.Entities
{
	public class BaseEntity
	{
		public Guid Id { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTimeOffset CreatedDate { get; set; }

		public Guid? UpdatedBy { get; set; }

		public DateTimeOffset? UpdatedDate { get; set; }
	}
}
