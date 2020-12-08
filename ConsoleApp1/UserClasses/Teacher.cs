using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Teacher : Person
    {
        public List<string> HasGroups { get; set; }
        public Teacher()
        {
            HasGroups = new List<string>();
            Type = "Teacher";
        }

    }
}
