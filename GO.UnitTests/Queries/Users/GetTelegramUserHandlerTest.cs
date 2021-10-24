using System;
using System.Threading;
using System.Threading.Tasks;
using GO.Domain.Enums.Users;
using GO.Domain.Exceptions;
using GO.Queries.Handlers.Users;
using GO.Queries.ResponseModels.Users;
using GO.Queries.Users;
using GO.UnitTests.Builders;
using Xunit;

namespace GO.UnitTests.Queries.Users
{
	public sealed class GetTelegramUserHandlerTest
	{
		private readonly CancellationToken _cancellationToken;
		private const long FakeConnectionId = 123456;
		private const ConnectionType FakeDefaultConnectionType = ConnectionType.Telegram;

		public GetTelegramUserHandlerTest()
		{
			var tokenSource = new CancellationTokenSource();
			_cancellationToken = tokenSource.Token;
		}

		[Fact]
		public async Task Get_user_success()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Get_user_success));
			var fakeUserId = await FakeUserHelper.AddCorrectAsync(
				contextHandler,
				FakeConnectionId,
				FakeDefaultConnectionType,
				_cancellationToken);

			var query = new GetTelegramUserQuery(FakeConnectionId);

			var response = await TestAsync(contextHandler, query);

			Assert.NotNull(response);
			Assert.Equal(response.UserId, fakeUserId);
		}

		[Fact]
		public async Task Handle_throws_exception_when_non_existent_user()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Get_user_success));
			await FakeUserHelper.AddCorrectAsync(
				contextHandler,
				FakeConnectionId,
				FakeDefaultConnectionType,
				_cancellationToken);

			var query = new GetTelegramUserQuery(-1);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, query));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_is_locked()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_is_locked));
			await FakeUserHelper.AddLockedAsync(
				contextHandler,
				FakeConnectionId,
				FakeDefaultConnectionType,
				_cancellationToken);

			var query = new GetTelegramUserQuery(FakeConnectionId);

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, query));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_is_archived()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_is_archived));
			await FakeUserHelper.AddArchivedAsync(
				contextHandler,
				FakeConnectionId,
				FakeDefaultConnectionType,
				_cancellationToken);

			var query = new GetTelegramUserQuery(FakeConnectionId);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, query));
		}

		private Task<TelegramUserResponse> TestAsync(
			ApplicationDbContextTest contextHandler,
			GetTelegramUserQuery query) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
			{
				var handler = new GetTelegramUserHandler(context);

				return handler.Handle(query, _cancellationToken);
			});
	}
}
