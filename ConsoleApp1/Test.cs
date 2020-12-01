using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Test
    {
        public static void Run()
        {
            Stack<dynamic> list = new Stack<dynamic>();
            list.Push(1);
            list.Push(1);
            list.Push("2");
            list.Push(new Person() { LastName = "HI" });
            for (int i = -1; i <= list.Count; i++)
                Console.WriteLine(list.Pop());
            Console.WriteLine(list.Count + " count");
        }
    }
}
