using GO.Domain.Enums.Domain;
using Humanizer;

namespace GO.Domain.Exceptions
{
	public sealed class ErrorDetails
	{
		public int Code { get; set; }

		public string Message { get; set; }

		public string FieldName { get; set; }

		public ErrorDetails(ExceptionType code)
			: this(code, code.Humanize())
		{
		}

		public ErrorDetails(ExceptionType code, string message)
			: this(code, message, string.Empty)
		{
		}

		public ErrorDetails(ExceptionType code, string message, string fieldName)
		{
			Code = (int)code;
			Message = message;
			FieldName = fieldName;
		}
	}
}
