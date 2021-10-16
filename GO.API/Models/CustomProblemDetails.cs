using System.Collections.Generic;
using GO.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GO.API.Models
{
	public sealed class CustomProblemDetails
		: ProblemDetails
	{
		public List<ErrorDetails> Errors { get; set; } = new();
	}
}
