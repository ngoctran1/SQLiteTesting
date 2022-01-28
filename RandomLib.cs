using System.Text;

namespace SQLiteTesting {
    public static class RandomLib {
        public static string RandomName(int maxSize)
        {
            if (maxSize <= 0) throw new ArgumentOutOfRangeException("Must be larger than 0");

            var stringBuilder = new StringBuilder();
            var rand = new Random();

            stringBuilder.Append(RandomLetter()); // Ensure the string is not empty
            for (var charNumber = 0; charNumber < maxSize - 1; charNumber++)
            {
                if (rand.Next() % 2 == 0) continue; // Sometimes skip letters

                stringBuilder.Append(RandomLetter());
            }

            return stringBuilder.ToString();
        }

        // Letter can be upper or lower case
        public static char RandomLetter()
        {
            var rand = new Random();

            var isCapital = rand.Next() % 2 == 0;
            var charAsciiValue = rand.Next() % 26;

            // 65 is the ASCII value for upper case 'a' whilst 97 (65 + 32) is the value for lower case 'a'
            return (char)(charAsciiValue + 65 + (isCapital ? 0 : 32));
        }
    }
}