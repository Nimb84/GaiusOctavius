using MediatR;
using System;

namespace GO.Commands.Budgets
{
	public sealed record GenerateBudgetsStatisticsCommand(DateTimeOffset DateFrom, DateTimeOffset DateTo)
		: IRequest<Unit>;
}
