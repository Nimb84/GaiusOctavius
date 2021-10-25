using System;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.Budgets;
using GO.Commands.Handlers.Budgets;
using GO.Domain.Entities.Budgets;
using GO.Domain.Enums.Users;
using GO.Domain.Exceptions;
using GO.UnitTests.Builders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GO.UnitTests.Commands.Budgets
{
	public sealed class CreateBudgetHandlerTest
	{
		private readonly CancellationToken _cancellationToken;

		public CreateBudgetHandlerTest()
		{
			var tokenSource = new CancellationTokenSource();
			_cancellationToken = tokenSource.Token;
		}

		[Fact]
		public async Task Create_budget_with_user_success()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Create_budget_with_user_success));
			var fakeUser = FakeUserHelper.CreateUser();

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new CreateBudgetCommand(Guid.NewGuid(), fakeUser.Id);

			await TestAsync(contextHandler, command);

			var dbModel = await GetEntityAsync(contextHandler, command.BudgetId, _cancellationToken);

			Assert.Equal(command.BudgetId, dbModel.Id);
			Assert.NotNull(dbModel);
		}

		[Fact]
		public async Task Handle_throws_exception_when_non_existent_user()
		{
			var budgetId = Guid.NewGuid();

			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_non_existent_user));

			await FakeUserHelper.AddToDbAsync(contextHandler, FakeUserHelper.CreateUser(), _cancellationToken);

			var command = new CreateBudgetCommand(budgetId, Guid.NewGuid());

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, command));
		}

		[Fact]
		public async Task Handle_throws_exception_when_forbidden_wrong_userId()
		{
			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_forbidden_wrong_userId));
			
			await FakeUserHelper.AddToDbAsync(contextHandler, FakeUserHelper.CreateUser(), _cancellationToken);

			var command = new CreateBudgetCommand(Guid.NewGuid(), Guid.NewGuid());

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, command));
		}

		[Fact]
		public async Task Handle_throws_exception_when_forbidden_no_access()
		{
			var budgetId = Guid.NewGuid();

			var contextHandler = new ApplicationDbContextTest(nameof(Handle_throws_exception_when_forbidden_no_access));
			var fakeUser = FakeUserHelper.CreateDefault();

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			var command = new CreateBudgetCommand(budgetId, fakeUser.Id);

			await Assert.ThrowsAsync<GoForbiddenException>(async () => await TestAsync(contextHandler, command));
		}

		private Task<Unit> TestAsync(ApplicationDbContextTest contextHandler, CreateBudgetCommand command) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
			{
				var handler = new CreateBudgetHandler(context);

				return handler.Handle(command, _cancellationToken);
			});

		private static Task<Budget> GetEntityAsync(
			ApplicationDbContextTest contextHandler,
			Guid budgetId,
			CancellationToken cancellationToken) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
				context.Budgets.FirstOrDefaultAsync(budget => budget.Id == budgetId, cancellationToken));
	}
}
