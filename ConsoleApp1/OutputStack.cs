using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class OutputStack
    {
        private static List<dynamic> Items = new List<dynamic>();

        public static void Push(dynamic element)
        {
            Items.Add(element);
        }

        public static dynamic Pop()
        {
            int i = Items.Count - 1;
            if (i != -1)
            {
                dynamic ret = Items[i];
                Items.RemoveAt(i);
                return ret;
            }
            return null;
        }

        public static dynamic Peek(int d = 0)
        {
            int i = Items.Count - 1 - d;
            if (i >= 0)
            {
                dynamic ret = Items[i];
                return ret;
            }
            return null;
        }

    }
}
