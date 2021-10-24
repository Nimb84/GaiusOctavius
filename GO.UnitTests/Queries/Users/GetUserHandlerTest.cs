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
		private const long FakeConnectionId = -1;
		private const ConnectionType FakeDefaultConnectionType = ConnectionType.Telegram;

		public GetUserHandlerTest()
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

			var query = new GetUserQuery
			{
				UserId = fakeUserId,
				CurrentUserId = fakeUserId,
				ConnectionType = ConnectionType.Unsupported
			};

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
			var fakeUserId = await FakeUserHelper.AddCorrectAsync(
				contextHandler,
				FakeConnectionId,
				FakeDefaultConnectionType,
				_cancellationToken);

			var query = new GetUserQuery
			{
				UserId = fakeUserId,
				CurrentUserId = fakeUserId,
				ConnectionType = ConnectionType.Telegram
			};

			var response = await TestAsync(contextHandler, query);

			var dbModel = await FakeUserHelper.GetEntityAsync(contextHandler, query.UserId, _cancellationToken);

			Assert.NotNull(response);
			Assert.Equal(response.Id, dbModel.Id);
			Assert.Equal(response.Id, query.UserId);
		}

		[Fact]
		public async Task Handle_throws_exception_when_non_existent_user()
		{
			var fakeUserId = Guid.NewGuid();

			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_non_existent_user));
			await FakeUserHelper.AddCorrectAsync(
				contextHandler,
				FakeConnectionId,
				FakeDefaultConnectionType,
				_cancellationToken);

			var query = new GetUserQuery
			{
				UserId = fakeUserId,
				CurrentUserId = fakeUserId,
				ConnectionType = ConnectionType.Unsupported
			};

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, query));
		}

		[Fact]
		public async Task Handle_throws_exception_when_not_found_by_connectionType()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_not_found_by_connectionType));
			var fakeUserId = await FakeUserHelper.AddCorrectAsync(
				contextHandler,
				FakeConnectionId,
				ConnectionType.Unsupported,
				_cancellationToken);

			var query = new GetUserQuery
			{
				UserId = fakeUserId,
				CurrentUserId = fakeUserId,
				ConnectionType = ConnectionType.Telegram
			};

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, query));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_is_locked()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_is_locked));
			var fakeUserId = await FakeUserHelper.AddLockedAsync(
				contextHandler,
				FakeConnectionId,
				FakeDefaultConnectionType,
				_cancellationToken);

			var query = new GetUserQuery
			{
				UserId = fakeUserId,
				CurrentUserId = fakeUserId
			};

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, query));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_is_archived()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_is_archived));
			var fakeUserId = await FakeUserHelper.AddArchivedAsync(
				contextHandler,
				FakeConnectionId,
				FakeDefaultConnectionType,
				_cancellationToken);

			var query = new GetUserQuery
			{
				UserId = fakeUserId,
				CurrentUserId = fakeUserId
			};

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, query));
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
