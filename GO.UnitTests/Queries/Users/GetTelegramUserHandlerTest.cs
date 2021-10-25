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
		private readonly long _connectionId = 123456;

		public GetTelegramUserHandlerTest()
		{
			var tokenSource = new CancellationTokenSource();
			_cancellationToken = tokenSource.Token;
		}

		[Fact]
		public async Task Get_user_success()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Get_user_success));
			var fakeUserId = await AddFakeUserAsync(contextHandler);

			var query = new GetTelegramUserQuery(_connectionId);

			var response = await TestAsync(contextHandler, query);

			Assert.NotNull(response);
			Assert.Equal(response.UserId, fakeUserId);
		}

		[Fact]
		public async Task Handle_throws_exception_when_non_existent_user()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Get_user_success));
			await AddFakeUserAsync(contextHandler);

			var query = new GetTelegramUserQuery(-1);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, query));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_is_locked()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_is_locked));
			await AddFakeUserAsync(contextHandler, isLocked: true);

			var query = new GetTelegramUserQuery(_connectionId);

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, query));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_is_archived()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_is_archived));
			await AddFakeUserAsync(contextHandler, isArchived: true);

			var query = new GetTelegramUserQuery(_connectionId);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, query));
		}

		private async Task<Guid> AddFakeUserAsync(
			ApplicationDbContextTest contextHandler,
			bool isLocked = default,
			bool isArchived = default)
		{
			var fakeUser = FakeUserHelper.CreateDefault(
				isLocked: isLocked,
				isArchived: isArchived,
				connectionType: ConnectionType.Telegram,
				connectionId: _connectionId);

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			return fakeUser.Id;
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
