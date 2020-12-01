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
            System.IO.Directory.CreateDirectory($"{Constants.DB_Path}{path}");
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

        public static void NEW_PERSON((string, string, string) name, DateTime date, string group, string login, string hash)
        {
            (string name_first, string name_middle, string name_last) = name;

            ulong id;
            using (BinaryReader reader = new BinaryReader(File.Open(Constants.DB_Path + Constants.USERS_Path + "id.dat", FileMode.Open)))
                id = reader.ReadUInt64() + 1;
            using (BinaryWriter writer = new BinaryWriter(File.Open(Constants.DB_Path + Constants.USERS_Path + "id.dat", FileMode.OpenOrCreate)))
                writer.Write(id);

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
    }
}
