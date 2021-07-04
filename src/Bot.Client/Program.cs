using Bot.Client.EventHandlers;
using Bot.Client.Helpers;
using Bot.Client.Services.DiscordLoggerService;
using Bot.Client.Services.RedditAPI;
using Bot.Common;
using Bot.Common.Contract;
using Bot.DataAccess;
using Bot.DataAccess.Contract;
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
            var host = CreateHostBuilder(args).Build();
            using (host)
            {
                await host.RunAsync();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
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
                .UseCommandService((context, config) =>
                {
                    config.CaseSensitiveCommands = false;
                    config.LogLevel = LogSeverity.Debug;
                    config.DefaultRunMode = Discord.Commands.RunMode.Async;
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("Default");

                    services
                        .AddDbContext<BotContext>(options =>
                        {
                            options
                              .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                              .EnableSensitiveDataLogging()
                              .EnableDetailedErrors();
                        })
                        .AddHostedService<CommandHandler>()
                        .AddHostedService<MiscEventHandler>()
                        .AddHostedService<GuildConntectedHandler>()
                        .AddTransient<Utilities>()
                        .AddTransient<IDiscordLogger, DiscordLogger>()
                        .AddTransient<IRedditAPIService, RedditAPIService>()
                        .AddSingleton<IDataAccessLayer, DataAccessLayer>()
                        .AddSingleton<IStringService, StringService>();
                })
                .UseConsoleLifetime();

            return builder;
        }
    }
}
