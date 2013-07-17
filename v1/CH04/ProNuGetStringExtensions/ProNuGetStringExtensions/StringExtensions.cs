namespace System
{
    public static class StringExtensions
    {
        public static string Truncate(this string input, int length)
        {
            return Truncate(input, length, "...");
        }

        public static string Truncate(this string input, int length, string suffix)
        {
            if (input == null)
            {
                return "";
            }
            if (input.Length <= length)
            {
                return input;
            }

            if (suffix == null)
            {
                suffix = "...";
            }

            return input.Substring(0, length - suffix.Length) + suffix;
        }
    }
}
