using System;
using System.Linq;
using GO.Domain.Enums.Domain;
using GO.Domain.Exceptions;
using GO.Domain.Extensions;
using GO.Integrations.TelegramBot.Enums;
using GO.Integrations.TelegramBot.Models.Requests;
using Microsoft.AspNetCore.Http;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GO.Integrations.TelegramBot.Extensions
{
	internal static class UpdateExtensions
	{
		public static long GetTelegramId(this Update model) =>
			model.Type switch
			{
				UpdateType.Message => model.Message.From.Id,
				UpdateType.EditedMessage => model.EditedMessage.From.Id,
				UpdateType.CallbackQuery => model.CallbackQuery.From.Id,
				_ => throw new ArgumentOutOfRangeException(nameof(UpdateType), model.Type, null)
			};

		public static string GetText(this Update model) =>
			model.Type switch
			{
				UpdateType.Message => model.Message.Text,
				UpdateType.CallbackQuery => model.CallbackQuery.Message.Text,
				_ => throw new ArgumentOutOfRangeException(nameof(model.Type), model.Type, null)
			};

		public static string GetCommand(this Update model) =>
			model.Type switch
			{
				UpdateType.Message => model.Message.Text,
				UpdateType.CallbackQuery => model.CallbackQuery.Data,
				_ => throw new ArgumentOutOfRangeException(nameof(model.Type), model.Type, null)
			};

		public static bool IsCommand(this Update model) =>
			model.IsCommand(out _);

		public static bool IsCommand(this Update model, out CommandType type)
		{
			type = model.Type is UpdateType.Message or UpdateType.CallbackQuery
				? EnumExtensions.Parse<CommandType>(model.GetCommand().Split().First()[1..])
				: CommandType.None;

			return type != CommandType.None;
		}

		public static CommandRequest ToCommandRequest(this Update model) =>
			model.IsCommand()
				? new CommandRequest(model.GetCommand())
				: throw new GoException(StatusCodes.Status400BadRequest, ExceptionType.Unsupported);

		public static bool IsBot(this Update model) =>
			model.Type switch
			{
				UpdateType.Message => model.Message.From.IsBot,
				UpdateType.EditedMessage => model.EditedMessage.From.IsBot,
				UpdateType.CallbackQuery => model.CallbackQuery.From.IsBot,
				_ => throw new ArgumentOutOfRangeException(nameof(UpdateType), model.Type, null)
			};

		public static long GetChatId(this Update model) =>
			model.Type switch
			{
				UpdateType.Message => model.Message.Chat.Id,
				UpdateType.EditedMessage => model.EditedMessage.Chat.Id,
				UpdateType.CallbackQuery => model.CallbackQuery.Message.Chat.Id,
				_ => throw new ArgumentOutOfRangeException(nameof(UpdateType), model.Type, null)
			};

		public static int GetMessageId(this Update model) =>
			model.Type switch
			{
				UpdateType.Message => model.Message.MessageId,
				UpdateType.EditedMessage => model.EditedMessage.MessageId,
				UpdateType.CallbackQuery => model.CallbackQuery.Message.MessageId,
				_ => throw new ArgumentOutOfRangeException(nameof(UpdateType), model.Type, null)
			};
	}
}
