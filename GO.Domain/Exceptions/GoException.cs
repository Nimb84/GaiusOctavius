using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GO.Domain.Enums.Domain;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GO.Domain.Exceptions
{
	public class GoException
		: Exception
	{
		public string Title { get; set; }

		public string Details { get; set; }

		public int StatusCode { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Guid? TraceId { get; set; }

		public IEnumerable<ErrorDetails> Errors { get; set; }

		[JsonIgnore]
		public Exception OriginalException { get; set; }

		public GoException(
			int statusCode,
			ExceptionType errorCode,
			string errorCodeMessage,
			string fieldName,
			string title = null,
			string details = "Please refer to errors collection for more detailed information if present",
			Guid? traceId = null,
			Exception originalException = null)
			: this(
				statusCode,
				new List<ErrorDetails> { new(errorCode, errorCodeMessage, fieldName) },
				title,
				details,
				traceId,
				originalException)
		{
		}

		public GoException(
			int statusCode,
			ExceptionType errorCode,
			string title = null,
			string details = "Please refer to errors collection for more detailed information if present",
			Guid? traceId = null,
			Exception originalException = null)
			: this(statusCode, title, details, new List<ExceptionType> { errorCode }, traceId, originalException)
		{
		}

		public GoException(
			int statusCode,
			string title = null,
			string details = "Please refer to errors collection for more detailed information if present",
			IReadOnlyCollection<ExceptionType> errors = null,
			Guid? traceId = null,
			Exception originalException = null)
			: this(
				statusCode,
				errors != null && errors.Any() ? errors.Select(code => new ErrorDetails(code)).ToList() : new List<ErrorDetails>(),
				title,
				details,
				traceId,
				originalException)
		{
		}

		public GoException(
			int statusCode,
			List<ErrorDetails> errorDetails = null,
			string title = null,
			string details = "Please refer to errors collection for more detailed information if present",
			Guid? traceId = null,
			Exception originalException = null)
		{
			Title = title ?? GetDefaultTitle(statusCode);
			Details = details ?? GetDefaultDetails(statusCode);
			StatusCode = statusCode;
			TraceId = traceId;
			OriginalException = originalException;
			Errors = errorDetails ?? new List<ErrorDetails>();
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(Title), Title);
			info.AddValue(nameof(Details), Details);
			info.AddValue(nameof(StatusCode), StatusCode);
			info.AddValue(nameof(TraceId), TraceId);
			info.AddValue(nameof(OriginalException), OriginalException);
			base.GetObjectData(info, context);
		}

		private static string GetDefaultTitle(int statusCode) =>
			statusCode switch
			{
				StatusCodes.Status400BadRequest => "Bad Request",
				StatusCodes.Status404NotFound => "Not Found",
				StatusCodes.Status403Forbidden => "Forbidden",
				_ => "Generic error"
			};

		private static string GetDefaultDetails(int statusCode) => statusCode switch
		{
			StatusCodes.Status400BadRequest => "Ignored or malformed request",
			StatusCodes.Status404NotFound => "Entity does not exists",
			StatusCodes.Status403Forbidden => "Operation is forbidden",
			_ => "Generic error"
		};
	}
}
