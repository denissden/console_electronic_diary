using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace ConsoleApp1
{
    public class DB
    {
        public static void JSON_UI(UI ui, string path)
        {
            int c = 0;
            foreach (dynamic e in ui.Elements) JSON_ELEMENT(e, path, ++c);
            /*c = 0;
            foreach (TextLine t in ui.TextLines) JSON_ELEMENT(t, path, ++c);*/
            /*var roundTrippedJson = JsonSerializer.Serialize<UI>
                (ui);
            Console.WriteLine($"Output JSON: {roundTrippedJson}");
            using (FileStream fs = File.Create($"{DB_Path}{path}/damn.json"))
            {
                JsonSerializer.SerializeAsync(fs, ui);
            }*/
        }

        async public static void JSON_ELEMENT(dynamic b, string path, int id = 0)
        {
            var options = new JsonSerializerOptions()
            {
                IncludeFields = true,
            };
            Directory.CreateDirectory($"{Constants.DB_Path}{path}");
            using (FileStream fs = File.Create($"{Constants.DB_Path}{path}/{b.GetType().Name}_{id}_{b.Name}.json"))
            {
                await JsonSerializer.SerializeAsync(fs, b, options);
            }
        }

        public static UI READ_JSON_UI(string path)
        {
            UI ui = new UI();
            DirectoryInfo d = new DirectoryInfo($"{Constants.DB_Path}{path}");
            FileInfo[] files = d.GetFiles();
            Array.Sort(files, (x, y) =>
            {
                string xFilename = x.Name;
                string yFilename = y.Name;
                int xId = Convert.ToInt32(xFilename.Split('_')[1]);
                int yId = Convert.ToInt32(yFilename.Split('_')[1]);
                Console.Write(xId);
                Console.WriteLine(yId);
                return xId > yId ? 1 : -1;
            });


            foreach (FileInfo f in files)
            {
                string input = f.Name;
                Console.WriteLine(input);
                int index = input.IndexOf('_');
                if (index > 0)
                    input = input.Substring(0, index);

                object T = null;
                switch (input)
                {
                    case "Button":
                        T = READ_JSON_OBJECT<Button>(f);
                        break;
                    case "TextLine":
                        T = READ_JSON_OBJECT<TextLine>(f);
                        break;
                    case "InputBox":
                        T = READ_JSON_OBJECT<InputBox>(f);
                        break;
                    case "ListSelect":
                        T = READ_JSON_OBJECT<ListSelect>(f);
                        break;
                }
                if (T != null)
                    ui.Elements.Add(T);
                Console.WriteLine(input);
            }
            Console.WriteLine(ui.Elements);
            Console.WriteLine(ui.Elements.Count);
            Console.Clear();
            return ui;
        }

        public static T READ_JSON_OBJECT<T>(FileInfo path)
        {
            string s = File.ReadAllText(path.FullName);
            object t = JsonSerializer.Deserialize<T>(s);
            return (T)t;
        }
        // BINARY
        public static ulong READ_BINARY_ULONG(string path)
        {
            ulong ret;
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                ret = reader.ReadUInt64();
            return ret;
        }

        public static void WRITE_BINARY_ULONG(string path, ulong value)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                writer.Write(value);
        }

        // PERSON
        public static void NEW_PERSON((string, string, string) name, DateTime date, string group, string login, string hash)
        {
            (string name_first, string name_middle, string name_last) = name;

            ulong id = READ_BINARY_ULONG(Constants.DB_Path + Constants.USERS_Path + "id.dat") + 1;
            WRITE_BINARY_ULONG(Constants.DB_Path + Constants.USERS_Path + "id.dat", id);

            Person p = new Person()
            {
                FirstName = name_first,
                MiddleName = name_middle,
                LastName = name_last,
                BirthYear = date,
                Group = group,
                Login = login,
                PasswordHash = hash,
                Id = id
            };

            JSON_PERSON(p, Constants.USERS_Path);

            WRITE_BINARY_ULONG(Constants.DB_Path + Constants.IDS_Path + $"{p.Login}.dat", id);


            Console.Clear();
            Console.WriteLine($"{name_first} {name_middle} {name_last}\n" +
                $"{date.ToString()}\n" +
                $"{group}\n" +
                $"{login}\n" +
                $"{hash}\n" +
                $"ID: {p.Id}");

            Console.ReadKey();
            Console.Clear();
        }

        async public static void JSON_PERSON(dynamic p, string path)
        {
            var options = new JsonSerializerOptions()
            {
                IncludeFields = true,
            };
            System.IO.Directory.CreateDirectory($"{Constants.DB_Path}{path}");
            using (FileStream fs = File.Create($"{Constants.DB_Path}{path}/id{p.Id}.json"))
            {
                await JsonSerializer.SerializeAsync(fs, p, options);
            }
        }

        public static T READ_PERSON_BY_LOGIN<T>(string login)
        {
            ulong id = READ_BINARY_ULONG(Constants.DB_Path + Constants.IDS_Path + $"{login}.dat");
            FileInfo path = new FileInfo(Constants.DB_Path + Constants.USERS_Path + $"id{id}.json");
            T ret = READ_JSON_OBJECT<T>(path);
            return ret;
        }

        public static T READ_PERSON_BY_ID<T>(ulong id)
        {
            FileInfo path = new FileInfo(Constants.DB_Path + Constants.USERS_Path + $"id{id}.json");
            T ret = READ_JSON_OBJECT<T>(path);
            return ret;
        }

        public static bool PERSON_EXISTS(string login)
        {
            FileInfo path = new FileInfo(Constants.DB_Path + Constants.IDS_Path + $"{login}.dat");
            return path.Exists;
        }

        public static bool PERSON_EXISTS(ulong id)
        {
            FileInfo path = new FileInfo(Constants.DB_Path + Constants.USERS_Path + $"id{id}.json");
            return path.Exists;
        }

        public static List<T> SELECT_PERSON_BY_TYPE<T>(string type)
        {
            DirectoryInfo d = new DirectoryInfo(Constants.DB_Path + Constants.IDS_Path);
            FileInfo[] files = d.GetFiles();
            List<T> result = new List<T>();

            foreach(FileInfo file in files)
            {
                dynamic p = READ_PERSON_BY_LOGIN<Person>(file.Name.Replace(".dat", ""));
                if (p.Type == type)
                    result.Add(p);
            }
            return result;
        }

    }
}
