using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace ConsoleApp1
{
 
    public class Person
    {
        public string Type { get; set; }
        public DateTime DateCreated { get; set; }
        public ulong Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Group { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime BirthYear { get; set; }


        public Person()
        {
            Type = "Guest";
            LastName = "";
            FirstName = "";
            MiddleName = "";
            Age = -1;
            PasswordHash = Functions.GetHashString("");
            DateCreated = DateTime.Now;
        }

        public bool SetPassword(string _new, string old = "")
        {
            string hash_new = Functions.GetHashString(_new);
            string hash_old = Functions.GetHashString(old);
            if (String.Equals(PasswordHash, hash_old))
            {
                PasswordHash = hash_new;
                return true;
            }
            return false;
        }

        public string GetFullName()
        {
            if (MiddleName != "") return $"{LastName} {FirstName} {MiddleName}";
            else return $"{LastName} {FirstName}";
        }

        public string GetShortName()
        {
            string res = "";
            if (LastName != "")
                res += LastName + " ";
            if (FirstName != "")
                res += FirstName[0].ToString().ToUpper() + '.';
            if (MiddleName != "")
                res += FirstName[0].ToString().ToUpper() + '.';
            return res;
        }

        public override string ToString()
        {
            string result = "Login: ";
            result += Login;
            result += "  Name: ";
            result += GetShortName();
            result += "  Born ";
            result += BirthYear.ToString("dd.MM.yyyy");
            return result;
        }

    }

}
