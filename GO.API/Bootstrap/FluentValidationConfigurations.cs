using System.Collections.Generic;
using System.Reflection;
using FluentValidation;
using GO.API.Bootstrap.Pipelines;
using GO.Commands.Management;
using GO.Queries.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GO.API.Bootstrap
{
	public static class FluentValidationConfigurations
	{
		private static readonly List<Assembly> _validatorsAccemlyList = new()
		{
			typeof(GetUserQuery).Assembly,
			typeof(RegisterTelegramUserCommand).Assembly
		};

		public static IServiceCollection RegisterFluentValidation(this IServiceCollection services)
		{
			AssemblyScanner
				.FindValidatorsInAssemblies(_validatorsAccemlyList)
				.ForEach(assembly => services.AddScoped(assembly.InterfaceType, assembly.ValidatorType));

			return services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MediatRPipelineValidationBehavior<,>));
		}
	}
}
