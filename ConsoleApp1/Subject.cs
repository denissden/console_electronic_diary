using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public struct Subject
    {
        public List<string> Groups { get; set; }
        public string Name { get; set; }

        public Subject(string name, List<string> groups)
        {
            Name = name;
            Groups = groups;
        }

        public override string ToString()
        {
            string res = Name;
            res += ": ";
            foreach (string g in Groups)
                res += g + " ";
            return res;
        }
    }
}
