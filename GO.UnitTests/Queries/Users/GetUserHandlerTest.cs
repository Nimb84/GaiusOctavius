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
	public sealed class GetUserHandlerTest
	{
		private readonly CancellationToken _cancellationToken;
		private readonly long _connectionId = 11223344;

		public GetUserHandlerTest()
		{
			var tokenSource = new CancellationTokenSource();
			_cancellationToken = tokenSource.Token;
		}

		[Fact]
		public async Task Get_user_success()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Get_user_success));
			var fakeUserId = await AddFakeUserAsync(contextHandler);

			var query = new GetUserQuery(fakeUserId, fakeUserId);

			var response = await TestAsync(contextHandler, query);

			var dbModel = await FakeUserHelper.GetEntityAsync(contextHandler, query.UserId, _cancellationToken);

			Assert.NotNull(response);
			Assert.Equal(response.Id, dbModel.Id);
			Assert.Equal(response.Id, query.UserId);
		}

		[Fact]
		public async Task Get_user_success_by_connectionType()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Get_user_success_by_connectionType));
			var fakeUserId = await AddFakeUserAsync(contextHandler);

			var query = new GetUserQuery(fakeUserId, fakeUserId, ConnectionType.Telegram);

			var response = await TestAsync(contextHandler, query);

			var dbModel = await FakeUserHelper.GetEntityAsync(contextHandler, query.UserId, _cancellationToken);

			Assert.NotNull(response);
			Assert.Equal(response.Id, dbModel.Id);
			Assert.Equal(response.Id, query.UserId);
			Assert.Equal(response.ConnectionId, _connectionId);
		}

		[Fact]
		public async Task Handle_throws_exception_when_non_existent_user()
		{
			var fakeWrongUserId = Guid.NewGuid();

			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_non_existent_user));
			await AddFakeUserAsync(contextHandler);
			
			var query = new GetUserQuery(fakeWrongUserId, fakeWrongUserId);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, query));
		}

		[Fact]
		public async Task Handle_throws_exception_when_not_found_by_connectionType()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_not_found_by_connectionType));
			var fakeUserId = await AddFakeUserAsync(contextHandler, connectionType: default);

			var query = new GetUserQuery(fakeUserId, fakeUserId, ConnectionType.Telegram);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, query));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_is_locked()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_is_locked));
			var fakeUserId = await AddFakeUserAsync(contextHandler, isLocked: true);

			var query = new GetUserQuery(fakeUserId, fakeUserId);

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, query));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_is_archived()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_is_archived));
			var fakeUserId = await AddFakeUserAsync(contextHandler, isArchived: true);

			var query = new GetUserQuery(fakeUserId, fakeUserId);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, query));
		}

		private async Task<Guid> AddFakeUserAsync(
			ApplicationDbContextTest contextHandler,
			bool isLocked = default,
			bool isArchived = default,
			ConnectionType connectionType = ConnectionType.Telegram)
		{
			var fakeUser = FakeUserHelper.CreateDefault(
				isLocked: isLocked,
				isArchived: isArchived,
				connectionType: connectionType,
				connectionId: _connectionId);

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			return fakeUser.Id;
		}

		private Task<UserResponse> TestAsync(
			ApplicationDbContextTest contextHandler,
			GetUserQuery query) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
			{
				var handler = new GetUserHandler(context);

				return handler.Handle(query, _cancellationToken);
			});
	}
}
