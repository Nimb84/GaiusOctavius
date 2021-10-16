using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Authentication;
using GO.API.Models;
using GO.Domain.Enums.Domain;
using GO.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;

namespace GO.API.Filters
{
	public sealed class ExceptionResponseFilter
		: IExceptionFilter
	{
		private readonly ILogger<ExceptionResponseFilter> _logger;

		public ExceptionResponseFilter(ILogger<ExceptionResponseFilter> logger)
		{
			_logger = logger;
		}

		public void OnException(ExceptionContext context)
		{
			var statusCode = context.Exception switch
			{
				GoException apiException => apiException.StatusCode,
				ArgumentNullException _ => StatusCodes.Status422UnprocessableEntity,
				ValidationException _ => StatusCodes.Status422UnprocessableEntity,
				AuthenticationException _ => StatusCodes.Status401Unauthorized,
				_ => StatusCodes.Status500InternalServerError
			};

			var title = context.Exception switch
			{
				GoException apiException => apiException.Title,
				ArgumentNullException _ => "Ignored or malformed argument",
				ValidationException _ => "Validation error",
				AuthenticationException _ => "Authentication issue",
				_ => "Internal error"
			};

			var message = context.Exception switch
			{
				GoException apiException => apiException.Details,
				ArgumentNullException _ => context.Exception.Message,
				ValidationException _ => context.Exception.Message,
				AuthenticationException _ => "Action is permitted because of authentication reasons",
				_ => "Internal error occurred. Please try a little bit later"
			};

			var details = context.Exception switch
			{
				GoException apiException => apiException.Errors.ToList(),
				//ValidationException validationException => validationException.Errors
				//	.Select(t => new ErrorDetails(
				//		!(t.CustomState is null) && Enum.IsDefined(typeof(ExceptionType), t.CustomState)
				//			? (ExceptionType)t.CustomState
				//			: ExceptionType.Validation,
				//		t.ErrorMessage
				//			?? (!(t.CustomState is null)
				//				? ((ExceptionType)t.CustomState).Humanize()
				//				: "Internal error occurred. Please try a little bit later")))
				//	.ToList(),
				_ => new List<ErrorDetails> { new(ExceptionType.Internal, context.Exception.Message) }
			};

			var problemDetails = new CustomProblemDetails
			{
				Title = title,
				Status = statusCode,
				Detail = message,
				Errors = details
			};

			switch (problemDetails.Status)
			{
				case >= StatusCodes.Status500InternalServerError:
					_logger.LogError(context.Exception, "Critical error handled");
					break;
				case >= StatusCodes.Status400BadRequest:
					_logger.LogError(context.Exception, "Request error handled");
					break;
			}

			var response = BuildResponse(problemDetails);

			context.HttpContext.Response.StatusCode = response.StatusCode ?? StatusCodes.Status500InternalServerError;
			context.Result = response;
			context.ExceptionHandled = true;
		}

		private static ObjectResult BuildResponse(ProblemDetails problem) =>
			new(problem)
			{
				StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError,
				ContentTypes = new MediaTypeCollection { "application/problem+json" }
			};
	}

}
