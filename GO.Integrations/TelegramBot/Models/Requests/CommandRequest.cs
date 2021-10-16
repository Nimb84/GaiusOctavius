using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GO.Domain.Enums.Domain;
using GO.Domain.Exceptions;
using GO.Domain.Extensions;
using GO.Integrations.TelegramBot.Enums;
using Humanizer;
using Microsoft.AspNetCore.Http;

namespace GO.Integrations.TelegramBot.Models.Requests
{
	internal sealed class CommandRequest
	{
		private const string ParsePattern = @"^\/(\w+|\?)\s*(\w*)\s*(.*?)$";

		public CommandType Command { get; }

		public string Action { get; }

		public List<string> Arguments { get; } = new();

		public CommandRequest(string message)
		{
			var matches = Regex.Matches(message, ParsePattern);

			if (!matches.Any())
				throw new GoException(
					StatusCodes.Status400BadRequest,
					ExceptionType.Cast);

			Command = EnumExtensions.Parse<CommandType>(matches[0].Groups[1].Value.Trim().ToLower());
			Action = matches[0].Groups[2].Value.Trim().ToLower();

			var argsString = matches[0].Groups[3].Value.Trim().ToLower();
			if (!string.IsNullOrWhiteSpace(argsString))
			{
				Arguments = argsString.Split()
					.Where(arg => !string.IsNullOrWhiteSpace(arg))
					.ToList();
			}
		}
	}
}
