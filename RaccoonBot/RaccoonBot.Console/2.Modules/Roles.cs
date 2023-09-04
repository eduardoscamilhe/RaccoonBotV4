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
        [Command(Commands.NSFW)]
        [Summary(Summary.NSFW)]
        public async Task GetNSFW()
        {
            var role = Context.Guild.Roles.Where(x => x.Name.Contains(CustomRoles.NsfwRole)).FirstOrDefault();
            if (role == null)
            {
                await Context.Channel.SendMessageAsync(string.Format(Messages.NullRoleMessage, role.Name));
                return;
            }
            var userWithRole = role.Members.FirstOrDefault(x => x == Context.User);

            if (userWithRole == null)
                await (Context.User as IGuildUser).AddRoleAsync(role);
            else
                await (Context.User as IGuildUser).RemoveRoleAsync(role);

            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
        }

        [Command(Commands.DarkHumor)]
        [Summary(Summary.DarkHumor)]
        public async Task GetDarkHumor()
        {
            var role = Context.Guild.Roles.Where(x => x.Name.Contains(CustomRoles.DarkHumorRole)).FirstOrDefault();
            var userWithRole = role.Members.FirstOrDefault(x => x == Context.User);
            if (role == null)
            {
                await Context.Channel.SendMessageAsync(string.Format(Messages.NullRoleMessage, role.Name));
                return;
            }

            if (userWithRole == null)
                await (Context.User as IGuildUser).AddRoleAsync(role);
            else
                await (Context.User as IGuildUser).RemoveRoleAsync(role);

            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
        }

        [Command(Commands.IPlay)]
        [Summary(Summary.IPlay)]
        public async Task IPlay([Remainder] string nameOfGame)
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
                await (Context.User as SocketGuildUser).AddRoleAsync(roleGame);
                await Context.Channel.SendMessageAsync(string.Format(Messages.IPlaySuccess, roleGame.Name, Context.User.Username));
            }
            else await Context.Channel.SendMessageAsync(Messages.IPlayFail);
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
                textEmbedArray.Add($@"{roleGame.Name} : {countPerson}");
            }

            embedBuilder.WithDescription(string.Join(Environment.NewLine, textEmbedArray));
            await ReplyAsync("", false, embedBuilder.Build());
        }
    }
}