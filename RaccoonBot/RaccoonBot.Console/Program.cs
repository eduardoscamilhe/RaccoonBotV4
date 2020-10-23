using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using RaccoonBot.Domain.Command;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RaccoonBot
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private Settings _settings;
        public async Task RunBotAsync()
        {

            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _settings = Settings.Instance;

            _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .AddSingleton(_settings)
            .BuildServiceProvider();

            _client.Log += Log;

            #region Starting Bot
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, _settings.BotToken);
            await _client.SetGameAsync(_settings.Prefix + Commands.Help);

            await _client.StartAsync();
            await Task.Delay(-1);
            #endregion
        }


        private Task Log(LogMessage arg)
        {
            System.Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        #region Command Register
        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message is null || message.Author.IsBot)
                return;
            int argPos = 0;

            if (message.HasStringPrefix(Commands.prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                    System.Console.WriteLine(result.ErrorReason);
            }

        }
        #endregion
    }
}