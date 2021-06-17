namespace Bot.Common
{
    /// <summary>
    /// Utilities that multiple commands might use.
    /// </summary>
    internal class Utilities
    {
        public bool IsAllCaps(string input)
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
