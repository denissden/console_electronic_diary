using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Test
    {
        public static void Run()
        {
            List<object> list = new List<object>();
            list.Add(1);
            list.Add("2");
            list.Add(new Person() { FamilyName = "HI" });
            foreach (object o in list) Console.WriteLine(o);
        }
    }
}
