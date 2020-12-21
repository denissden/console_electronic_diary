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

        public Subject(string name)
        {
            Name = name;
            Groups = new List<string>();
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

    public class SubjectContainer
    {
        public Subject S { get; set; }
        public ulong CreatorId { get; set; }

        public SubjectContainer(Subject s, ulong id)
        {
            S = s;
            CreatorId = id;
        }

        public override string ToString()
        {
            return S.ToString();
        }
    }

    public class GroupContainer
    {
        public string Name { get; set; }
        public ulong CreatorId { get; set; }
        public string SubjectName { get; set; }

        public GroupContainer(string name, string s_name, ulong id)
        {
            Name = name;
            SubjectName = s_name;
            CreatorId = id;
        }

        public override string ToString()
        {
            return Name;
        }
    } 
}
