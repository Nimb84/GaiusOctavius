namespace GO.Domain.Options
{
	public sealed class TelegramBotSettings
	{
		public string Token { get; set; }

		public long LogChatId { get; set; }

		public long AdminChatId { get; set; }

		public string WebhookUrl { get; set; }
	}
}
