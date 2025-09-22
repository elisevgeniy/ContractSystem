using ContractSystem.Core.DTO;
using ContractSystem.Service;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using User = ContractSystem.Core.DTO.User;

namespace Console.Advanced.Services;

public class UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger) : IUpdateHandler
{
    #region menues
    private static InlineKeyboardMarkup MenuMain = new InlineKeyboardMarkup()
            .AddNewRow()
                .AddButton("Список ваших договоров", "UserDocList")
            .AddNewRow()
                .AddButton("Добавить договор", "AddDoc")
            .AddNewRow()
                .AddButton("Согласовать договор", "ApproveDoc")
            .AddNewRow()
                .AddButton("Запросить договор на согласование", "AskDocForApprove");

    #endregion

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
            "/login" => SendLogin(msg),
            _ => Usage(msg)
        });
        logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.Id);
    }

    async Task<Message> Usage(Message msg)
    {
        const string usage = """
                <b><u>Меню</u></b>:
                /login       - Войти в систему
            """;
        return await bot.SendMessage(msg.Chat, usage, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
    }

    async Task<Message> SendLogin(Message msg)
    {
        var user = UserService.AddUser(msg.From.Username ?? msg.From.Id.ToString(), msg.From.FirstName);
        
        return await bot.SendMessage(msg.Chat, $"Вы вошли как {user.Firstname}", replyMarkup: MenuMain);
    }


    static Task<Message> FailingHandler(Message msg)
    {
        throw new NotImplementedException("FailingHandler");
    }

    // Process Inline Keyboard callback data
    private async Task OnCallbackQuery(CallbackQuery callbackQuery)
    {
        logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

        var user = UserService.GetUser(callbackQuery.From.Username ?? callbackQuery.From.Id.ToString());
        if (user == null)
        {
            await Usage(callbackQuery.Message!);
            return;
        }

        switch (callbackQuery.Data)
        {
            case "UserDocList":
                var docMessages = PrepareDocumentList(user);
                await bot.SendMessage(callbackQuery.Message!.Chat,
                    (docMessages.Count == 0) ? "Договоров нет" : "Список Ваших договоров:"
                    , ParseMode.Html);
                foreach (String message in docMessages)
                {
                    await bot.SendMessage(callbackQuery.Message!.Chat, message, ParseMode.Html);
                }
                break;
            case "AddDoc":
                var doc = DocumentService.AddDocumentByUser($"Doc-{user.Firstname}-{DateTime.Now.Ticks.ToString()}", "Some content", user);
                await bot.SendMessage(callbackQuery.Message!.Chat, $"Добавлен договор № {doc.Index}\nСогласован: {doc.IsApproved}\nСодержимое: {doc.Content}", ParseMode.Html);
                break;
            case "ApproveDoc":
                await bot.SendMessage(callbackQuery.Message!.Chat, $"Нажмите, чтобы согласовать:", ParseMode.Html, 
                    replyMarkup: PrepareDocumentApproveList(user)
                    );
                break;
            case "AskDocForApprove":
                await bot.AnswerCallbackQuery(callbackQuery.Id, $"Received {callbackQuery.Data}, but not implemented yet");
                break;
            default: 
                if (callbackQuery.Data!.StartsWith("ApproveDoc_"))
                {
                    string docIdStr = callbackQuery.Data.Split("_").ToList().Last();
                    if (int.TryParse(docIdStr, out int docId))
                    {
                        if (DocumentService.Approve(docId, user))
                            await bot.SendMessage(callbackQuery.Message!.Chat, "Договор согласован", ParseMode.Html);
                        else
                            await bot.SendMessage(callbackQuery.Message!.Chat, "Произошла ошибка", ParseMode.Html);
                    }
                }
                break;
        }
    }

    private List<String> PrepareDocumentList(User user)
    {
        var docs = DocumentService.GetDocumentByUser(user);
        var result = new List<String>();
        foreach (var doc in docs)
        {
            result.Add($"Договор № {doc.Index}\nСогласован: {doc.IsApproved}\nСодержимое: {doc.Content}");
        }

        return result;
    }

    private InlineKeyboardMarkup PrepareDocumentApproveList(User user)
    {
        var docs = DocumentService.GetDocumentByUser(user);
        InlineKeyboardMarkup docList = new InlineKeyboardMarkup();
        foreach (var doc in docs)
        {
            if (!doc.IsApproved) {
                docList = docList
                    .AddNewRow()
                        .AddButton($"№ {doc.Index}", $"ApproveDoc_{doc.Id}");
            }

        }
        return docList;
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
