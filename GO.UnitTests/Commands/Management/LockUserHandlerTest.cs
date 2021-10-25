using System;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.Handlers.Management;
using GO.Commands.Management;
using GO.Domain.Exceptions;
using GO.UnitTests.Builders;
using Xunit;

namespace GO.UnitTests.Commands.Management
{
	public sealed class LockUserHandlerTest
	{
		private readonly CancellationToken _cancellationToken;

		public LockUserHandlerTest()
		{
			var tokenSource = new CancellationTokenSource();
			_cancellationToken = tokenSource.Token;
		}

		[Fact]
		public async Task Lock_user_success()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Lock_user_success));
			var fakeAdmin = FakeUserHelper.CreateAdmin();
			var fakeUser = FakeUserHelper.CreateUser();

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new LockUserCommand(fakeUser.Id, fakeAdmin.Id);

			await TestAsync(contextHandler, command);

			var dbModel = await FakeUserHelper.GetEntityAsync(contextHandler, command.UserId, _cancellationToken);

			Assert.True(dbModel.IsLocked);
			Assert.NotNull(dbModel.UpdatedDate);
			Assert.Equal(dbModel.UpdatedBy, fakeAdmin.Id);
		}

		[Fact]
		public async Task Handle_throws_exception_when_no_rights()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_no_rights));
			var fakeAdmin = FakeUserHelper.CreateUser();
			var fakeUser = FakeUserHelper.CreateUser();

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new LockUserCommand(fakeUser.Id, fakeAdmin.Id);

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, command));
		}

		[Fact]
		public async Task Handle_throws_exception_when_admin_not_found()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_admin_not_found));
			var fakeAdmin = FakeUserHelper.CreateAdmin();
			var fakeUser = FakeUserHelper.CreateUser();

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new LockUserCommand(fakeUser.Id, Guid.NewGuid());

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, command));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_not_found()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_not_found));
			var fakeAdmin = FakeUserHelper.CreateAdmin();
			var fakeUser = FakeUserHelper.CreateUser();

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new LockUserCommand(Guid.NewGuid(), fakeAdmin.Id);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, command));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_is_archived()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_is_archived));
			var fakeAdmin = FakeUserHelper.CreateAdmin();
			var fakeUser = FakeUserHelper.CreateUser(isArchived: true);

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new LockUserCommand(fakeUser.Id, fakeAdmin.Id);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, command));
		}

		[Fact]
		public async Task Handle_throws_exception_when_admin_is_archived()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_admin_is_archived));
			var fakeAdmin = FakeUserHelper.CreateAdmin(isArchived: true);
			var fakeUser = FakeUserHelper.CreateUser();

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new LockUserCommand(fakeUser.Id, fakeAdmin.Id);

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, command));
		}

		private Task TestAsync(
			ApplicationDbContextTest contextHandler,
			LockUserCommand command) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
			{
				var handler = new LockUserHandler(context);

				return handler.Handle(command, _cancellationToken);
			});
	}
}
