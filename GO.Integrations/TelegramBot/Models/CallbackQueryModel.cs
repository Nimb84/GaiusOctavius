namespace GO.Integrations.TelegramBot.Models
{
	internal sealed class CallbackQueryModel
	{
		public string Text { get; }

		public string Command { get; }

		public CallbackQueryModel(string text, string command)
		{
			Text = text;
			Command = command;
		}
	}
}
