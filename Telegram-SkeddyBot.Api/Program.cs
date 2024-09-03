using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Telegram_SkeddyBot.Api.Extensions;
using Telegram_SkeddyBot.Core.Contracts;
using System.Threading.Tasks;
using System.Threading;

// Create a host for the application using the default configuration settings
var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        // Load configuration settings from appsettings.json
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Register the Telegram bot services defined in the extension method
        services.AddTelegramBotServices(context.Configuration);
    })
    .Build(); // Build the host

// Retrieve the registered ITelegramBotService from the built service provider
var botService = host.Services.GetRequiredService<ITelegramBotService>();

// Create a CancellationTokenSource to handle cancellation requests
var cancellationTokenSource = new CancellationTokenSource();

// Handle application exit (e.g., Ctrl+C) by requesting cancellation
Console.CancelKeyPress += (sender, eventArgs) =>
{
    // Cancel the operation when the user presses Ctrl+C
    cancellationTokenSource.Cancel();
    // Prevent the process from terminating immediately
    eventArgs.Cancel = true;
};

// Start the Telegram bot service asynchronously, passing the cancellation token
await botService.StartBotAsync(cancellationTokenSource.Token);
