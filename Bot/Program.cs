namespace Bot
{
    using Bot.Services.StringProcService;
    using Bot.Utils;
    using Bot.EventHandlers;
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Configuration;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(x =>
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();

                    x.AddConfiguration(configuration);
                })
                .ConfigureLogging(x =>
                {
                    x.AddConfiguration();
                    x.AddConsole();
                })
                .ConfigureDiscordHost<DiscordSocketClient>((context, config) =>
                {
                    config.SocketConfig = new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Debug,
                        AlwaysDownloadUsers = false,
                        MessageCacheSize = 200,
                    };

                    config.Token = context.Configuration["Token"];
                })
                .UseCommandService((context, config) =>
                {
                    config.CaseSensitiveCommands = true;
                    config.LogLevel = LogSeverity.Debug;
                    config.DefaultRunMode = Discord.Commands.RunMode.Async;
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddHostedService<CommandExecutedEventHandler>()
                        .AddHostedService<MessageRecievedEventHandler>()
                        .AddTransient<Utilities>()
                        .AddSingleton<IStringProcService, StringProcService>();
                })
                .UseConsoleLifetime();

            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}
