using System;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.Handlers.Management;
using GO.Commands.Management;
using GO.Domain.Enums.Users;
using GO.Domain.Exceptions;
using GO.UnitTests.Builders;
using Xunit;

namespace GO.UnitTests.Commands.Management
{
	public sealed class RegisterTelegramUserHandlerTest
	{
		private readonly CancellationToken _cancellationToken;
		private readonly long _connectionId = 654321;

		public RegisterTelegramUserHandlerTest()
		{
			var tokenSource = new CancellationTokenSource();
			_cancellationToken = tokenSource.Token;
		}

		[Fact]
		public async Task Register_telegram_user_success()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Register_telegram_user_success));

			var command = new RegisterTelegramUserCommand(
				Guid.NewGuid(),
				"FakeFirstName",
				"FakeLastName",
				"@FakeNickname",
				_connectionId);

			await TestAsync(contextHandler, command);

			var dbModel = await FakeUserHelper.GetEntityAsync(contextHandler, command.UserId, _cancellationToken);

			Assert.NotNull(dbModel);
			Assert.Equal(dbModel.FirstName, command.FirstName);
			Assert.Equal(dbModel.LastName, command.LastName);
			Assert.True(dbModel.Connections.TrueForAll(connection => connection.ConnectionId == command.TelegramId));
			Assert.True(dbModel.Connections.TrueForAll(connection => connection.NickName == command.NickName));
			Assert.Equal(dbModel.CreatedBy, command.UserId);
		}

		[Fact]
		public async Task Handle_throws_exception_when_collision_telegramId()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_collision_telegramId));

			var fakeUser = FakeUserHelper.CreateDefault(
				connectionType: ConnectionType.Telegram,
				connectionId: _connectionId);

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new RegisterTelegramUserCommand(
				Guid.NewGuid(),
				"FakeFirstName",
				"FakeLastName",
				"@FakeNickname",
				_connectionId);

			await Assert.ThrowsAsync<GoException>(async () => await TestAsync(contextHandler, command));
		}

		private Task TestAsync(
			ApplicationDbContextTest contextHandler,
			RegisterTelegramUserCommand command) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
			{
				var handler = new RegisterTelegramUserHandler(context);

				return handler.Handle(command, _cancellationToken);
			});
	}
}
