namespace RaccoonBot
{
    public class Settings
    {
        public static readonly Settings Instance = new Settings();

        public string BotToken = "";
        public bool RoleGame = false;
        public ulong RaccoonlandId = 220277571798040576;
        public short PurgeCount = 10;
        public ulong RaccoonPlayerId = 205074810810793984;
        public string Prefix = "r.";
    }
}