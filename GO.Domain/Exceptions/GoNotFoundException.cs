using GO.Domain.Enums.Domain;
using GO.Domain.Resources;
using Humanizer;
using Microsoft.AspNetCore.Http;

namespace GO.Domain.Exceptions
{
	public sealed class GoNotFoundException
		: GoException
	{
		public GoNotFoundException(string entityName)
			: base(
				StatusCodes.Status404NotFound,
				ExceptionType.NotFound,
				ValidationResource.NotFound_Format.FormatWith(entityName))
		{
		}
	}
}
