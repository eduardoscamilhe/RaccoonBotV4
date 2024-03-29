﻿namespace RaccoonBot.Domain.Command
{
    public static class Commands
    {
        #region Other
        public const string Suggest = "suggest";

        #endregion

        #region Manage
        public const string prefix = "r.";
        public const string Clean = "clean";
        public const string InviteBot = "join";
        public const string InviteDiscord = "invite";

        public const string InviteOriginDiscord = "origin";

        public const string Help = "help";
        public const string HelpAdmin = "admin";
        public const string HelpMhw = "mhw";
        public const string HelpUserChannel = "userChannel";

        public const string LuckyNumbers = "lucky";
        public const string LinkRoleRoom = "linkRoom";
        public const string Copy = "copy";
        public const string Ping = "ping";
        public const string PlayingCount = "playing";
        public const string MuteChannel = "muteall";
        public const string MuteMaster = "mutemaster";

        public const string RemoveCategory = "removeCategory";

        #endregion

        #region Roles

        public const string SetRoleAll = "setroles";
        public const string DelRoleAll = "delroles";
        public const string NSFW = "nsfw";
        public const string BlackHumor = "black";
        public const string SetRole = "set";
        public const string RemoveRole = "remove";


        public const string ReorderRoleGaming = "rrg";
        public const string ReorderBotRoles = "rbr";
        public const string PurgeRoles = "pr";


        public const string UpdateGameRoles = "uproles";

        public const string SetMentionable = "setmention";

        public const string SetHoistable = "sethoist";
        public const string CountRoles = "countroles";
        public const string DeleteRepeatableRoles = "drr";

        public const string ApplyRoleGaming = "applyRoleGaming";
        public const string BotContact = "bot";

        #endregion

        #region MHW
        public const string MHWCreate = "createMHW";
        public const string MHWLinkRoleRoom = "linkWeapon";
        public const string MHWRemove = "removeMHW";
        public const string MHWCreateRole = "createWeapon";
        public const string AllWeapons = "allWeapon";
        public const string GetWeapons = "weapon";
        public const string Events = "events";
        public const string EventsAdmin = "eventsAdmin";
        public const string Decoration = "jewel";
        public const string Material = "drop";

        public const string Weakness = "weak";

        #endregion

        #region Secret
        public const string Cheat = "cheat";

        #endregion

        #region UserEnvironment
        public const string CreateUserChannel = "createChannel";
        public const string UserAddFriend = "addFriend";
        public const string UserAddRoom = "addRoom";
        public const string UserRemoveFriend = "removeFriend";
        public const string UserRemoveRoom = "removeRoom";
        #endregion
    }
}
