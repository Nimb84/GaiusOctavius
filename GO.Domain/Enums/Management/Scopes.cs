using System;

namespace GO.Domain.Enums.Management
{
	[Flags]
	public enum Scopes : byte
	{
		None = 1 << 0,
		Administration = 1 << 1,
		Management = 1 << 2,
		Budget = 1 << 3
	}
}
