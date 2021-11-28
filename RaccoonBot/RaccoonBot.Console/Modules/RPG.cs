using Discord.Commands;
using RaccoonBot.Domain.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RaccoonBot.Modules
{
    public class RPG : ModuleBase<SocketCommandContext>
    {
        [Command("r")]
        [Summary("")]
        public async Task Roll(string roll)
        {
            await Task.Run(() =>
            {
                try
                {
                    var dice = roll.Split("d");
                    var qt = !string.IsNullOrWhiteSpace(dice[0]) ? int.Parse(dice[0]) : 1;
                    var plus = dice[1].Split("+");
                    var diceNumber = int.Parse(plus[0]);
                    var plusNumber = plus.Length > 1 ? int.Parse(plus[1]) : 0;
                    List<string> listNumber = new List<string>();
                    var msgPlus = string.Empty;
                    var random = new Random();
                    var result = 0;
                    for (var i = 0; i < qt; i++)
                    {
                        var numberRandom = random.Next(1, diceNumber);
                        listNumber.Add(numberRandom.ToString());
                        result += numberRandom;
                    }
                    if (plusNumber > 0)
                    {
                        result += plusNumber;
                        msgPlus = !string.IsNullOrWhiteSpace(plus[1]) ? $" + {plus[1]}" : string.Empty;

                    }
                    var msg = (listNumber.Count > 1 || !string.IsNullOrWhiteSpace(msgPlus)) ?
                    $" {string.Join(" + ", listNumber) }{ msgPlus}   = {result}" : listNumber.FirstOrDefault();
                    ReplyAsync(msg);
                }
                catch
                {

                }

            });
        }

        /// <summary>Our Random object.  Make it a first-class citizen so that it produces truly *random* results</summary>
        Random r = new Random();

        /// <summary>Roll</summary>
        /// <param name="s">string to be evaluated</param>
        /// <returns>result of evaluated string</returns>
        public int R(string s)
        {
            int t = 0;

            // Addition is lowest order of precedence
            var a = s.Split('+');

            // Add results of each group
            if (a.Count() > 1)
                foreach (var b in a)
                    t += R(b);
            else
            {
                // Multiplication is next order of precedence
                var m = a[0].Split('*');

                // Multiply results of each group
                if (m.Count() > 1)
                {
                    t = 1; // So that we don't zero-out our results...

                    foreach (var n in m)
                        t *= R(n);
                }
                else
                {
                    // Die definition is our highest order of precedence
                    var d = m[0].Split('d');

                    // This operand will be our die count, static digits, or else something we don't understand
                    if (!int.TryParse(d[0].Trim(), out t))
                        t = 0;

                    int f;

                    // Multiple definitions ("2d6d8") iterate through left-to-right: (2d6)d8
                    for (int i = 1; i < d.Count(); i++)
                    {
                        // If we don't have a right side (face count), assume 6
                        if (!int.TryParse(d[i].Trim(), out f))
                            f = 6;

                        int u = 0;

                        // If we don't have a die count, use 1
                        for (int j = 0; j < (t == 0 ? 1 : t); j++)
                            u += r.Next(1, f);

                        t += u;
                    }
                }
            }

            return t;
        }
        //[Command(Commands.MuteChannel)]
        //[Summary(Summary.MuteChannel)]
        //public async Task MuteUnmuteEveryoneInChat()
        //{
        //    await Task.Run(() =>
        //    {
        //        var author = (SocketGuildUser)Context.Message.Author;

        //        if (author.Roles.Any(x => x.Name == CustomRoles.MuteMaster))
        //        {
        //            var channelAuthor = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Users.Any(u => u.Id == author.Id));
        //            foreach (var member in channelAuthor.Users)
        //                member.ModifyAsync(x => { x.Mute = !member.IsMuted; });
        //        }
        //    });
        //}

        //[Command(Commands.MuteMaster)]
        //[Summary(Summary.MuteMaster)]
        //public async Task MuteMaster(string user)
        //{
        //    await Task.Run(() =>
        //    {

        //        var author = (SocketGuildUser)Context.Message.Author;
        //        var channelAuthor = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Users.Any(u => u.Id == author.Id));
        //        var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == CustomRoles.MuteMaster);
        //        if (string.IsNullOrEmpty(user))
        //        {
        //            if (channelAuthor.Users.All(x => !x.Roles.Any(r => r.Name == CustomRoles.MuteMaster)))
        //                AddMuteMaster(author, channelAuthor, role);
        //        }
        //        else
        //        {
        //            if (author.Roles.Any(x => x.Name == CustomRoles.MuteMaster))
        //            {
        //                var muteMasterDesignated = channelAuthor.Users.FirstOrDefault(u => u.Id == DiscordHelper.UnMention(user));
        //                author.RemoveRoleAsync(role);
        //                AddMuteMaster(muteMasterDesignated, channelAuthor, role);
        //            }
        //        }
        //    });
        //}
        //[Command(Commands.MuteMaster)]
        //[Summary(Summary.MuteMaster)]
        //public async Task MuteMaster()
        //{
        //    await Task.Run(() =>
        //    {

        //        var author = (SocketGuildUser)Context.Message.Author;
        //        var channelAuthor = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Users.Any(u => u.Id == author.Id));
        //        var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == CustomRoles.MuteMaster);

        //        if (channelAuthor.Users.All(x => !x.Roles.Any(r => r.Name == CustomRoles.MuteMaster)))
        //            AddMuteMaster(author, channelAuthor, role);

        //    });
        //}

        //private void AddMuteMaster(SocketGuildUser author, SocketVoiceChannel channelAuthor, SocketRole role)
        //{
        //    author.AddRoleAsync(role);
        //    Context.Channel.SendMessageAsync($"{author.Mention} é o Mute Master da sala {channelAuthor.Name}");
        //}

        //[Command(Commands.LinkRoleRoom)]
        //[Summary(Summary.LinkRoleRoom)]
        //public async Task LinkRoleRoom(SocketGuildChannel channelMention, [Remainder] string rolesMention)
        //{
        //    try
        //    {
        //        var everyone = Context.Guild.Roles.FirstOrDefault(x => x.IsEveryone);
        //        await channelMention.AddPermissionOverwriteAsync(everyone, new OverwritePermissions
        //        (connect: PermValue.Deny,
        //         viewChannel: PermValue.Deny,
        //         readMessageHistory: PermValue.Deny,
        //         sendMessages: PermValue.Deny
        //         ));

        //        var rolesIds = rolesMention.Replace("<@&", string.Empty).Replace(">", string.Empty).Split(" ").ToList();
        //        foreach (var id in rolesIds)
        //        {
        //            var role = Context.Guild.GetRole(ulong.Parse(id));
        //            await channelMention.AddPermissionOverwriteAsync(role, new OverwritePermissions
        //            (connect: PermValue.Allow,
        //            viewChannel: PermValue.Allow,
        //            readMessageHistory: PermValue.Allow,
        //            sendMessages: PermValue.Allow));
        //        }

        //    }
        //    catch
        //    {

        //    }
        //}

    }
}
