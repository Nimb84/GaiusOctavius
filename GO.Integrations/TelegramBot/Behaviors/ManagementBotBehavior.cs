using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.Management;
using GO.Domain.Exceptions;
using GO.Domain.Extensions;
using GO.Domain.Options;
using GO.Integrations.TelegramBot.Abstractions;
using GO.Integrations.TelegramBot.Abstractions.Behaviors;
using GO.Integrations.TelegramBot.Enums;
using GO.Integrations.TelegramBot.Extensions;
using GO.Integrations.TelegramBot.Helpers;
using GO.Integrations.TelegramBot.Resources;
using GO.Queries.ResponseModels.Users;
using GO.Queries.Users;
using Humanizer;
using MediatR;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GO.Integrations.TelegramBot.Behaviors
{
	internal sealed class ManagementBotBehavior
		: IManagementBotBehavior
	{
		private readonly IMediator _mediator;
		private readonly TelegramBotSettings _telegramBotSettings;
		private readonly ITelegramBotClientService _telegramBotClientService;

		public ManagementBotBehavior(
			IMediator mediator,
			IOptions<TelegramBotSettings> telegramBotSettings,
			ITelegramBotClientService telegramBotClientService)
		{
			_mediator = mediator;
			_telegramBotSettings = telegramBotSettings.Value;
			_telegramBotClientService = telegramBotClientService;
		}

		public Task HandleCommandAsync(
			TelegramUserResponse currentUser,
			Update model,
			CancellationToken cancellationToken = default)
		{
			var command = model.ToCommandRequest().Type;

			return command switch
			{
				CommandType.Start => RegisterUser(model, cancellationToken),
				CommandType.Management => ManagementUser(currentUser, model, cancellationToken),
				CommandType.Information => SendUserInfo(currentUser, model, cancellationToken),
				_ => throw new ArgumentOutOfRangeException(nameof(CommandType), command, null)
			};
		}

		private async Task RegisterUser(Update model, CancellationToken cancellationToken)
		{
			if (model.IsBot() || model.GetChatId() != model.GetTelegramId())
				throw new GoForbiddenException();

			var command = new RegisterTelegramUserCommand
			{
				CurrentUserId = Guid.NewGuid(),
				FirstName = model.Message.From.FirstName,
				LastName = model.Message.From.LastName,
				NickName = model.Message.From.Username,
				TelegramId = model.Message.From.Id
			};

			await _mediator.Send(command, cancellationToken);

			await _telegramBotClientService.SendTextMessageAsync(
				_telegramBotSettings.AdminChatId,
				CommandResources.NewUser_Format
					.FormatWith($"{command.FirstName} {command.LastName}", command.NickName),
				InlineKeyboardHelper.GetLockUserKeyboard(command.CurrentUserId, ActionType.Decline),
				cancellationToken);
		}

		private Task ManagementUser(
			TelegramUserResponse currentUser,
			Update model,
			CancellationToken cancellationToken)
		{
			if (model.IsBot() || model.GetChatId() != _telegramBotSettings.AdminChatId)
				throw new GoForbiddenException();

			var command = model.ToCommandRequest();
			var action = EnumExtensions.Parse<ActionType>(command.Action);

			return action switch
			{
				ActionType.Decline => LockUser(
					Guid.Parse(command.Arguments.First()),
					currentUser.UserId,
					model,
					cancellationToken),
				ActionType.Approve => UnlockUser(
					Guid.Parse(command.Arguments.First()),
					currentUser.UserId,
					model,
					cancellationToken),
				_ => throw new ArgumentOutOfRangeException(nameof(ActionType), action, null)
			};
		}

		private async Task LockUser(
			Guid userId,
			Guid currentUserId,
			Update model,
			CancellationToken cancellationToken)
		{
			var command = new LockUserCommand(userId, currentUserId);

			await _mediator.Send(command, cancellationToken);

			if (model.Type == UpdateType.CallbackQuery)
			{
				await _telegramBotClientService.UpdateTextMessageAsync(
					model.GetChatId(),
					model.GetMessageId(),
					model.GetText(),
					InlineKeyboardHelper.GetLockUserKeyboard(userId, ActionType.Approve),
					cancellationToken);
			}
		}

		private async Task UnlockUser(
			Guid userId,
			Guid currentUserId,
			Update model,
			CancellationToken cancellationToken)
		{
			var command = new UnlockUserCommand(userId, currentUserId);

			await _mediator.Send(command, cancellationToken);

			if (model.Type == UpdateType.CallbackQuery)
			{
				await _telegramBotClientService.UpdateTextMessageAsync(
					model.GetChatId(),
					model.GetMessageId(),
					model.GetText(),
					InlineKeyboardHelper.GetLockUserKeyboard(userId, ActionType.Decline),
					cancellationToken);
			}
		}

		private async Task SendUserInfo(
			TelegramUserResponse currentUser,
			Update model,
			CancellationToken cancellationToken)
		{
			if (model.IsBot() || model.Message.Chat.Id != model.Message.From.Id)
				throw new GoForbiddenException();

			var command = new GetUserQuery
			{
				UserId = currentUser.UserId,
				CurrentUserId = currentUser.UserId
			};

			var user = await _mediator.Send(command, cancellationToken);

			await _telegramBotClientService.SendTextMessageAsync(
				model.Message.Chat.Id,
				GetUserInfo(user),
				cancellationToken);
		}

		private string GetUserInfo(UserResponse user)
		{
			var builder = new StringBuilder();

			builder.AppendLine($"Hi {user.FirstName} {user.LastName}");
			builder.AppendLine($"Your scopes: {user.Scopes}");

			if (user.CurrentScope.HasValue)
				builder.AppendLine($"{Environment.NewLine}Current service {user.CurrentScope}");

			return builder.ToString();
		}
	}
}
