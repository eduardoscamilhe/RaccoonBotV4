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
    public class Fun : ModuleBase<SocketCommandContext>
    {
        [Command(Commands.Say), RequireUserPermission(GuildPermission.ManageMessages)]
        [Summary("")]
        public async Task Say([Remainder] string say)
        {
            var channel = (Context.Channel as SocketTextChannel);
            await channel.DeleteMessageAsync(Context.Message);
            await Context.Channel.SendMessageAsync(say);
        }

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
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync(Messages.ErrorLuckyNumber);
                Console.WriteLine(ex.Message);
            }
        }
    }
}