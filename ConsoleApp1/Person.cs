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
        public string Type = "Guest";
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
