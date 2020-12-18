using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class TypeGetter
    {
        public string Type { get; set; }

        public TypeGetter()
        {

        }

        public Type GetPersonType()
        {
            switch (Type)
            {
                case "Guest":
                    return typeof(Person);
                case "Student":
                    return typeof(Student);
                case "Teacher":
                    return typeof(Teacher);
                case "Admin":
                    return typeof(Admin);
                default:
                    return null;
            }
        }
    }
}
