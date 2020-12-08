using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Globalization;

namespace ConsoleApp1
{
    class Functions
    {

        // CONSOLE
        public static bool WriteAt(string s, int x, int y)
        {
            if (x > Console.WindowWidth || y > Console.WindowWidth) return false;
            if (x + s.Length > Console.WindowWidth && x <= Console.WindowWidth) s = s.Substring(0, Console.WindowWidth - x);
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
                return true;
            }
            catch
            {
                // Console.Clear();
                // Console.WriteLine("ERROR in UI.WriteAt");
            }
            return false;
        }

        public static void SetColor(int i, bool reverse = false)
        {
            if (reverse)
            {
                Console.ForegroundColor = Constants.Colors[i, 1];
                Console.BackgroundColor = Constants.Colors[i, 0];
            }
            else
            {
                Console.ForegroundColor = Constants.Colors[i, 0];
                Console.BackgroundColor = Constants.Colors[i, 1];
            }
        }

        // HASH
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }


        // BASIC
        public static void Exit(object _)
        {
            System.Environment.Exit(0);
        }

        public static void Pass(object _) { }


        public static void WriteTest(object _)
        {
            Console.WriteLine("Test");
        }

        public static DateTime ToDatetime(string s)
        {
            DateTime ret;
            DateTime.TryParseExact(s, Constants.DatePresets, CultureInfo.InvariantCulture, DateTimeStyles.None, out ret);
            return ret;
        }

        public static int ClipInt(int input, int a, int b)
        {
            if (input < a) return a;
            else if (input > b) return b;
            else return input;
        }

    }
}
