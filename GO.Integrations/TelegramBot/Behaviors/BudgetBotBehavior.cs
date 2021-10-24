using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.BudgetRecords;
using GO.Domain.Enums.Budgets;
using GO.Domain.Extensions;
using GO.Integrations.TelegramBot.Abstractions;
using GO.Integrations.TelegramBot.Abstractions.Behaviors;
using GO.Integrations.TelegramBot.Enums;
using GO.Integrations.TelegramBot.Extensions;
using GO.Integrations.TelegramBot.Helpers;
using GO.Integrations.TelegramBot.Models.Requests;
using GO.Integrations.TelegramBot.Resources;
using GO.Queries.BudgetRecords;
using GO.Queries.ResponseModels.Users;
using Humanizer;
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
				BudgetId = currentUser.BudgetId.GetValueOrDefault(),
				CurrentUserId = currentUser.UserId,
				CategoryType = request.Category,
				Amount = request.Amount,
			};

			await _mediator.Send(command, cancellationToken);

			await _telegramBotClientService.SendTextMessageAsync(
				model.GetChatId(),
				GetMessage(command.Amount, command.CategoryType),
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
			var command = model.ToCommandRequest();
			var action = EnumExtensions.Parse<ActionType>(command.Action);

			return action switch
			{
				ActionType.Change => ChangeBudgetRecordCategoryAsync(
					Guid.Parse(command.Arguments.First()),
					currentUser.BudgetId.GetValueOrDefault(),
					currentUser.UserId,
					EnumExtensions.Parse<CategoryType>(command.Arguments[1]),
					model,
					cancellationToken),
				ActionType.Approve => RemoveKeyboardAsync(model, cancellationToken),
				ActionType.Decline => DeleteBudgetRecordAsync(
					Guid.Parse(command.Arguments.First()),
					currentUser.BudgetId.GetValueOrDefault(),
					currentUser.UserId,
					model,
					cancellationToken),
				_ => throw new ArgumentOutOfRangeException(nameof(ActionType), action, null)
			};
		}

		private async Task ChangeBudgetRecordCategoryAsync(
			Guid recordId,
			Guid budgetId,
			Guid userId,
			CategoryType type,
			Update model,
			CancellationToken cancellationToken)
		{
			await _mediator.Send(new ChangeBudgetRecordCategoryCommand
			{
				RecordId = recordId,
				BudgetId = budgetId,
				CurrentUserId = userId,
				CategoryType = type
			}, cancellationToken);

			var record = await _mediator.Send(new GetBudgetRecordQuery
			{
				BudgetRecordId = recordId,
				UserId = userId
			}, cancellationToken);

			await _telegramBotClientService.UpdateTextMessageAsync(
				model.GetChatId(),
				model.GetMessageId(),
				GetMessage(record.Amount, record.CategoryType),
				cancellationToken);
		}

		private Task RemoveKeyboardAsync(
			Update model,
			CancellationToken cancellationToken) =>
			_telegramBotClientService.UpdateTextMessageAsync(
				model.GetChatId(),
				model.GetMessageId(),
				model.GetText(),
				cancellationToken);

		private async Task DeleteBudgetRecordAsync(
			Guid recordId,
			Guid budgetId,
			Guid userId,
			Update model,
			CancellationToken cancellationToken)
		{
			await _mediator.Send(new DeleteBudgetRecordCommand
			{
				RecordId = recordId,
				BudgetId = budgetId,
				CurrentUserId = userId
			}, cancellationToken);

			await _telegramBotClientService.DeleteMessageAsync(
				model.GetChatId(),
				model.GetMessageId(),
				cancellationToken);
		}

		private static string GetMessage(uint amount, CategoryType category) =>
			MessageResources.BudgetRecord.FormatWith(
				$"{(category is CategoryType.Income ? "+" : "-")}{amount}",
				MessageResources.BudgetCategory.FormatWith(
					InlineKeyboardHelper.GetBudgetCategoryIcon(category),
					category));
	}
}
