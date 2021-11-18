using System;
using System.Configuration;
using System.Linq;

namespace t4
{
    static class CeasarCipher
    {
        public static string Encipher(string input, int offset) =>
            input.Aggregate(string.Empty, (current, ch) => current + Cipher(ch, offset));

        public static string Decipher(string input, int key) =>
            Encipher(input, 26 - key);

        private static char Cipher(char ch, int offset)
        {
            if (!char.IsLetter(ch)) return ch;

            var d = char.IsUpper(ch) ? 'A' : 'a';
            return (char) ((((ch + offset) - d) % 26) + d);
        }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var offset = int.Parse(ConfigurationManager.AppSettings["offset"] ?? "2");

                const string str = "Testujemy!";
                var cipherText = CeasarCipher.Encipher(str, offset);
                Console.WriteLine("Encrypted Data:");
                Console.WriteLine(cipherText);

                Console.WriteLine("\nDecrypted Data:");
                var t = CeasarCipher.Decipher(cipherText, offset);
                Console.WriteLine(t);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }
    }
}