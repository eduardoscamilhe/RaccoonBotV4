using Discord;
using Discord.WebSocket;
using RaccoonBot.Domain.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RaccoonBot.Console.Events
{
    public class GuildMemberUpdated
    {
        private Settings _settings;
        public GuildMemberUpdated()
        {
            _settings = Settings.Instance;
        }
        public async Task RoleCreateByGame(SocketGuildUser before, SocketGuildUser after)
        {
            var guild = after.Guild;
            if (_settings.RaccoonId == guild.Id)
            {
                var listActivity = guild.Users.Where(x => x.Activity != null && x.Activity.Type == ActivityType.Playing && !x.IsBot).Select(x => x.Activity).ToList();

                Random rnd = new Random();

                foreach (var activity in listActivity)
                {
                    var roleTOCreate = guild.Roles.Where(x => x.Name.ToUpper() == activity.Name.ToUpper()).FirstOrDefault();
                    if (roleTOCreate == null)
                    {
                        try
                        {
                            await guild.CreateRoleAsync(activity.Name, null, new Color(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)), true, null);
                        }
                        catch { }
                    }

                }
            }

        }

        public async Task AddRoleByGame(SocketGuildUser before, SocketGuildUser after)
        {
            var guild = after.Guild;
            if (_settings.RaccoonId == guild.Id)
            {
                var listActivity = guild.Users.Where(x => x.Activity != null && x.Activity.Type == ActivityType.Playing).Select(x => x.Activity).ToList();

                foreach (var activity in listActivity)
                {
                    var role = guild.Roles.Where(x => x.Name.ToUpper() == activity.Name.ToUpper()).FirstOrDefault();
                    var usersPlaying = guild.Users.Where(x => x.Activity != null && x.Activity.Name.ToUpper() == activity.Name.ToUpper() && !x.IsBot);
                    foreach (var user in usersPlaying)
                    {

                        if (role != null && user.Roles.Count(x => x.Name.ToUpper() == activity.Name.ToUpper()) == 0)
                        {
                            try
                            {
                                await user.AddRoleAsync(role);
                            }
                            catch { }
                        }

                    }
                }
            }
        }

    }
}
