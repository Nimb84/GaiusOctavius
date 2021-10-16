using System.Threading;
using System.Threading.Tasks;
using GO.Integrations.TelegramBot.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace GO.API.Controllers
{
	[Route("api/webhooks")]
	[ApiController]
	public sealed class WebhooksController : ControllerBase
	{
		private readonly ITelegramBotListenerService _telegramBotListenerService;

		public WebhooksController(ITelegramBotListenerService telegramBotListenerService)
		{
			_telegramBotListenerService = telegramBotListenerService;
		}

		[HttpPost("telegram")]
		public async Task<IActionResult> TelegramHandler(
			[FromBody] Update webhookModel,
			CancellationToken cancellationToken)
		{
			await _telegramBotListenerService.EchoUpdateAsync(webhookModel, cancellationToken);

			return Ok();
		}
	}
}
