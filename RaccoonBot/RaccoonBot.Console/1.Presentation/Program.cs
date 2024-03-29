﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using RaccoonBot.Domain.Command;
using RaccoonBot.Domain.Constants;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace RaccoonBot
{
    internal class Program
    {
        private static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private Settings _settings;
        private SocketGuild logServer;

        public async Task RunBotAsync()
        {
            var config = new DiscordSocketConfig { GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent };
            _client = new DiscordSocketClient(config);
            _commands = new CommandService();
            _settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("./settings.json"));

            _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .AddSingleton(_settings)
            .BuildServiceProvider();

            _client.UserJoined += AnnounceUserJoined;
            _client.GuildMemberUpdated += AddRoleGame;
            _client.UserVoiceStateUpdated += VoiceLog;
            _client.Log += Log;

            #region Starting Bot

            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, _settings.BotToken);
            await _client.SetGameAsync(_settings.Prefix + Commands.Help);

            await _client.StartAsync();
            await Task.Delay(-1);

            #endregion Starting Bot
        }

        private async Task AddRoleGame(Cacheable<SocketGuildUser, ulong> userParameter, SocketGuildUser after)
        {
            var guild = after.Guild;
            if (_settings.RaccoonlandId == guild.Id)
            {
                var listActivity = guild.Users.Where(x => x.Activities.Any() && !x.IsBot).Select(x => x.Activities).ToList();

                foreach (var activity in listActivity)
                {
                    var role = guild.Roles.Where(x => x.Name.ToUpper() == activity.FirstOrDefault().Name.ToUpper()).FirstOrDefault();
                    var usersPlaying = guild.Users.Where(x => x.Activities.Any() && x.Activities.FirstOrDefault().Name.ToUpper() == activity.FirstOrDefault().Name.ToUpper() && !x.IsBot);
                    foreach (var user in usersPlaying)
                    {
                        if (role != null && user.Roles.Count(x => x.Name.ToUpper() == activity.FirstOrDefault().Name.ToUpper()) == 0)
                        {
                            try
                            {
                                await user.AddRoleAsync(role);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
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

            if (message.HasStringPrefix(Commands.prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)
                || message.HasStringPrefix(Commands.optPrefix, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                    System.Console.WriteLine(result.ErrorReason);
            }
        }

        #endregion Command Register

        private async Task VoiceLog(SocketUser socketUser, SocketVoiceState before, SocketVoiceState after)
        {
            try
            {
                logServer = _client.GetGuild(Servers.Logs.FirstOrDefault());

                var user = (SocketGuildUser)socketUser;
                SocketTextChannel channelLog = null;

                if (user.Guild.Id == Servers.Raccoonland.FirstOrDefault())
                    channelLog = logServer.GetTextChannel(Servers.Raccoonland.LastOrDefault());

                if (channelLog != null)
                    await SendLog(socketUser, before.VoiceChannel, after.VoiceChannel, channelLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task SendLog(SocketUser socketUser, SocketVoiceChannel beforeChannel, SocketVoiceChannel afterChannel, SocketTextChannel textLog)
        {
            if (beforeChannel != null && afterChannel == null)
            {
                await textLog.SendMessageAsync(
                    string.Format(Messages.VoiceLogLeft, socketUser.Username, beforeChannel.Name, DateTime.Now.ToString("dd/MM HH:mm:ss"))
                    );
            }
            if (afterChannel != null && beforeChannel != afterChannel)
            {
                await textLog.SendMessageAsync(
                    string.Format(Messages.VoiceLogJoin, socketUser.Username, afterChannel.Name, DateTime.Now.ToString("dd/MM HH:mm:ss"))
                    );
            }
        }

        public async Task AnnounceUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;

            if (_settings.RaccoonlandId == guild.Id)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Name.ToLower().Contains(CustomRoles.RaccoonsRole.ToLower()));
                await guild.GetTextChannel(220277571798040576)
                    .SendMessageAsync(string.Format(Messages.Welcome, user.Guild.Name, user.Mention));
                if (role != null) await user.AddRoleAsync(role);
            }
        }
    }
}