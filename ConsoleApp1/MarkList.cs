using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public struct MarkList
    {
        public string SubjectName { get; set; }
        public ulong CreatorId { get; set; }
        public Dictionary<DateTime, Mark> Marks { get; set; }

        public MarkList(string name, ulong id)
        {
            SubjectName = name;
            CreatorId = id;
            Marks = new Dictionary<DateTime, Mark>();
        }

        public void SetMark(DateTime date, string mark)
        {
            DateTime d = date.Date;
            if (mark == " " || mark == "")
            {
                Marks.Remove(d);
                return;
            }
            Mark m = new Mark(mark, d);
            if (Marks.ContainsKey(d))
                Marks[d] = m;
            else
                Marks.Add(d, m);
        }

        public override string ToString()
        {
            return SubjectName;
        }
    }

    public struct Mark
    {
        public string Value { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }

        public Mark(string value, DateTime date)
        {
            Value = value;
            Date = date.Date;
            DateCreated = DateTime.Now;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public struct SimplifiedMark
    {
        public string Value { get; set; }
        public DateTime Date { get; set; }
        public SimplifiedMark(string value, DateTime date)
        {
            Value = value;
            Date = date.Date;
        }

        public override string ToString()
        {
            return Date.ToString("dd.MM.yyyy");
        }

    }
}
