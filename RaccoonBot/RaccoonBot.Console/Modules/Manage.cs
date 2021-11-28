using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RaccoonBot.Domain.Command;
using RaccoonBot.Domain.Constants;
using System.Linq;
using System.Threading.Tasks;

namespace RaccoonBot.Modules
{
    public class Manage : ModuleBase<SocketCommandContext>
    {
        private string _inviteLink = "https://discordapp.com/api/oauth2/authorize?client_id={0}&permissions=8&scope=bot";
        public CommandService Service { get; set; }
        private Settings _settings = Settings.Instance;

        [Command(Commands.Ping)]
        [Summary(Summary.Ping)]
        public async Task PingDefault()
        {
            await Task.Run(() =>
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Pong!").WithDescription("Pong Pong Pong!").WithColor(Color.Blue);
                ReplyAsync("", false, builder.Build());
            });
        }




        [Command(Commands.Clean), RequireUserPermission(ChannelPermission.ManageMessages)]
        [Summary(Summary.Clean)]
        public async Task Clean(int delnum = 0)
        {
            var messages = await Context.Channel.GetMessagesAsync(delnum + 1).FlattenAsync();
            var channel = (Context.Channel as SocketTextChannel);

            try
            {
                await channel.DeleteMessagesAsync(messages);
            }
            catch
            {

                if (delnum > 100)
                {
                    await ReplyAsync("100 é o maximo de mensagens a serem excluidas.");
                    return;
                }

                foreach (var msg in messages)
                {
                    await channel.DeleteMessageAsync(msg.Id);
                }

            }
        }

        #region Invites
        [Command(Commands.InviteBot)]
        [Summary(Summary.InviteBot)]
        public async Task InviteBot()
        {
            try
            {
                await Context.User.SendMessageAsync(string.Format(_inviteLink, Context.Client.CurrentUser.Id));
            }
            catch { }
        }

        [Command(Commands.InviteOriginDiscord)]
        [Summary(Summary.InviteOriginDiscord)]
        public async Task InviteOriginDiscord()
        {
            try
            {
                await Context.User.SendMessageAsync(_settings.OriginDiscord);
            }
            catch { }
        }

        [Command(Commands.InviteDiscord)]
        [Summary(Summary.InviteDiscord)]
        public async Task InviteDiscord()
        {
            try
            {
                var arrInvite = await Context.Guild.GetInvitesAsync();
                await Context.User.SendMessageAsync(arrInvite.FirstOrDefault().ToString());
            }
            catch { }
        }

        #endregion

        [Command(Commands.Suggest)]
        [Summary(Summary.Suggest)]
        public async Task Suggest([Remainder] string suggest)
        {
            var DiscordUser = Context.Client.GetApplicationInfoAsync().Result.Owner;
            await DiscordUser.SendMessageAsync(string.Format(Messages.SuggestMention, Context.User.Mention, suggest));
        }
    }
}