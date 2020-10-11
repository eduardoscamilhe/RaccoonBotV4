using Discord.WebSocket;
using RaccoonBot.Domain.Constants;
using RaccoonBot.Domain.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaccoonBot.Console.Events
{
    public class UserVoiceStateUpdated
    {
        private DiscordSocketClient _client;
        private Settings _settings;
        public UserVoiceStateUpdated(DiscordSocketClient client)
        {
            _client = client;
            _settings = Settings.Instance;
        }
        public async Task MostPlaybleGame(SocketUser socketUser, SocketVoiceState before, SocketVoiceState after)
        {
            await Task.Run(() =>
            {
                try
                {
                    var raccoonGuild = _client.GetGuild(_settings.RaccoonId);
                    var role = raccoonGuild.Roles.FirstOrDefault(x => x.Name == CustomRoles.MostPlaybleGame);
                    if (after.VoiceChannel != null)
                    {

                        var usersVoice = raccoonGuild.VoiceChannels.Select(x => x.Users).Where(x => x.Count > 0).ToList();
                        List<SocketGuildUser> users = new List<SocketGuildUser>();

                        Dictionary<string, int> gamesMostWanted = new Dictionary<string, int>();

                        usersVoice.ForEach(x => { x.ToList().ForEach(y => { users.Add(y); }); });

                        var gameMost = users.Where(x => x.Activity != null).GroupBy(k => k.Activity.Name).ToDictionary(g => g.Key, g => g.Count())
                            .Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                        var usersHighLight = users.Where(x => x.Activity != null && x.Activity.Name == gameMost).ToList();
                        var removeRole = users.Except(usersHighLight);

                        foreach (var user in usersHighLight)
                            if (!user.Roles.Any(x => x.Name == role.Name))
                                user.AddRoleAsync(role);

                        foreach (var user in removeRole)
                            if (user.Roles.Any(x => x.Name == role.Name))
                                user.RemoveRoleAsync(role);

                    }
                    else
                    {
                        if (role != null) (socketUser as SocketGuildUser).RemoveRoleAsync(role);
                    }
                }
                catch
                {


                }
            });

        }
        public async Task VoiceLog(SocketUser socketUser, SocketVoiceState before, SocketVoiceState after)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (after.VoiceChannel != null && _settings.RaccoonId == after.VoiceChannel.Guild.Id && socketUser.Id != _settings.RaccoonPlayerId)
                    {
                        var raccoonGuild = _client.GetGuild(_settings.RaccoonId);
                        var textChannel = raccoonGuild.TextChannels.FirstOrDefault(x => x.Name.Contains(Channels.VoiceLog));
                        if (before.VoiceChannel != null && after.VoiceChannel == null)
                        {

                            textChannel.SendMessageAsync(
                               string.Format(Messages.VoiceLogLeft, socketUser.Mention, before.VoiceChannel.Name)
                               );
                        }

                        if (after.VoiceChannel != null && before.VoiceChannel != after.VoiceChannel)
                        {

                            textChannel.SendMessageAsync(
                               string.Format(Messages.VoiceLogJoin, socketUser.Mention, after.VoiceChannel.Name)
                               );

                        }
                    }
                }
                catch
                {


                }
            });
        }

        public async Task RemoveMuteMaster(SocketUser socketUser, SocketVoiceState before, SocketVoiceState after)
        {
            await Task.Run(() =>
            {
                var guild = before.VoiceChannel?.Guild;
                var muteMasterRole = guild?.Roles.FirstOrDefault(x => x.Name == CustomRoles.MuteMaster);

                if (muteMasterRole != null && after.VoiceChannel == null)
                {
                    var socketGuildUser = (socketUser as SocketGuildUser);
                    socketGuildUser.RemoveRoleAsync(muteMasterRole);
                }
            });
        }
    }
}
