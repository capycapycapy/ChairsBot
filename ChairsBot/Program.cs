using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7784803775:AAFDOQT1kZLNXWA-ynN52yUl_khWCp7jCgo");
bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);

Console.WriteLine($"Bot is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)//ловим сообщения в консоль
{
    if (update.Type == UpdateType.Message && update.Message?.Text != null)
    {
        // Отображаем сообщение пользователя в консоли
        Console.WriteLine($"Received message from {update.Message.From.Username}: {update.Message.Text}");

        await Batons(update.Message);
    }
    else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
    {
        await HandleCallbackQuery(update.CallbackQuery);
    }
}

async Task Batons(Message msg)//команды 
{
    if (msg.Text == "/start")
    {
        await bot.SendTextMessageAsync(msg.Chat, "Привет, выбери кнопочку.",//батоны(кнопки)
            replyMarkup: new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("Проверка", "check"),
                InlineKeyboardButton.WithCallbackData("Стул 1", "chair1"),
                InlineKeyboardButton.WithCallbackData("Стул 2", "chair2"),
                InlineKeyboardButton.WithCallbackData("Стул 3", "chair3"),
                InlineKeyboardButton.WithCallbackData("Очистка", "clear_chat")
            }));
    }
    else if (msg.Text == "/Check" || msg.Text == "/check")
    {
        await bot.SendTextMessageAsync(msg.Chat, "Проверка бота: работа корректна");
    }
    else if (msg.Text == "Привет" || msg.Text == "привет")
    {
        await bot.SendTextMessageAsync(msg.Chat, "Здравствуй, пользователь.");
    }
    else
    {
        await bot.SendTextMessageAsync(msg.Chat, "Такой команды пока нет. Доступные команды: /check, /start");
    }
}

async Task HandleCallbackQuery(CallbackQuery callbackQuery)//нажатия кнопок
{
    var chatId = callbackQuery.Message.Chat.Id;

    switch (callbackQuery.Data)
    {
        case "check":
            await bot.SendTextMessageAsync(chatId, "Проверка бота: работа корректна");
            break;
        case "chair1":
            // стул 1
            var photoUrl = "https://www.stolberi.ru/upload/iblock/2ea/45347ccc_28a3_11ea_8086_fcaa145d5838_d7d995d4_cb4a_11ea_c59a_005056bdce03.jpg"; 
            await bot.SendPhotoAsync(chatId, photoUrl);
            break;
        case "chair2":
            // стул 2
            var photoUrl2 = "https://ir.ozone.ru/s3/multimedia-r/c1000/6478407159.jpg"; 
            await bot.SendPhotoAsync(chatId, photoUrl2);
            break;
        case "chair3":
            // стул 3
            var photoUrl3 = "https://cs15.pikabu.ru/post_img/2024/10/22/11/172962549913092764.jpg";
            await bot.SendPhotoAsync(chatId, photoUrl3);
            break;
        case "clear_chat":
            // удаление сообщений
            var messagesToDelete = 5; // колво сообщений для удаления
            for (int i = 0; i < messagesToDelete; i++)
            {
                try
                {
                    
                    var messageIdToDelete = callbackQuery.Message.MessageId - i;
                    await bot.DeleteMessageAsync(chatId, messageIdToDelete);
                }
                catch (Exception ex)//недостаточно сообщений для удаления
                {
                    Console.WriteLine($"Ошибка при удалении сообщения: {ex.Message}");
                }
            }
            await bot.SendTextMessageAsync(chatId, "Чат очищен!");
            break;
        default:
            await bot.SendTextMessageAsync(chatId, "Неизвестный выбор.");
            break;
    }

   
    await bot.AnswerCallbackQueryAsync(callbackQuery.Id);
}

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    Console.WriteLine($"Error: {exception.Message}");
    return Task.CompletedTask;
}