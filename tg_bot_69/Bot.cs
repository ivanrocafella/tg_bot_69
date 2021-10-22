using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace tg_bot_69
{
    class Bot
    {
        //bot_tg_69
        private readonly TelegramBotClient _bot;
        private string ChoiceBot;

        public Bot(string token)
        {
            _bot = new TelegramBotClient(token);
        }

        [Obsolete]
        public void StartBot()
        {
            _bot.OnMessage += OnMessageReceived;
            _bot.OnCallbackQuery += HandleCallbackQuery;
            _bot.StartReceiving();
            while (true)
            {
                Console.WriteLine("Bot is worked all right");
                Thread.Sleep(int.MaxValue);
            }
        }

        [Obsolete]
        private async void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            try
            {
                Message message = messageEventArgs.Message;
                Console.WriteLine(message.Text);
                string AnswerOfBot = null;
                if (message.Text == "/start")
                {
                    AnswerOfBot = "Вас приветствует игровой бот!\n" +
                        "Со мной вы можете сыграть в игру \"Камень-ножницы-бумага\"\n" +
                        "Правила игры следующие:\n" +
                        "Бумага побеждает камень. Камень побеждает ножницы. Ножницы побеждают бумагу";
                }
                else if (message.Text == "/help")
                {
                    AnswerOfBot = "Правила игры следующие:\n" +
                                 "Бумага побеждает камень («бумага обёртывает камень»).\n" +
                                 "Камень побеждает ножницы («камень затупляет или ломает ножницы»).\n" +
                                 "Ножницы побеждают бумагу («ножницы разрезают бумагу»).";
                }
                else if (message.Text == "/game")
                {
                    string[] RPS = { "Камень", "Ножницы", "Бумага" };
                    Random random = new Random();
                    ChoiceBot = RPS[random.Next(0, 3)];
                    var markup = new InlineKeyboardMarkup(new[]
                    {
                        new InlineKeyboardButton(){Text = "Камень", CallbackData = RockPaperScissors(ChoiceBot, "Камень")},
                        new InlineKeyboardButton(){Text = "Ножницы", CallbackData = RockPaperScissors(ChoiceBot, "Ножницы")},
                        new InlineKeyboardButton(){Text = "Бумага", CallbackData = RockPaperScissors(ChoiceBot, "Бумага")},
                    }); 
                    await _bot.SendTextMessageAsync(message.Chat.Id, "Выберите вариант", replyMarkup: markup);
                }
                await _bot.SendTextMessageAsync(message.Chat.Id, AnswerOfBot);
                


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static string RockPaperScissors(string first, string second) =>
            (first, second) switch
            {
                ("Камень", "Бумага") => "бумага обернула камень. Вы победили",
                ("Камень", "Ножницы") => "камень сломал ножницы. Бот победил",
                ("Бумага", "Камень") => "бумага обернула камень. Бот победил",
                ("Бумага", "Ножницы") => "ножницы разрезали бумагу. Вы победили",
                ("Ножницы", "Камень") => "ножницы сломаны от камня. Вы победили",
                ("Ножницы", "Бумага") => "ножницы разрезали бумагу. Бот победил",
                (_, _) => "Ничья",
            };

        [Obsolete]
        private async void HandleCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await _bot.SendTextMessageAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id,
                callbackQueryEventArgs.CallbackQuery.Data);
            await _bot.EditMessageReplyMarkupAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id,
                callbackQueryEventArgs.CallbackQuery.Message.MessageId, null);
        }




    }
}
