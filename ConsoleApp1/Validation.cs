using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Validation
    {
        // default
        public static bool True(string s = "") { return true; }
        public static bool False(string s = "") { return false; }

        // longer
        public static bool Longer8(string s = "") { return s.Length >= 8; }
        public static bool Shorter30(string s = "") { return s.Length <= 30; }

        // login
        public static bool Login(string s = "") 
        { 
            return s.Length <= 30; 
        }

        // password 
        public static bool Password(string s = "") 
        { 
            return s.Length <= 30; 
        }
    }
}
