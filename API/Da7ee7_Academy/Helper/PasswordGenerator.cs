namespace Da7ee7_Academy.Helper
{
    public static class PasswordGenerator
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Title: this algorithm for generate password
        /// steps
        /// 1: define the type of charchters (L: lowercase, U: uppercase, D: digit, S: symbols)
        /// 2: put on a string one of each charcters type and get 4 of them randomly
        /// 3: reorder the whole string randomly 
        /// 4: set value for example if password[i] = 'D' then it should be replaced with digitg from 0 to 9
        /// </summary>
        /// <returns>string (password)</returns>
        public static string Generate()
        {
            string Initial = "LUDS";
            string password = Initial;
            for (int i = 0; i < 4; i++)
            {
                int randomIndex = random.Next(0, Initial.Length);
                password += Initial[randomIndex];
            }
            password = Reorder(password);

            var passwordValue = "";
            for (int i = 0; i < password.Length; i++)
            {
                switch (password[i])
                {
                    case 'L':
                        passwordValue += char.ToLower(GetLetter());
                        break;
                    case 'U':
                        passwordValue += GetLetter();
                        break;
                    case 'D':
                        passwordValue += GetDigit();
                        break;
                    case 'S':
                        passwordValue += GetSymbols();
                        break;
                }
            }

            return passwordValue;
        }
        private static string Reorder(string password)
        {
            string reorderedPassword = "";
            int length = password.Length;
            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(0, password.Length);
                reorderedPassword += password[randomIndex];
                password = password.Remove(randomIndex, 1);
            }
            return reorderedPassword;
        }
        private static char GetLetter()
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int randomIndex = random.Next(0, letters.Length);
            return letters[randomIndex];
        }
        private static char GetDigit()
        {
            string digits = "0123456789";
            int randomIndex = random.Next(0, digits.Length);
            return digits[randomIndex];
        }
        private static char GetSymbols()
        {
            string symbols = "!@#$%^&*";
            int randomIndex = random.Next(0, symbols.Length);
            return symbols[randomIndex];
        }
    }
}
