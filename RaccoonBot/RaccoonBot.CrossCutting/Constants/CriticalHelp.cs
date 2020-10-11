using RaccoonBot.Domain.Command;

namespace RaccoonBot.Domain.Constants
{
    public static class CriticalHelp
    {

        public static readonly string[] CommandsSummary = {
            Commands.Clean,
            Commands.Ping,
            Commands.InviteBot,
            Commands.InviteDiscord,
            Commands.InviteOriginDiscord,
            Commands.LuckyNumbers,
            Commands.LinkRoleRoom,
            Commands.Suggest,
            Commands.BlackHumor,
            Commands.NSFW,
            Commands.Help,
            Commands.HelpAdmin,
            Commands.HelpMhw,
            Commands.CreateUserChannel,
            Commands.UserAddFriend,
            Commands.UserAddRoom,
            Commands.UserRemoveFriend,
            Commands.UserRemoveRoom,
         };
        public static readonly string[] CommandsSummaryAdmin = {
             Commands.SetRoleAll,
             Commands.DelRoleAll,
             Commands.Copy
         };
        public static readonly string[] CommandsSummaryMhw = {
            Commands.Events,
            Commands.Decoration
         };

        public static readonly string[] CommandsSummaryUserChannel = {
            Commands.CreateUserChannel,
            Commands.UserAddFriend,
            Commands.UserAddRoom,
            Commands.UserRemoveFriend,
            Commands.UserRemoveRoom,
         };

    }
}
