using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Input
    {
        public static int InputInt()
        {
            int res;
            try
            {
                res = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Введите ЦЕЛОЕ ЧИСЛО");
                res = InputInt();
            }
            return res;
        }

        public static double InputDouble()
        {
            double res;
            try
            {
                res = Convert.ToDouble(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Введите ЧИСЛО");
                res = InputDouble();
            }
            return res;
        }

        public static char InputChar()
        {
            char res;
            try
            {
                res = Convert.ToChar(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Введите СИМВОЛ");
                res = InputChar();
            }
            return res;
        }
    }
}
