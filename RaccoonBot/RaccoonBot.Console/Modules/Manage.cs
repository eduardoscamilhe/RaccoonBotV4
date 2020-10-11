using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RaccoonBot.Domain.Command;
using RaccoonBot.Domain.Constants;
using RaccoonBot.Domain.Settings;
using RaccoonBot.Domain.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RaccoonBot.Console.Modules
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


        [Command(Commands.MuteChannel)]
        [Summary(Summary.MuteChannel)]
        public async Task MuteUnmuteEveryoneInChat()
        {
            await Task.Run(() =>
            {
                var author = (SocketGuildUser)Context.Message.Author;

                if (author.Roles.Any(x => x.Name == CustomRoles.MuteMaster))
                {
                    var channelAuthor = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Users.Any(u => u.Id == author.Id));
                    foreach (var member in channelAuthor.Users)
                        member.ModifyAsync(x => { x.Mute = !member.IsMuted; });
                }
            });
        }

        [Command(Commands.MuteMaster)]
        [Summary(Summary.MuteMaster)]
        public async Task MuteMaster(string user)
        {
            await Task.Run(() =>
            {

                var author = (SocketGuildUser)Context.Message.Author;
                var channelAuthor = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Users.Any(u => u.Id == author.Id));
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == CustomRoles.MuteMaster);
                if (string.IsNullOrEmpty(user))
                {
                    if (channelAuthor.Users.All(x => !x.Roles.Any(r => r.Name == CustomRoles.MuteMaster)))
                        AddMuteMaster(author, channelAuthor, role);
                }
                else
                {
                    if (author.Roles.Any(x => x.Name == CustomRoles.MuteMaster))
                    {
                        var muteMasterDesignated = channelAuthor.Users.FirstOrDefault(u => u.Id == DiscordHelper.UnMention(user));
                        author.RemoveRoleAsync(role);
                        AddMuteMaster(muteMasterDesignated, channelAuthor, role);
                    }
                }
            });
        }
        [Command(Commands.MuteMaster)]
        [Summary(Summary.MuteMaster)]
        public async Task MuteMaster()
        {
            await Task.Run(() =>
            {

                var author = (SocketGuildUser)Context.Message.Author;
                var channelAuthor = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Users.Any(u => u.Id == author.Id));
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == CustomRoles.MuteMaster);

                if (channelAuthor.Users.All(x => !x.Roles.Any(r => r.Name == CustomRoles.MuteMaster)))
                    AddMuteMaster(author, channelAuthor, role);

            });
        }

        private void AddMuteMaster(SocketGuildUser author, SocketVoiceChannel channelAuthor, SocketRole role)
        {
            author.AddRoleAsync(role);
            Context.Channel.SendMessageAsync($"{author.Mention} é o Mute Master da sala {channelAuthor.Name}");
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

        [Command(Commands.LuckyNumbers)]
        [Summary(Summary.LuckyNumbers)]
        public async Task LuckyNumbers(string maxNumbers = "6", string numbersLimit = "60")
        {
            try
            {
                var i = 0;
                var rnd = new Random();
                var arrNumbers = new List<int>();
                while (i < int.Parse(maxNumbers))
                {
                    var newNumber = rnd.Next(int.Parse(numbersLimit));
                    if (!arrNumbers.Any(x => x == newNumber) && newNumber > 0)
                    {
                        arrNumbers.Add(newNumber);
                        i++;
                    }
                }
                await Context.Channel.SendMessageAsync(string.Format(Messages.LuckyNumbers, string.Join(' ', arrNumbers.OrderBy(x => x))));
            }
            catch
            {
                await Context.Channel.SendMessageAsync(Messages.ErrorLuckyNumber);
            }
        }

        [Command(Commands.LinkRoleRoom)]
        [Summary(Summary.LinkRoleRoom)]
        public async Task LinkRoleRoom(SocketGuildChannel channelMention, [Remainder] string rolesMention)
        {
            try
            {
                var everyone = Context.Guild.Roles.FirstOrDefault(x => x.IsEveryone);
                await channelMention.AddPermissionOverwriteAsync(everyone, new OverwritePermissions
                (connect: PermValue.Deny,
                 viewChannel: PermValue.Deny,
                 readMessageHistory: PermValue.Deny,
                 sendMessages: PermValue.Deny
                 ));

                var rolesIds = rolesMention.Replace("<@&", string.Empty).Replace(">", string.Empty).Split(" ").ToList();
                foreach (var id in rolesIds)
                {
                    var role = Context.Guild.GetRole(ulong.Parse(id));
                    await channelMention.AddPermissionOverwriteAsync(role, new OverwritePermissions
                    (connect: PermValue.Allow,
                    viewChannel: PermValue.Allow,
                    readMessageHistory: PermValue.Allow,
                    sendMessages: PermValue.Allow));
                }

            }
            catch
            {

            }
        }

        [Command(Commands.Copy), RequireUserPermission(GuildPermission.Administrator)]
        [Summary(Summary.Copy)]
        public async Task CopyMesssageChat(SocketGuildChannel copyChannel, SocketGuildChannel pasteChannel, string quantityMsg, string mentionUsers = "false")
        {
            try
            {

                var msgs = await (copyChannel as ISocketMessageChannel).GetMessagesAsync(Int32.Parse(quantityMsg) + 1).FlattenAsync();
                var mentionUser = bool.Parse(mentionUsers);
                foreach (var msg in msgs.OrderBy(x => x.CreatedAt.DateTime))
                {
                    try
                    {
                        var paste = (pasteChannel as ISocketMessageChannel);
                        var mention = mentionUser ? $"{msg.Author.Mention} disse :  \n" : string.Empty;
                        await paste.SendMessageAsync(mention + msg.Content);

                        foreach (var att in msg.Attachments)
                        {
                            var webClient = new WebClient();
                            byte[] imageBytes = webClient.DownloadData(att.Url);
                            Stream stream = new MemoryStream(imageBytes);
                            await paste.SendFileAsync(stream, att.Filename);
                        }



                    }
                    catch { }

                }
            }
            catch
            {

            }
        }

        [Command(Commands.RemoveCategory), RequireUserPermission(GuildPermission.Administrator)]
        [Summary(Summary.RemoveCategory)]
        public async Task RemoveCategory([Remainder] string categories)
        {
            try
            {
                var guild = Context.Guild;
                var listCategories = categories.Split(',');

                foreach (var category in listCategories)
                {
                    var cat = guild.CategoryChannels.FirstOrDefault(x => x.Name.ToUpper() == category.ToUpper());

                    foreach (var room in cat.Channels)
                        await room.DeleteAsync();

                    await cat.DeleteAsync();
                }
            }
            catch
            {

            }
        }

        [Command(Commands.Suggest)]
        [Summary(Summary.Suggest)]
        public async Task Suggest([Remainder] string suggest)
        {
            var guild = Context.Guild;
            var DiscordUser = Context.Client.GetApplicationInfoAsync().Result.Owner;
            await DiscordUser.SendMessageAsync(string.Format(Messages.SuggestMention, Context.User.Mention, suggest));
        }
    }
}