using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Validation
    {
        private static string TmpString = "";
        // default
        public static bool True(string s = "") { return true; }
        public static bool False(string s = "") { return false; }

        // longer
        public static bool Longer8(string s = "") { return s.Length >= 8; }
        public static bool Longer3(string s = "") { return s.Length >= 3; }
        public static bool NonEmpty(string s = "") { return s != ""; }

        // shorter
        public static bool Shorter30(string s = "") { return s.Length <= 30; }
        public static bool Shorter10(string s = "") { return s.Length <= 10; }

        // date
        public static bool Date(string s = "")
        {
            return DateTime.TryParseExact(s, Constants.DatePresets, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

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

        public static bool PasswordsMatchAdd(string s = "")
        {
            TmpString = s;
            return true;
        }

        public static bool PasswordsMatchCheck(string s)
        {
            if (s.GetType() == typeof(string))
                if (String.Equals(s, TmpString)) 
                    return true;
            return false;
        }


    }
}
