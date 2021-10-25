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
	public sealed class UnlockUserHandlerTest
	{
		private readonly CancellationToken _cancellationToken;

		public UnlockUserHandlerTest()
		{
			var tokenSource = new CancellationTokenSource();
			_cancellationToken = tokenSource.Token;
		}

		[Fact]
		public async Task Unlock_user_success()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Unlock_user_success));
			var fakeAdmin = FakeUserHelper.CreateAdmin();
			var fakeUser = FakeUserHelper.CreateUser(isLocked: true);

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new UnlockUserCommand(fakeUser.Id, fakeAdmin.Id);

			await TestAsync(contextHandler, command);

			var dbModel = await FakeUserHelper.GetEntityAsync(contextHandler, command.UserId, _cancellationToken);

			Assert.False(dbModel.IsLocked);
			Assert.NotNull(dbModel.UpdatedDate);
			Assert.Equal(dbModel.UpdatedBy, fakeAdmin.Id);
		}

		[Fact]
		public async Task Handle_throws_exception_when_no_rights()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_no_rights));
			var fakeAdmin = FakeUserHelper.CreateDefault();
			var fakeUser = FakeUserHelper.CreateUser(isLocked: true);

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new UnlockUserCommand(fakeUser.Id, fakeAdmin.Id);

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, command));
		}

		[Fact]
		public async Task Handle_throws_exception_when_admin_not_found()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_admin_not_found));
			var fakeAdmin = FakeUserHelper.CreateAdmin();
			var fakeUser = FakeUserHelper.CreateUser(isLocked: true);

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new UnlockUserCommand(fakeUser.Id, Guid.NewGuid());

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, command));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_not_found()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_not_found));
			var fakeAdmin = FakeUserHelper.CreateDefault();
			var fakeUser = FakeUserHelper.CreateUser(isLocked: true);

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new UnlockUserCommand(Guid.NewGuid(), fakeAdmin.Id);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, command));
		}

		[Fact]
		public async Task Handle_throws_exception_when_user_is_archived()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_user_is_archived));
			var fakeAdmin = FakeUserHelper.CreateDefault();
			var fakeUser = FakeUserHelper.CreateUser(isLocked: true, isArchived: true);

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeAdmin, _cancellationToken);
			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new UnlockUserCommand(fakeUser.Id, fakeAdmin.Id);

			await Assert.ThrowsAsync<GoNotFoundException>(async () => await TestAsync(contextHandler, command));
		}

		private Task TestAsync(
			ApplicationDbContextTest contextHandler,
			UnlockUserCommand command) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
			{
				var handler = new UnlockUserHandler(context);

				return handler.Handle(command, _cancellationToken);
			});
	}
}
