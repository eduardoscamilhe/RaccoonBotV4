using RaccoonBot.Domain.Constants;
using System.Text.RegularExpressions;

namespace RaccoonBot.Domain.Utils
{
    public class DiscordHelper
    {
        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        public static string OnlyLetters(string name) =>
            Regex.Replace(name, CustomRegex.OnlyLetters, string.Empty, RegexOptions.Compiled);
        public static ulong GetIdFromMention(string name) => ulong.Parse(Regex.Replace(name, CustomRegex.UnMention, string.Empty, RegexOptions.Compiled));

        public static string Mention(ulong id) => $"<@!{id}>";
        public static ulong UnMentionRole(string role) => ulong.Parse(role.Replace("<@&", string.Empty).Replace(">", string.Empty));

        public static ulong UnMention(string unmention) => ulong.Parse(unmention.Replace("<@!", string.Empty).Replace(">", string.Empty));

    }
}
