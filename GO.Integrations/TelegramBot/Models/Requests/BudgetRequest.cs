using System.Linq;
using System.Text.RegularExpressions;
using GO.Domain.Enums.Domain;
using GO.Domain.Exceptions;
using Humanizer;
using Microsoft.AspNetCore.Http;

namespace GO.Integrations.TelegramBot.Models.Requests
{
	internal sealed class BudgetRequest
	{
		private const string ParsePattern = @"^[+]?(\d+([,.]?\d{0,2}))\s*(.*?)?$";

		public uint Amount { get; }
		public string Description { get; }

		public BudgetRequest(string message)
		{
			var matches = Regex.Matches(message, ParsePattern);

			if (!matches.Any() || !uint.TryParse(matches[0].Groups[1].Value, out var price))
				throw new GoException(
					StatusCodes.Status400BadRequest,
					ExceptionType.Cast);

			Amount = price;
			Description = matches[0].Groups[3].Value;
		}
	}
}
