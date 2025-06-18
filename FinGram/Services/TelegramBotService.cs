using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FinGram.Data;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Services
{
    public class TelegramBotService : BackgroundService
    {
        private readonly ILogger<TelegramBotService> _logger;
        private readonly IConfiguration _configuration;
        private TelegramBotClient _botClient;

        public TelegramBotService(ILogger<TelegramBotService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string botToken = _configuration["TelegramBotToken"];

            _botClient = new TelegramBotClient(botToken);

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                cancellationToken: stoppingToken
            );

            var me = await _botClient.GetMeAsync();
            _logger.LogInformation($"✅ Бот @{me.Username} запущен");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
        {
            if (update.Type != UpdateType.Message || update.Message?.Text == null)
                return;

            var message = update.Message;
            if (message.Text.StartsWith("/start"))
            {
                await HandleStartCommand(bot, message);
            }
            else if (message.Text == "/stats")
            {
                await HandleStatsCommand(bot, message);
            }
            else
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Пожалуйста, используйте /start <токен> для начала.");
            }
        }

        private async Task HandleStartCommand(ITelegramBotClient bot, Message message)
        {
            var args = message.Text.Split(' ');
            if (args.Length != 2)
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Неверный формат. Перейдите по ссылке с сайта.");
                return;
            }

            string token = args[1];
            int telegramId = (int)message.Chat.Id;

            var request = new
            {
                Token = token,
                TelegramId = telegramId
            };

            using var client = new HttpClient();

            try
            {
                var response = await client.PostAsJsonAsync("https://localhost:7190/api/telegramauth", request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TelegramAuthResponse>();
                    await bot.SendTextMessageAsync(telegramId, $"Вы авторизованы как {result.UserName}!");
                }
                else
                {
                    await bot.SendTextMessageAsync(telegramId, "Ошибка авторизации. Токен может быть неверным или использован.");
                }
            }
            catch (Exception ex)
            {
                await bot.SendTextMessageAsync(telegramId, "Сервер недоступен. Попробуйте позже.");
                _logger.LogError(ex, "Ошибка при запросе авторизации");
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiEx => $"Telegram API Error: {apiEx.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(errorMessage);
            return Task.CompletedTask;
        }

        private async Task HandleStatsCommand(ITelegramBotClient bot, Message message)
        {
            using var scope = Program.ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var telegramId = (int)message.Chat.Id;
            var userLink = await context.TelegramLinks
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TelegramId == telegramId);

            if (userLink?.User == null)
            {
                await bot.SendTextMessageAsync(telegramId, "Вы не авторизованы. Используйте ссылку из личного кабинета.");
                return;
            }

            var userId = userLink.UserId;

            var userResults = await context.UserTestResults
                .Where(r => r.UserId == userId)
                .ToListAsync();

            int currentPoints = userResults.Sum(r => r.Score);
            int maxPoints = await context.Questions.CountAsync();

            int lessonsPassed = await context.UserLessons
                .CountAsync(ul => ul.UserId == userId);

            int pointsToCertificate = Math.Max((int)Math.Ceiling(maxPoints / 2.0) - currentPoints, 0);

            string response = $"\uD83D\uDCCA Ваша статистика:\n\n" +
                              $"\u2705 Пройдено уроков: {lessonsPassed}\n" +
                              $"\uD83C\uDFAF Баллы за тесты: {currentPoints} / {maxPoints}\n" +
                              $"\uD83C\uDFC5 Осталось до сертификата: {pointsToCertificate} баллов";

            await bot.SendTextMessageAsync(telegramId, response);
        }

        private class TelegramAuthResponse
        {
            [JsonPropertyName("userId")]
            public int UserId { get; set; }

            [JsonPropertyName("userName")]
            public string UserName { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }
        }
    }
}
