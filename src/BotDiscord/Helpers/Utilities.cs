namespace Bot.Client.Helpers
{
    /// <summary>
    /// Utilities that multiple commands might use.
    /// </summary>
    public class Utilities
    {
        public static bool IsAllCaps(string input)
        {
            foreach (char character in input)
            {
                if (char.IsLower(character))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
