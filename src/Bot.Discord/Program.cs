using Bot.Client.EventHandlers;
using Bot.Client.Services.DiscordLoggerService;
using Bot.Client.Services.RedditAPI;
using Bot.Client.Helpers;
using Bot.Common.StringService;
using Bot.DataAccess;
using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Bot.Client
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {

                    config.Sources.Clear();
                    config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("Settings/appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"Settings/appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .Build();
                })
                .ConfigureLogging(config =>
                {
                    config.AddConfiguration();
                    config.AddConsole();
                })
                .ConfigureDiscordHost((context, config) =>
                {
                    config.SocketConfig = new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Debug,
                        AlwaysDownloadUsers = false,
                        MessageCacheSize = 200,
                    };

                    config.Token = context.Configuration["Token"];
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("Default");
                    services.AddDbContext<GuildContext>((options) =>
                    {
                        options
                        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();
                    });

                    services
                        .AddHostedService<CommandEventHandler>()
                        .AddHostedService<MiscEventHandler>()
                        .AddTransient<Utilities>()
                        .AddTransient<IDiscordLogger, DiscordLogger>()
                        .AddTransient<IRedditAPIService, RedditAPIService>()
                        .AddSingleton<IStringService, StringService>();
                })
                .UseCommandService((context, config) =>
                {
                    config.CaseSensitiveCommands = false;
                    config.LogLevel = LogSeverity.Debug;
                    config.DefaultRunMode = Discord.Commands.RunMode.Async;
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
