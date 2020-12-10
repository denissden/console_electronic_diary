using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class GenerateUsers
    {
        public static void Generate(dynamic _)
        {
            string AcceptedCharacters = @"^[A-Za-z0-9А-Яа-я_]+$";
            Regex reg = new Regex(AcceptedCharacters);

            string[] lines = System.IO.File.ReadAllLines(Constants.DB_Path + "LeakedUsernames.txt");

            foreach (string line in lines)
            {
                string cut_line = line.Split(new char[] { ':', '|', ' ' })[0].Trim();
                int c = 0;
                if (reg.IsMatch(cut_line))
                {
                    DB.NEW_PERSON((cut_line, cut_line, cut_line), DateTime.Now, "generated", cut_line, Functions.GetHashString(cut_line));
                    Console.WriteLine(c++);
                }
            }
        }
    }
}
