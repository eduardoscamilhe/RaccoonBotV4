using RaccoonBot.Domain.Command;

namespace RaccoonBot.Domain.Constants
{
    public static class CriticalHelp
    {

        public static readonly string[] CommandsSummary = {
            Commands.Ping,
            Commands.InviteBot,
            Commands.InviteDiscord,
            Commands.InviteOriginDiscord,
            Commands.LuckyNumbers,
            Commands.Suggest,
            Commands.BlackHumor,
            Commands.NSFW,
            Commands.IPlay,
            Commands.Help,
            Commands.HelpAdmin
         };
        public static readonly string[] CommandsSummaryAdmin = {
             Commands.Clean,
             Commands.SetRoleAll,
             Commands.DelRoleAll,
             Commands.Copy,
         };
    }
}
