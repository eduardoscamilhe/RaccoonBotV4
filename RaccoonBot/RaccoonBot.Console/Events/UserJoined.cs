using Discord.WebSocket;
using RaccoonBot.Domain.Constants;
using RaccoonBot.Domain.Settings;
using System.Linq;
using System.Threading.Tasks;

namespace RaccoonBot.Console.Events
{
    public class UserJoined
    {
        private Settings _settings;
        public UserJoined()
        {
            _settings = Settings.Instance;
        }
        public async Task AnnounceUserJoined(SocketGuildUser user)
        {
            await Task.Run(() =>
            {
                var guild = user.Guild;

                if (_settings.RaccoonId == guild.Id)
                {
                    var role = guild.Roles.FirstOrDefault(x => x.Name.ToLower() == CustomRoles.RaccoonsRole.ToLower());
                    guild.DefaultChannel.SendMessageAsync(string.Format(Messages.Welcome, user.Guild.Name, user.Mention, guild.MemberCount));
                    if (role != null) user.AddRoleAsync(role);
                }
            });
        }
    }
}

