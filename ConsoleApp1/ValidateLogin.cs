using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class ValidateLogin
    {
        public static bool Validate(string login, string hash)
        {

            if (!DB.PERSON_EXISTS(login)) return false;
            Person p = DB.READ_PERSON_BY_LOGIN<Person>(login);
            string p_hash = p.PasswordHash;
            if (hash != p_hash) return false;

            string type = p.Type;

            switch (type)
            {
                case "Student":
                    break;
            }
            Console.Clear();
            Console.WriteLine("DONE");
            Console.ReadKey();
            return true;

        }
    }
}
