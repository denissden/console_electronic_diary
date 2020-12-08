using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
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
