namespace RaccoonBot.Domain.Constants
{
    public static class CustomRegex
    {
        public const string RegexUrl = @"((http(s)?(\:\/\/))+(www\.)?([\w\-\.\/])*(\.[a-zA-Z]{2,3}\/?))[^\s\b\n|]*[^.,;:\?\!\@\^\$ -]";
        public const string OnlyLetters = @"^[a-zA-Z0-9_]*$";
        public const string UnMention = @"[@!<>]";
    }
}
