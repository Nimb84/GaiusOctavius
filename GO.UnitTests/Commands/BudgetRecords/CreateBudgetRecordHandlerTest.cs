using System;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.BudgetRecords;
using GO.Commands.Handlers.BudgetRecords;
using GO.UnitTests.Builders;

namespace GO.UnitTests.Commands.BudgetRecords
{
	public sealed class CreateBudgetRecordHandlerTest
	{
		private readonly CancellationToken _cancellationToken;

		public CreateBudgetRecordHandlerTest()
		{
			var tokenSource = new CancellationTokenSource();
			_cancellationToken = tokenSource.Token;
		}

		private async Task<Guid> AddFakeUserAsync(
			ApplicationDbContextTest contextHandler,
			bool isLocked = default,
			bool isArchived = default)
		{
			var fakeUser = FakeUserHelper.CreateDefault(
				isLocked: isLocked,
				isArchived: isArchived);

			await FakeUserHelper.AddToDbAsync(contextHandler, fakeUser, _cancellationToken);

			return fakeUser.Id;
		}

		private Task TestAsync(
			ApplicationDbContextTest contextHandler,
			CreateBudgetRecordCommand command) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
			{
				var handler = new CreateBudgetRecordHandler(context);

				return handler.Handle(command, _cancellationToken);
			});
	}
}
