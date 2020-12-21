using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Threading;

namespace ConsoleApp1
{
    public class DB
    {
        public static void JSON_UI(UI ui, string path)
        {
            int c = 0;
            Directory.CreateDirectory($"{Constants.DB_Path}{path}");
            foreach (dynamic e in ui.Elements) JSON_ELEMENT(e, path, ++c);
        }

        async public static void JSON_ELEMENT(dynamic b, string path, int id = 0)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = false,
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
                    case "ListView":
                        T = READ_JSON_OBJECT<ListView>(f);
                        break;
                    case "Table":
                        T = READ_JSON_OBJECT<Table>(f);
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
            string s;
            using (StreamReader sr = new StreamReader(path.FullName))
            {
                s = sr.ReadToEnd();
            }
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
                writer.Write(Convert.ToUInt64(value));
        }

        // PERSON
        public static void NEW_PERSON((string, string, string) name, DateTime date, string group, string login, string hash)
        {
            if (Validation.LoginExists(login))
            {
                Console.WriteLine($"ERROR: login {login} already exists!");
                return;
            }

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


            //Console.Clear();
            /*Console.WriteLine($"{name_first} {name_middle} {name_last}\n" +
                $"{date.ToString()}\n" +
                $"{group}\n" +
                $"{login}\n" +
                $"{hash}\n" +
                $"ID: {p.Id}");*/

            // Console.ReadKey();
            Console.Clear();
        }

        async public static void JSON_PERSON(dynamic p, string path)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = false,
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
            T ret = READ_PERSON_BY_ID<T>(id);
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

        public static List<dynamic> SELECT_PERSON_BY_TYPE(string type)
        {
            DirectoryInfo d = new DirectoryInfo(Constants.DB_Path + Constants.IDS_Path);
            FileInfo[] files = d.GetFiles();
            List<dynamic> result = new List<dynamic>();

            foreach(FileInfo file in files)
            {
                dynamic p = READ_PERSON_BY_LOGIN<Person>(file.Name.Replace(".dat", ""));
                if (p.Type == type)
                    result.Add(p);
            }
            return result;
        }

        public static List<dynamic> LOAD_ALL_PEOPLE()
        {
            DirectoryInfo d = new DirectoryInfo(Constants.DB_Path + Constants.IDS_Path);
            FileInfo[] files = d.GetFiles();
            List<dynamic> result = new List<dynamic>();
            foreach (FileInfo file in files)
            {
                try
                {
                    Person p = READ_PERSON_BY_LOGIN<Person>(file.Name.Replace(".dat", ""));
                    p.GetAge();
                    result.Add(p);
                }
                catch
                {
                    Console.WriteLine($"Error reading {file}");
                }
            }
            return result;
        }

        public static List<dynamic> LOAD_PERSON_BY_PROPERTY_MATCH(string property, dynamic value) 
        {
            List<dynamic> result = new List<dynamic>();

            Person test = new Person();
            if (!Functions.HasProperty(test, property)) return null;

            DirectoryInfo d = new DirectoryInfo(Constants.DB_Path + Constants.IDS_Path);
            FileInfo[] files = d.GetFiles();

            foreach (FileInfo file in files)
            {
                Person p = READ_PERSON_BY_LOGIN<Person>(file.Name.Replace(".dat", ""));
                dynamic v = p.GetType().GetProperty(property).GetValue(p, null);
                if (v == value)
                    result.Add(p);
            }

            return result;
        }

        public static void SAVE_PERSON_CHOICE_MAP(List<ChoiceMapElement> map, bool set_type = true)
        {
            foreach (ChoiceMapElement p in map)
            {
                if (p.Changed == true && p.State != p.InitState)
                {
                    dynamic person = p.Element;
                    if (set_type)
                        person.Type = p.State.ToString();
                    if (person.Type != "Guest")
                        ADD_USER_TO_GROUP(person);
                    JSON_PERSON(person, Constants.USERS_Path);
                    Console.WriteLine($"{person} {person.Type}");
                }
            }
            Console.ReadKey();
        }



        // GROUP
        public static void JSON_GROUP(dynamic g, string path)
        {
            System.IO.Directory.CreateDirectory($"{Constants.DB_Path}{path}");
            string s = JsonSerializer.Serialize(g);
            File.WriteAllText($"{Constants.DB_Path}{path}/{g.Name}.json", s);

        }

        public static Group READ_GROUP(string name)
        {
            FileInfo path = new FileInfo(Constants.DB_Path + Constants.GROUPS_Path + $"{name}.json");
            if (path.Exists)
                return READ_JSON_OBJECT<Group>(path);
            return null;
        }

        public static List<Person> LOAD_PEOPLE_FROM_GROUP(Group g)
        {
            List<Person> ret = new List<Person>();
            foreach (ulong id in g.People)
            {
                Person p = READ_PERSON_BY_ID<Person>(id);
                ret.Add(p);
            }
            return ret;
        }

        public static List<dynamic> LOAD_ALL_GROUPS()
        {
            DirectoryInfo d = new DirectoryInfo(Constants.DB_Path + Constants.GROUPS_Path);
            FileInfo[] files = d.GetFiles();
            List<dynamic> result = new List<dynamic>();
            foreach (FileInfo file in files)
            {
                try
                {
                    Group p = READ_JSON_OBJECT<Group>(file);
                    result.Add(p);
                }
                catch
                {
                    Console.WriteLine($"Error reading {file}");
                }
            }
            return result;
        }

        public static void SAVE_GROUP_CHOICE_MAP(List<ChoiceMapElement> map)
        {
            foreach (ChoiceMapElement g in map)
            {
                if (g.Changed == true && g.State != g.InitState)
                {
                    if (g.State == "Removed") 
                    {
                        string path = Constants.DB_Path + Constants.GROUPS_Path + g.Element.Name + ".json";
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                            Console.WriteLine($"Deleted {g.Element}");
                        }
                    }
                    else
                    {
                        dynamic group = g.Element;
                        JSON_GROUP(group, Constants.GROUPS_Path);
                        Console.WriteLine($"{group}");
                    }
                }
            }
            Console.ReadKey();
        }

        public static bool NEW_GROUP(Group g)
        {
            if (!Validation.NewGroup(g.Name)) return false;

            JSON_GROUP(g, Constants.GROUPS_Path);
            return true;
        }

        public static void REMOVE_USER_FROM_GROUP(ulong id, string group)
        { 
            if (Validation.GroupExists(group))
            {
                Console.WriteLine($"Removing {id} from {group}");
                FileInfo path = new FileInfo(Constants.DB_Path + Constants.GROUPS_Path + $"{group}.json");
                Group g = READ_JSON_OBJECT<Group>(path);
                g.People.RemoveAll(p => p == id);
                JSON_GROUP(g, Constants.GROUPS_Path);
            }
        }

        public static void ADD_USER_TO_GROUP(Person p) 
        {
            ulong id = p.Id;
            string new_group = p.Group;
            string old_group = p.OldGroup;
            REMOVE_USER_FROM_GROUP(id, old_group);
            if (Validation.GroupExists(new_group))
            {
                Console.WriteLine($"Adding {p.Id} to {new_group}");
                FileInfo path = new FileInfo(Constants.DB_Path + Constants.GROUPS_Path + $"{new_group}.json");
                Group g = READ_JSON_OBJECT<Group>(path);
                if (!g.People.Contains(id))
                    g.People.Add(id);
                JSON_GROUP(g, Constants.GROUPS_Path);
            }
        }

        public static bool PERSON_IN_GROUP(ulong id, string group)
        {
            if (Validation.GroupExists(group))
            {
                FileInfo path = new FileInfo(Constants.DB_Path + Constants.GROUPS_Path + $"{group}.json");
                Group g = READ_JSON_OBJECT<Group>(path);
                return g.People.Contains(id);
            }
            return false;
        }

        // SUBJECT
        public static void ADD_SUBJECT_TO_GROUP(Group g, string name, ulong creator_id)
        {
            foreach (ulong id in g.People)
            {
                Person p = READ_PERSON_BY_ID<Person>(id);
                p.SubjectMarks.Add(new MarkList(name, creator_id));
                JSON_PERSON(p, Constants.USERS_Path);
            }
        }

        // TABLE
        public static void SAVE_TABLE(Table table)
        {
            List<Label> labels = table.RowLabels;
            foreach (Label l in labels)
            {
                Person p = l.Item;
                JSON_PERSON(p, Constants.USERS_Path);
            }
        }
    }
}
