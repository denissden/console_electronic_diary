using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Teacher : Person
    {
        public List<string> Groups { get; set; }
        public Teacher()
        {
            Groups = new List<string>();
            Type = "Teacher";
        }
    }
}
