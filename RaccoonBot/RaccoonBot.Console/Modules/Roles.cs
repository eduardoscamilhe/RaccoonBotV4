using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RaccoonBot.Domain.Command;
using RaccoonBot.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaccoonBot.Modules
{
    public class Role : ModuleBase<SocketCommandContext>
    {
        private Settings _settings = Settings.Instance;


        [Command(Commands.NSFW)]
        [Summary(Summary.NSFW)]
        public async Task GetNSFW()
        {
            await Task.Run(() =>
            {
                var role = Context.Guild.Roles.Where(x => x.Name.Contains(CustomRoles.NsfwRole)).FirstOrDefault();
                if (role == null)
                {
                    Context.Channel.SendMessageAsync(string.Format(Messages.NullRoleMessage, role.Name));
                    return;
                }
                var userWithRole = role.Members.FirstOrDefault(x => x == Context.User);

                if (userWithRole == null)
                    (Context.User as IGuildUser).AddRoleAsync(role);
                else
                    (Context.User as IGuildUser).RemoveRoleAsync(role);

                Context.Channel.DeleteMessageAsync(Context.Message.Id);
            });
        }

        [Command(Commands.BlackHumor)]
        [Summary(Summary.BlackHumor)]
        public async Task GetBlackHumor()
        {
            await Task.Run(() =>
            {
                var role = Context.Guild.Roles.Where(x => x.Name.Contains(CustomRoles.BlackHumorRole)).FirstOrDefault();
                var userWithRole = role.Members.FirstOrDefault(x => x == Context.User);
                if (role == null)
                {
                    Context.Channel.SendMessageAsync(string.Format(Messages.NullRoleMessage, role.Name));
                    return;
                }

                if (userWithRole == null)
                    (Context.User as IGuildUser).AddRoleAsync(role);
                else
                    (Context.User as IGuildUser).RemoveRoleAsync(role);

                Context.Channel.DeleteMessageAsync(Context.Message.Id);
            });
        }

        [Command(Commands.IPlay)]
        [Summary(Summary.IPlay)]
        public async Task IPlay([Remainder] string nameOfGame)
        {
            await Task.Run(() =>
            {
                var roleGame = Context.Guild.Roles.FirstOrDefault(x =>
                    !x.Permissions.Administrator &&
                    !x.Permissions.BanMembers &&
                    !x.Permissions.KickMembers &&
                    !x.Permissions.DeafenMembers &&
                    !x.Permissions.ManageMessages &&
                    !x.Permissions.ManageChannels &&
                    !x.Permissions.ManageRoles &&
                    !x.Permissions.MoveMembers &&
                    !x.Permissions.MuteMembers &&
                    !x.Permissions.ManageGuild &&
                x.Name.ToLower().Contains(nameOfGame.ToLower()));
                if (roleGame != null)
                {
                    (Context.User as SocketGuildUser).AddRoleAsync(roleGame);
                    Context.Channel.SendMessageAsync(string.Format(Messages.IPlaySuccess, roleGame.Name, Context.User.Username));
                }
                else Context.Channel.SendMessageAsync(Messages.IPlayFail);
            });
        }

        [Command(Commands.GameRoles)]
        public async Task GameRoles()
        {
            var guild = Context.Guild;
            var rolesGame = guild.Roles.Where(x =>
                !x.Permissions.Administrator &&
                !x.Permissions.BanMembers &&
                !x.Permissions.KickMembers &&
                !x.Permissions.DeafenMembers &&
                !x.Permissions.ManageMessages &&
                !x.Permissions.ManageChannels &&
                !x.Permissions.ManageRoles &&
                !x.Permissions.MoveMembers &&
                !x.Permissions.MuteMembers &&
                !x.Permissions.ManageGuild &&
                !x.IsEveryone &&
                x.IsHoisted && x.Name != "Raccoons"
                );
            EmbedBuilder embedBuilder = new EmbedBuilder();

            embedBuilder.WithTitle("Quantidade de pessoas por roles:");
            var textEmbedArray = new List<string>();

            foreach (var roleGame in rolesGame)
            {
                var countPerson = guild.Users.Count(x => x.Roles.Any(i => i.Name.Contains(roleGame.Name)));
                textEmbedArray.Add($@"{ roleGame.Name} : {countPerson}");
            }

            embedBuilder.WithDescription(string.Join(Environment.NewLine, textEmbedArray));
            await ReplyAsync("", false, embedBuilder.Build());

        }



    }
}
