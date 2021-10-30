using GO.API.Bootstrap;
using GO.BackgroundJobs.Bootstrap;
using GO.Integrations.TelegramBot.Bootstrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GO.API
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.RegisterOptions(Configuration);
			services.RegisterControllers();
			services.RegisterSwagger();
			services.RegisterMediatR();
			services.RegisterFluentValidation();
			services.RegisterSqlDatabase(Configuration);

			services.RegisterTelegramBot(Configuration);
			services.RegisterHangfire(Configuration);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwaggerConfiguration();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();

			app.UseHangfire();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
