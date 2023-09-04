using Discord;
using Discord.Commands;
using RaccoonBot.Domain.Command;
using RaccoonBot.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaccoonBot.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        public CommandService Service { get; set; }

        [Command(Commands.Help)]
        [Summary(Summary.Help)]
        public async Task HelpDefault()
        {
            List<CommandInfo> commands = Service.Commands.Where(x => CriticalHelp.CommandsSummary.Any(cs => cs == x.Name)).ToList();
            await CreateEmbedTextHelpModulesCommands(commands);
        }

        [Command(Commands.HelpAdmin)]
        [Summary(Summary.HelpAdmin)]
        public async Task HelpAdmin()
        {
            List<CommandInfo> commands = Service.Commands.Where(x => CriticalHelp.CommandsSummaryAdmin.Any(cs => cs.ToLower() == x.Name.ToLower())).ToList();
            await CreateEmbedTextHelpModulesCommands(commands);
        }

        private async Task CreateEmbedTextHelpModulesCommands(List<CommandInfo> commands)
        {
            try
            {
                EmbedBuilder embedBuilder = new EmbedBuilder();
                foreach (var c in commands)
                {
                    string embedFieldText = !string.IsNullOrEmpty(c.Summary) ? c.Summary : "No description available\n";
                    embedBuilder.AddField(Commands.prefix + c.Name, embedFieldText);
                }
                await Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}