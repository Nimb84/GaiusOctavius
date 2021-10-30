using System;

namespace GO.Domain.Models
{
	public sealed record SecurityToken(
		Guid Key,
		Guid Issuer,
		DateTime TimeStamp);
}
