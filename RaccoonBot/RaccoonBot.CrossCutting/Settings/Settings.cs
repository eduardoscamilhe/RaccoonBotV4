﻿namespace RaccoonBot.Domain.Settings
{
    public class Settings
    {
        public static readonly Settings Instance = new Settings();

        public string BotToken = "MjY4ODE5MTgwODEwMjcyNzcw.WHaDXw.zbFNrjcTL6QPc8uNLb-vgeJESk0";
        public bool RoleGame = false;
        public ulong RaccoonId = 220277571798040576;
        public string OriginDiscord = "https://discord.gg/FKdqcPX";
        public short BotCount = 6;
        public short PurgeCount = 10;
        public ulong RaccoonPlayerId = 205074810810793984;
        public string NotPurgeRoles = "NSFW;Black Humor;RPG,Memes";
        public string Prefix = "r.";

        public ulong ColorMessageId = 764859454675877888;
    }
}