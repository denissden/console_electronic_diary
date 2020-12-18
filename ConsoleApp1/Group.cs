using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Group
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public List<ulong> People { get; set; }
        
        public Group(string name)
        {
            Name = name;
            People = new List<ulong>();
            Status = "Unchanged";
        }

        public Group()
        {
            People = new List<ulong>();
            Status = "Unchanged";
        }

        public override string ToString()
        {
            string res = Name;
            res += ": ";
            int c = Name.Length;
            foreach (ulong g in People)
            {
                string s = g.ToString();
                c += s.Length;
                if (c <= Constants.GroupDisplayInListViewMaxLength)
                {
                    res += s;
                    c += s.Length + 2;
                }
                else
                {
                    res += "..";
                    break;
                }
                res += ", ";
            }
            return res;
        }
    }
}
