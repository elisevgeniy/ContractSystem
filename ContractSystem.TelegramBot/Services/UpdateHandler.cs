using ContractSystem.Service;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Console.Advanced.Services;

public class UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger) : IUpdateHandler
{
    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleError: {Exception}", exception);
        // Cooldown in case of network connection error
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message }                        => OnMessage(message),
            { EditedMessage: { } message }                  => OnMessage(message),
            { CallbackQuery: { } callbackQuery }            => OnCallbackQuery(callbackQuery),
            { InlineQuery: { } inlineQuery }                => OnInlineQuery(inlineQuery),
            { ChosenInlineResult: { } chosenInlineResult }  => OnChosenInlineResult(chosenInlineResult),
            //{ Poll: { } poll }                              => OnPoll(poll),
            //{ PollAnswer: { } pollAnswer }                  => OnPollAnswer(pollAnswer),
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            _                                               => UnknownUpdateHandlerAsync(update)
        });
    }

    private async Task OnMessage(Message msg)
    {
        logger.LogInformation("Receive message type: {MessageType}", msg.Type);
        if (msg.Text is not { } messageText)
            return;

        Message sentMessage = await (messageText.Split(' ')[0] switch
        {
            "/register" => SendRegister(msg),
            _ => Usage(msg)
        });
        logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.Id);
    }

    async Task<Message> Usage(Message msg)
    {
        const string usage = """
                <b><u>Меню</u></b>:
                /register       - Зарегистрироваться
            """;
        return await bot.SendMessage(msg.Chat, usage, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
    }

    async Task<Message> SendRegister(Message msg)
    {
        var user = UserService.AddUser(msg.From.Username ?? msg.From.Id.ToString(), msg.From.FirstName);

        var inlineMarkup = new InlineKeyboardMarkup()
            .AddNewRow()
                .AddButton("Список ваших договоров", "UserDocList")
            .AddNewRow()
                .AddButton("Добавить договор", "AddDoc")
            .AddNewRow()
                .AddButton("Согласовать договор", "ApproveDoc")
            .AddNewRow()
                .AddButton("Запросить договор на согласование", "AslDocForApprove");
        return await bot.SendMessage(msg.Chat, $"Вы зарегистрированы как {user.Firstname}", replyMarkup: inlineMarkup);
    }


    static Task<Message> FailingHandler(Message msg)
    {
        throw new NotImplementedException("FailingHandler");
    }

    // Process Inline Keyboard callback data
    private async Task OnCallbackQuery(CallbackQuery callbackQuery)
    {
        logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);
        await bot.AnswerCallbackQuery(callbackQuery.Id, $"Received {callbackQuery.Data}");
        await bot.SendMessage(callbackQuery.Message!.Chat, $"Received {callbackQuery.Data}");
    }

    #region Inline Mode

    private async Task OnInlineQuery(InlineQuery inlineQuery)
    {
        logger.LogInformation("Received inline query from: {InlineQueryFromId}", inlineQuery.From.Id);

        InlineQueryResult[] results = [ // displayed result
            new InlineQueryResultArticle("1", "Telegram.Bot", new InputTextMessageContent("hello")),
            new InlineQueryResultArticle("2", "is the best", new InputTextMessageContent("world"))
        ];
        await bot.AnswerInlineQuery(inlineQuery.Id, results, cacheTime: 0, isPersonal: true);
    }

    private async Task OnChosenInlineResult(ChosenInlineResult chosenInlineResult)
    {
        logger.LogInformation("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);
        await bot.SendMessage(chosenInlineResult.From.Id, $"You chose result with Id: {chosenInlineResult.ResultId}");
    }

    #endregion

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
}
