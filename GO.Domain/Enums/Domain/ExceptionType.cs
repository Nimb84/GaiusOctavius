using System.ComponentModel;

namespace GO.Domain.Enums.Domain
{
	public enum ExceptionType
	{
		Unhandled = 0,

		Unsupported = 1,
		Forbidden = 2,
		NotFound = 3,
		Conflict = 4,
		Cast = 5,

		Validation = 100,

		Internal = 500,
	}
}
