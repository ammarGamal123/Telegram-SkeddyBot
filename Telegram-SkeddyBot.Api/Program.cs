using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Telegram_SkeddyBot.Api.Extensions;
using Telegram_SkeddyBot.Core.Contracts;
using System.Threading.Tasks;
using System.Threading;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddTelegramBotServices(context.Configuration);
    })
    .Build();

var botService = host.Services.GetRequiredService<ITelegramBotService>();
var cancellationTokenSource = new CancellationTokenSource();

// Handle application exit (e.g., Ctrl+C) and request cancellation
Console.CancelKeyPress += (sender, eventArgs) =>
{
    cancellationTokenSource.Cancel();
    eventArgs.Cancel = true;
};

await botService.StartBotAsync(cancellationTokenSource.Token);
