using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        public static bool Shorter20(string s = "") { return s.Length <= 20; }
        public static bool Shorter10(string s = "") { return s.Length <= 10; }

        // date
        public static bool Date(string s = "")
        {
            return DateTime.TryParseExact(s, Constants.DatePresets, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        // login
        public static bool NewLogin(string s = "") 
        {
            bool valid = true;
            valid = valid && !File.Exists(Constants.DB_Path + Constants.IDS_Path + $"{s}.dat");
            valid = valid && s.Length <= 30;
            valid = valid && s.Length >= 3;

            return valid; 
        }

        public static bool LoginExists(string s = "")
        {
            return File.Exists(Constants.DB_Path + Constants.IDS_Path + $"{s}.dat");
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

        // person
        public static bool PersonHasProperty(string s)
        {
            return typeof(Person).GetProperty(s) != null;
        }

        public static bool PersonCanDisplay(string s)
        {
            s = s.ToLower();
            string[] ToStringOptions = s.Split(new char[] { ',', '.', '-', '/' });
            string[] accepted = new string[] {
                "login",
                "id",
                "name",
                "date",
                "age",
                "group",
                "type", };
            bool ret = true;
            foreach (string option in ToStringOptions)
            {
                ret = ret && (accepted.Contains(option) || accepted.Contains(option.Replace("_", "")));
            }
            return ret;
        }

        // group
        public static bool NewGroup(string s)
        {
            return !File.Exists(Constants.DB_Path + Constants.GROUPS_Path + $"{s}.json");
        } 
        
        public static bool GroupExists(string s)
        {
            return File.Exists(Constants.DB_Path + Constants.GROUPS_Path + $"{s}.json");
        }
    }
}
