using System;
using System.Collections.Generic;
using GO.API.Filters;
using GO.Domain.Enums.Domain;
using GO.Domain.Exceptions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GO.API.Bootstrap
{
	public static class ApiControllersExtensions
	{
		public static void RegisterControllers(this IServiceCollection services)
		{
			services.AddControllers(options => options.Filters
				.Add(typeof(ExceptionResponseFilter)))
				.ConfigureApiBehaviorOptions(options =>
				{
					options.InvalidModelStateResponseFactory = c =>
					{
						var errors = new List<ErrorDetails>();

						foreach (var (key, state) in c.ModelState)
						{
							foreach (var error in state.Errors)
							{
								try
								{
									errors.Add(
										Enum.TryParse<ExceptionType>(error.ErrorMessage, out var enumValue)
											? new ErrorDetails(enumValue, enumValue.Humanize(), key)
											: new ErrorDetails(ExceptionType.Validation, error.ErrorMessage, key));
								}
								catch
								{
									// ignore
								}
							}
						}

						return new BadRequestObjectResult(new
						{
							Errors = errors
						});
					};
				})
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
					options.SerializerSettings.Formatting = Formatting.Indented;
				});
		}
	}
}
