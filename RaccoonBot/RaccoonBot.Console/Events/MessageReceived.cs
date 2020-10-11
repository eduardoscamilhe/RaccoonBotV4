using Discord;
using Discord.WebSocket;
using RaccoonBot.Domain.Constants;
using RaccoonBot.Domain.Settings;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RaccoonBot.Console.Events
{
    public class MessageReceived
    {
        private DiscordSocketClient _client;
        private Settings _settings;
        public MessageReceived(DiscordSocketClient client)
        {
            _client = client;
            _settings = Settings.Instance;
        }
        public async Task RemoveLastMessagePSO(SocketMessage msg)
        {

            try
            {
                if (msg.Channel.Name.Contains(Channels.PSO))
                {
                    var raccoonGuild = _client.GetGuild(_settings.RaccoonId);
                    var psoChannel = raccoonGuild.TextChannels.FirstOrDefault(x => x.Name.Contains(Channels.PSO));
                    var messages = await psoChannel.GetMessagesAsync(1000).FlattenAsync();
                    var msgs = messages.Where(x => x.CreatedAt <= DateTime.Now.AddHours(-3) && x != msg);
                    await psoChannel.DeleteMessagesAsync(msgs);
                }
            }
            catch { }


        }

        public async Task GameDealsUpdate(SocketMessage msg)
        {
            try
            {
                Regex rx = new Regex(CustomRegex.RegexUrl);

                var raccoonGuild = _client.GetGuild(_settings.RaccoonId);

                var match = rx.Match(msg.Content);

                if (match.Success && raccoonGuild != null)
                {
                    var raccoonChannel = raccoonGuild.TextChannels.FirstOrDefault(x => x.Name.Contains(Channels.FreeGames));
                    var lastmessage = await ((SocketTextChannel)raccoonChannel).GetMessagesAsync(1).FlattenAsync();

                    if (msg.Id == lastmessage.FirstOrDefault().Id)
                    {
                        foreach (var guild in _client.Guilds.Where(x => x.Id != _settings.RaccoonId))
                        {
                            var channelGuild = guild.TextChannels.FirstOrDefault(x => x.Name.Contains(Channels.FreeGames));

                            if (channelGuild != null)
                                await ((SocketTextChannel)channelGuild).SendMessageAsync(msg.Content);
                        }
                    }

                }
            }
            catch { }
        }
        public async Task BotContact(SocketMessage msg)
        {
            try
            {
                var raccoonGuild = _client.GetGuild(_settings.RaccoonId);

                if (raccoonGuild != null)
                {
                    var raccoonChannel = raccoonGuild.TextChannels.FirstOrDefault(x => x.Name.Contains(Channels.BotContact));
                    var lastmessage = await ((SocketTextChannel)raccoonChannel).GetMessagesAsync(1).FlattenAsync();

                    if (msg.Id == lastmessage.FirstOrDefault().Id)
                    {
                        foreach (var guild in _client.Guilds.Where(x => x.Id != _settings.RaccoonId))
                        {
                            var channelGuild = guild.DefaultChannel;

                            if (channelGuild != null)
                                await ((SocketTextChannel)channelGuild).SendMessageAsync(msg.Content);
                        }
                    }
                }
            }
            catch { }
        }
    }
}
