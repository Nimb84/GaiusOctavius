using System.Collections.Generic;

namespace GO.Domain.Models
{
	public sealed class PagedList<TType>
	{
		public List<TType> ItemList { get; set; }

		public ushort Total { get; set; }
	}
}
