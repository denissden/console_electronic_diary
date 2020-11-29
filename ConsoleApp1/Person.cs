using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ConsoleApp1
{
    class PersonFunctions
    {
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }


    public class Person
    {
        public string Type = "Guest";
        public int Id { get; set; }
        public string PasswordHash { get; set; }
        public string Group { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string FamilyName { get; set; }
        public int Age { get; set; }
        public int BirthYear { get; set; }


        public Person()
        {
            FamilyName = "";
            FirstName = "";
            MiddleName = "";
            Age = -1;
            BirthYear = -1;
            PasswordHash = PersonFunctions.GetHashString("");
        }

        public bool SetPassword(string _new, string old = "")
        {
            string hash_new = PersonFunctions.GetHashString(_new);
            string hash_old = PersonFunctions.GetHashString(old);
            if (String.Equals(hash_new, hash_old))
            {
                PasswordHash = hash_new;
                return true;
            }
            return false;

        }

        public string GetFullName()
        {
            if (MiddleName != "") return $"{FamilyName} {FirstName} {MiddleName}";
            else return $"{FamilyName} {FirstName}";
        }

        public override string ToString()
        {
            string result = "";
            result += "ФИО: " + GetFullName();
            result += $", Год рождения: {BirthYear}, Возраст: {Age}";
            return result;
        }

    }

    public class Student : Person
    {
        public Dictionary<string, List<(int, DateTime)>> SubjectMarks { get; set; }

        public Student()
        {
            SubjectMarks = new Dictionary<string, List<(int, DateTime)>>();
            Type = "Student";
        }

        public bool AddMark(string subject, int mark) // Добавить оценку к предмету
        {
            if (SubjectMarks.ContainsKey(subject))
            {
                SubjectMarks[subject].Add((mark, DateTime.Now));
                return true;
            }
            else return false;
        }

        public bool AddMarks(string subject, int[] marks) // Добавить несколько оценок к предмету
        {
            if (SubjectMarks.ContainsKey(subject))
            {
                foreach (int m in marks) AddMark(subject, m);
                return true;
            }
            else return false;
        }

        public bool SetMark(string subject, int index, int mark) // Заменить существующую оценку 
        {
            if (SubjectMarks.ContainsKey(subject) && index < SubjectMarks[subject].Count)
            {
                SubjectMarks[subject][index] = (mark, DateTime.Now);
                return true;
            }
            else return false;
        }

        public bool InsertMark(string subject, int index, int mark) // Заменить существующую оценку 
        {
            if (SubjectMarks.ContainsKey(subject))
            {
                if (index < SubjectMarks[subject].Count) SubjectMarks[subject][index] = (mark, DateTime.Now);
                else SubjectMarks[subject].Add((mark, DateTime.Now));
                return true;
            }
            else return false;
        }

        public bool DeleteMark(string subject, int index) // Заменить существующую оценку 
        {
            if (SubjectMarks.ContainsKey(subject) && index < SubjectMarks[subject].Count)
            {
                SubjectMarks[subject].RemoveAt(index);
                return true;
            }
            else return false;
        }


    }



}
