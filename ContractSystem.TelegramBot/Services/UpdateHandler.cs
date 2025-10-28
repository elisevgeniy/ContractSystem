using ContractSystem.Core.IRepositories;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using ContractSystem.Core.Models.Search;
using ContractSystem.Service;
using Mapster;
using Microsoft.Extensions.Logging;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using UserDTO = ContractSystem.Core.DTO.UserDTO;

namespace Console.Advanced.Services;

public class UpdateHandler : IUpdateHandler
{
    #region menues
    private static InlineKeyboardMarkup MenuMain = new InlineKeyboardMarkup()
            .AddNewRow()
                .AddButton("������ ����� ���������", "UserDocList")
            .AddNewRow()
                .AddButton("�������� �������", "AddDoc")
            .AddNewRow()
                .AddButton("����������� �������", "ApproveDoc")
            .AddNewRow()
                .AddButton("��������� ������� �� ������������", "AskDocForApprove");

    #endregion

    ITelegramBotClient bot;
    ILogger<UpdateHandler> logger;

    #region services
    UserService _userService;
    DocumentService _documentService;

    public UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger, UserService userService, DocumentService documentService)
    {
        this.bot = bot;
        this.logger = logger;
        _userService = userService;
        _documentService = documentService;
    }
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
            "/add" => AddDocument(msg),
            _ => Usage(msg)
        });
        logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.Id);
    }

    async Task<Message> Usage(Message msg)
    {
        const string usage = """
                <b><u>����</u></b>:
                /login       - ����� � �������
            """;
        return await bot.SendMessage(msg.Chat, usage, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
    }

    async Task<Message> SendLogin(Message msg)
    {
        UserOut user = _userService.AddUser(msg.From.Username ?? msg.From.Id.ToString(), msg.From.FirstName);
        
        return await bot.SendMessage(msg.Chat, $"�� ����� ��� {user.Firstname}", replyMarkup: MenuMain);
    }

    async Task<Message> AddDocument(Message msg)
    {
        String[] MessageData = msg.Text!.Split("\n");

        if (MessageData.Length == 1) return await bot.SendMessage(msg.Chat, "�� ����� ���� ����������, ��������� �����");
        String DocNumber = MessageData[0].Substring(5);
        String DocContent = MessageData.Length > 1 ? MessageData[1] : "������ ��������";

        var user = _userService.GetUser(msg.From!.Username ?? msg.From.Id.ToString());

        //try
        //{
            var doc = _documentService.AddDocument(
                new DocumentIn()
                {
                    Index = DocNumber,
                    Content = DocContent,
                    OwnerId = user.Id
                });
            return await bot.SendMessage(msg.Chat, $"�������� ������� � {doc.Index}\n����������: {doc.IsApproved}\n����������: {doc.Content}", ParseMode.Html);
        //} catch (Exception e)
        //{
        //    return await bot.SendMessage(msg.Chat, "���������, ������� � ����� ������� ��� ��������");
        //}
    }


    static Task<Message> FailingHandler(Message msg)
    {
        throw new NotImplementedException("FailingHandler");
    }

    // Process Inline Keyboard callback data
    private async Task OnCallbackQuery(CallbackQuery callbackQuery)
    {
        logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

        var user = _userService.GetUser(callbackQuery.From.Username ?? callbackQuery.From.Id.ToString());
        if (user == null)
        {
            await Usage(callbackQuery.Message!);
            return;
        }

        switch (callbackQuery.Data)
        {
            case "UserDocList":
                var docMessages = PrepareDocumentList(user.Adapt<UserSearch>());
                await bot.SendMessage(callbackQuery.Message!.Chat,
                    (docMessages.Count == 0) ? "��������� ���" : "������ ����� ���������:"
                    , ParseMode.Html);
                foreach (String message in docMessages)
                {
                    await bot.SendMessage(callbackQuery.Message!.Chat, message, ParseMode.Html);
                }
                break;
            case "AddDoc":
                await bot.SendMessage(callbackQuery.Message!.Chat, """
                    ��� ���������� ��������� ���������  � 2 ������:
                    /add ����� �������� (�������)
                    ���������� ��������
                    """);
                break;
            case "ApproveDoc":
                await bot.SendMessage(callbackQuery.Message!.Chat, $"�������, ����� �����������:", ParseMode.Html, 
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
                        try
                        {
                            _documentService.Approve(new ApprovalIn()
                            {
                                DocumentId = docId,
                                UserId = user.Id,
                            });
                            await bot.SendMessage(callbackQuery.Message!.Chat, "������� ����������", ParseMode.Html);
                        }
                        catch (Exception ex)
                        {
                            await bot.SendMessage(callbackQuery.Message!.Chat, "��������� ������", ParseMode.Html);
                        }
                    }
                }
                break;
        }
    }

    private List<String> PrepareDocumentList(UserSearch user)
    {
        var docs = _documentService.GetAllDocumentsByUser(user.Id);
        var result = new List<String>();
        foreach (var doc in docs)
        {
            result.Add($"������� � {doc.Index}\n����������: {doc.IsApproved}\n����������: {doc.Content}");
        }

        return result;
    }

    private InlineKeyboardMarkup PrepareDocumentApproveList(UserOut user)
    {
        var docs = _documentService.GetDocumentForApproveByUser(user.Id);
        InlineKeyboardMarkup docList = new InlineKeyboardMarkup();
        foreach (var doc in docs)
        {
            if (!doc.IsApproved) {
                docList = docList
                    .AddNewRow()
                        .AddButton($"� {doc.Index}", $"ApproveDoc_{doc.Id}");
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
