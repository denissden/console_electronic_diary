using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class TeacherEditSubject
    {
        public static void MainScreen(dynamic e)
        {
            dynamic g = e.Element;

            Console.Clear();
            Console.WriteLine(g);
            Console.ReadKey();
        }
    }
}
