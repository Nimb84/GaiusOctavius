using GO.Domain.Enums.Domain;
using GO.Domain.Resources;
using Microsoft.AspNetCore.Http;

namespace GO.Domain.Exceptions
{
	public sealed class GoForbiddenException
		: GoException
	{
		public GoForbiddenException()
			: base(
				StatusCodes.Status403Forbidden,
				ExceptionType.Forbidden,
				ValidationResource.Forbidden)
		{
		}
	}
}
