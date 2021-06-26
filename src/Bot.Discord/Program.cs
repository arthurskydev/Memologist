﻿using BotCommon.Helpers;
using BotDiscord.EventHandlers;
using BotDiscord.Services.RedditAPI;
using BotCommon.StringService;
using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System.IO;
using System.Threading.Tasks;
using BotDiscord.Services.DiscordLoggerService;

namespace BotDiscord
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
                .ConfigureLogging(context =>
                {
                    context.AddConfiguration();
                    context.AddConsole();
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
                    services
                        .AddHostedService<CommandHandler>()
                        .AddHostedService<MiscEventHandler>()
                        .AddHostedService<SelfEventHandler>()
                        .AddTransient<Utilities>()
                        .AddTransient<IDiscordLogger, DiscordLogger>()
                        .AddTransient<IRedditAPIService, RedditAPIService>()
                        .AddSingleton<IStringService, StringService>();
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
