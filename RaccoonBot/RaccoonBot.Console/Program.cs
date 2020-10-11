using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using RaccoonBot.Console.Events;
using RaccoonBot.Domain.Command;
using RaccoonBot.Domain.Settings;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RaccoonBot.Console
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private UserVoiceStateUpdated _userVoiceStateUpdate;
        private MessageReceived _messageReceived;
        private UserJoined _userJoined;
        private GuildMemberUpdated _guildMemberUpdated;
        private Settings _settings;
        public async Task RunBotAsync()
        {

            _client = new DiscordSocketClient();
            _commands = new CommandService();

            #region Grouping Instance 
            _userVoiceStateUpdate = new UserVoiceStateUpdated(_client);
            _messageReceived = new MessageReceived(_client);
            _userJoined = new UserJoined();
            _guildMemberUpdated = new GuildMemberUpdated();
            _settings = Settings.Instance;
            #endregion

            _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .AddSingleton(_userVoiceStateUpdate)
            .AddSingleton(_messageReceived)
            .AddSingleton(_userJoined)
            .AddSingleton(_guildMemberUpdated)
            .AddSingleton(_settings)
            .BuildServiceProvider();

            #region Organization

            _client.UserJoined += _userJoined.AnnounceUserJoined;


            _client.GuildMemberUpdated += _guildMemberUpdated.RoleCreateByGame;

            _client.GuildMemberUpdated += _guildMemberUpdated.AddRoleByGame;
            _client.MessageReceived += _messageReceived.GameDealsUpdate;
            _client.MessageReceived += _messageReceived.BotContact;
            _client.MessageReceived += _messageReceived.RemoveLastMessagePSO;
            _client.UserVoiceStateUpdated += _userVoiceStateUpdate.MostPlaybleGame;
            _client.UserVoiceStateUpdated += _userVoiceStateUpdate.VoiceLog;
            _client.UserVoiceStateUpdated += _userVoiceStateUpdate.RemoveMuteMaster;

            _client.Log += Log;
            #endregion

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