using System;
using System.Threading;
using System.Threading.Tasks;
using GO.Domain.Enums.Management;
using GO.Integrations.TelegramBot.Abstractions;
using GO.Integrations.TelegramBot.Abstractions.Behaviors;
using GO.Integrations.TelegramBot.Enums;
using GO.Integrations.TelegramBot.Extensions;
using GO.Queries.ResponseModels.Users;
using GO.Queries.Users;
using MediatR;
using Telegram.Bot.Types;

namespace GO.Integrations.TelegramBot.Services
{
	internal class BotBehaviorFactory
		: IBotBehaviorFactory
	{
		private readonly IMediator _mediator;
		private readonly IBudgetBotBehavior _budgetBotBehavior;
		private readonly IManagementBotBehavior _managementBotBehavior;

		public BotBehaviorFactory(
			IMediator mediator,
			IBudgetBotBehavior budgetBotBehavior,
			IManagementBotBehavior managementBotBehavior)
		{
			_mediator = mediator;
			_budgetBotBehavior = budgetBotBehavior;
			_managementBotBehavior = managementBotBehavior;
		}

		public Task HandleUpdateAsync(Update model, CancellationToken cancellationToken = default) =>
			model.IsCommand(out var command)
				? HandleAsCommandAsync(command, model, cancellationToken)
				: HandleAsMessageAsync(model, cancellationToken);

		private async Task HandleAsCommandAsync(
			CommandType command,
			Update model,
			CancellationToken cancellationToken)
		{
			var user = command != CommandType.Start
				? await GetUserAsync(model.GetTelegramId(), cancellationToken)
				: null;

			IBaseCommandChatBotBehavior behavior = command switch
			{
				CommandType.Start => _managementBotBehavior,
				CommandType.Information => _managementBotBehavior,
				CommandType.Management => _managementBotBehavior,
				CommandType.Budget => _budgetBotBehavior,
				_ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
			};

			await behavior.HandleCommandAsync(user, model, cancellationToken);
		}

		private async Task HandleAsMessageAsync(
			Update model,
			CancellationToken cancellationToken)
		{
			var user = await GetUserAsync(model.GetTelegramId(), cancellationToken);

			IBaseMessageChatBotBehavior behavior = user.CurrentScope switch
			{
				Scopes.Budget => _budgetBotBehavior,
				_ => throw new ArgumentOutOfRangeException(nameof(user.CurrentScope), user.CurrentScope, null)
			};

			await behavior.HandleMessageAsync(user, model, cancellationToken);
		}

		private Task<TelegramUserResponse> GetUserAsync(
			long telegramId,
			CancellationToken cancellationToken) =>
			_mediator.Send(new GetTelegramUserQuery(telegramId), cancellationToken);
	}
}
