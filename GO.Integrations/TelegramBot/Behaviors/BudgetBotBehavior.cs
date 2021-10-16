using System;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.BudgetRecords;
using GO.Domain.Enums.Budgets;
using GO.Integrations.TelegramBot.Abstractions;
using GO.Integrations.TelegramBot.Abstractions.Behaviors;
using GO.Integrations.TelegramBot.Extensions;
using GO.Integrations.TelegramBot.Helpers;
using GO.Integrations.TelegramBot.Models.Requests;
using GO.Queries.ResponseModels.Users;
using MediatR;
using Telegram.Bot.Types;

namespace GO.Integrations.TelegramBot.Behaviors
{
	internal sealed class BudgetBotBehavior
		: IBudgetBotBehavior
	{
		private readonly IMediator _mediator;
		private readonly ITelegramBotClientService _telegramBotClientService;

		public BudgetBotBehavior(
			IMediator mediator,
			ITelegramBotClientService telegramBotClientService)
		{
			_mediator = mediator;
			_telegramBotClientService = telegramBotClientService;
		}

		public async Task HandleMessageAsync(
			TelegramUserResponse currentUser,
			Update model,
			CancellationToken cancellationToken = default)
		{
			var request = new BudgetRequest(model.Message.Text);
			var command = new CreateBudgetRecordCommand
			{
				RecordId = Guid.NewGuid(),
				BudgetId = currentUser.ServiceId.GetValueOrDefault(),
				CurrentUserId = currentUser.UserId,
				CategoryType = CategoryType.Other,
				Amount = request.Amount,
				Description = request.Description
			};

			await _mediator.Send(command, cancellationToken);

			await _telegramBotClientService.SendTextMessageAsync(
				model.GetChatId(),
				$"-{request.Amount} {InlineKeyboardHelper.GetBudgetCategoryIcon(CategoryType.Other)} ({CategoryType.Other})",
				InlineKeyboardHelper.GetBudgetRecordKeyboard(command.RecordId),
				cancellationToken);

			await _telegramBotClientService.DeleteMessageAsync(
				model.GetChatId(),
				model.GetMessageId(),
				cancellationToken);
		}

		public Task HandleCommandAsync(
			TelegramUserResponse currentUser,
			Update model,
			CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}
