using System;
using System.Collections.Generic;
using System.Linq;
using GO.Domain.Enums.Budgets;
using GO.Integrations.TelegramBot.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GO.Integrations.TelegramBot.Helpers
{
	internal static class InlineKeyboardHelper
	{
		public static IReplyMarkup GetLockUserKeyboard(Guid userId, ActionType type)
		{
			var icon = type switch
			{
				ActionType.Decline => "❌",
				ActionType.Approve => "✔",
				_ => throw new ArgumentOutOfRangeException(nameof(ActionType), type, null)
			};

			return GetKeyboard(new List<Dictionary<string, string>>
			{
				new ()
				{
					{icon, $"/{CommandType.Management} {type} {userId}"},
				}
			});
		}

		public static IReplyMarkup GetBudgetRecordKeyboard(Guid recordId)
		{
			return GetKeyboard(new List<Dictionary<string, string>>
			{
				new ()
				{
					{GetBudgetCategoryIcon(CategoryType.Bill), GetBudgetCategory(recordId, CategoryType.Bill)},
					{GetBudgetCategoryIcon(CategoryType.Household), GetBudgetCategory(recordId, CategoryType.Household)},
					{GetBudgetCategoryIcon(CategoryType.Medicine), GetBudgetCategory(recordId, CategoryType.Medicine)},
					{GetBudgetCategoryIcon(CategoryType.Pet), GetBudgetCategory(recordId, CategoryType.Pet)},
					{GetBudgetCategoryIcon(CategoryType.Travel), GetBudgetCategory(recordId, CategoryType.Travel)}
				},
				new ()
				{
					{GetBudgetCategoryIcon(CategoryType.Beauty), GetBudgetCategory(recordId, CategoryType.Beauty)},
					{GetBudgetCategoryIcon(CategoryType.Clothes), GetBudgetCategory(recordId, CategoryType.Clothes)},
					{GetBudgetCategoryIcon(CategoryType.Emergency), GetBudgetCategory(recordId, CategoryType.Emergency)},
					{GetBudgetCategoryIcon(CategoryType.Entertainment), GetBudgetCategory(recordId, CategoryType.Entertainment)},
					{GetBudgetCategoryIcon(CategoryType.Education), GetBudgetCategory(recordId, CategoryType.Education)}
				},
				new ()
				{
					{GetBudgetCategoryIcon(CategoryType.Grocery), GetBudgetCategory(recordId, CategoryType.Grocery)},
					{GetBudgetCategoryIcon(CategoryType.Restaurant), GetBudgetCategory(recordId, CategoryType.Restaurant)},
					{GetBudgetCategoryIcon(CategoryType.Personal), GetBudgetCategory(recordId, CategoryType.Personal)},
				},
				new ()
				{
					{"✔", $"/{CommandType.Budget} {ActionType.Approve} {recordId}"},
					{"❌", $"/{CommandType.Budget} {ActionType.Decline} {recordId}"},
				}
			});
		}

		public static string GetBudgetCategoryIcon(CategoryType category) =>
			category switch
			{
				CategoryType.Other => "💩",
				CategoryType.Bill => "🧾",
				CategoryType.Grocery => "🥩",
				CategoryType.Household => "🏠",
				CategoryType.Medicine => "💊",
				CategoryType.Pet => "🐈‍⬛️",
				CategoryType.Beauty => "😇",
				CategoryType.Clothes => "👕",
				CategoryType.Restaurant => "🍔",
				CategoryType.Entertainment => "💃",
				CategoryType.Education => "📖",
				CategoryType.Travel => "🏖",
				CategoryType.Emergency => "⚠️",
				CategoryType.Personal => "🚩",
				_ => throw new ArgumentOutOfRangeException(nameof(CategoryType), category, null)
			};

		private static string GetBudgetCategory(Guid recordId, CategoryType category) =>
			$"/{CommandType.Budget} {ActionType.ChangeCategory} {recordId} {category}";

		// TODO: Fix mapping with new InlineKeyboardButton[][].
		private static InlineKeyboardMarkup GetKeyboard(IEnumerable<IDictionary<string, string>> buttons) =>
			new(buttons
				.Select(row => row
					.Select(cell => new InlineKeyboardButton
					{
						Text = cell.Key,
						CallbackData = cell.Value.ToLower()
					})
					.ToList())
				.ToList());
	}
}
