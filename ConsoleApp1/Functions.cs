using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Functions
    {
        public static void WriteAt(string s, int x, int y)
        {
            if (x > Console.WindowWidth || y > Console.WindowWidth) return;
            if (x + s.Length > Console.WindowWidth && x <= Console.WindowWidth) s = s.Substring(0, Console.WindowWidth - x);
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
            catch
            {
                // Console.Clear();
                // Console.WriteLine("ERROR in UI.WriteAt");
            }
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

        public static void Exit()
        {
            System.Environment.Exit(0);
        }

        public static void Pass() { }


        public static void WriteTest()
        {
            Console.WriteLine("Test");
        }

    }
}
