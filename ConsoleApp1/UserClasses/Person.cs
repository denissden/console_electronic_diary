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
        // USED FOR ALL USER TYPES
        public string Type { get; set; }
        public DateTime DateCreated { get; set; }
        public ulong Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Group { get; set; }
        public string OldGroup = ""; 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime BirthYear { get; set; }
        public List<string> ToStringOptions = new List<string>() { "Login" };

        // USED FOR STUDENT
        public List<MarkList> SubjectMarks { get; set; }

        // USED FOR TEACHER
        public List<Subject> Subjects { get; set; }

        public Person()
        {
            Type = "Guest";
            LastName = "";
            FirstName = "";
            MiddleName = "";
            Age = -1;     
            PasswordHash = Functions.GetHashString("");
            DateCreated = DateTime.Now;
            SubjectMarks = new List<MarkList>();
            Subjects = new List<Subject>();
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
                res += MiddleName[0].ToString().ToUpper() + '.';
            return res;
        }

        public int GetAge()
        {
            if (BirthYear == null) return -1;
            DateTime today = DateTime.Today;
            int age = today.Year - BirthYear.Year;
            if (BirthYear.Date > today.AddYears(-age)) age--;
            Age = age;
            return age;
        }

        public void SetToStringOptions(string s)
        {
            s = s.ToLower();
            ToStringOptions = s.Split(new char[] { ',', '.', '-', '/' }).OfType<string>().ToList();
        }

        public override string ToString()
        {
            string result = "";
            foreach (string s in ToStringOptions)
            {
                string property = s;
                string add = $"{s}: ";
                if (property.Length > 0 && property[0] == '_')
                {
                    add = "";
                    property = property.Substring(1, property.Length - 1);
                }
                result += add;
                switch (property) 
                {
                    case "login":
                        result += Login;
                        break;
                    case "id":
                        result += Id;
                        break;
                    case "name":
                        result += GetShortName();
                        break;
                    case "date":
                        result += BirthYear.ToString("dd.MM.yyyy");
                        break;
                    case "age":
                        result += GetAge();
                        break;
                    case "group":
                        result += Group;
                        break;
                    case "type":
                        result += Type;
                        break;
                }
                result += "  ";
            }
            return result;
        }

    }

}
