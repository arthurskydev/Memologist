namespace Bot.Client.Utilities
{
    /// <summary>
    /// Utilities that multiple commands might use.
    /// </summary>
    public class MiscUtilities
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
