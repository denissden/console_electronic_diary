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
        public string DB_Path = "";

        public void JSON_UI(UI ui, string path)
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

        async public void JSON_ELEMENT(dynamic b, string path, int id = 0)
        {
            var options = new JsonSerializerOptions()
            {
                IncludeFields = true,
            };
            System.IO.Directory.CreateDirectory($"{DB_Path}{path}");
            using (FileStream fs = File.Create($"{DB_Path}{path}/{b.GetType().Name}_{id}_{b.Name}.json"))
            {
                await JsonSerializer.SerializeAsync(fs, b, options);
            }
        }


        public UI READ_JSON_UI(string path)
        {
            UI ui = new UI();
            DirectoryInfo d = new DirectoryInfo($"{DB_Path}{path}");
            foreach (FileInfo f in d.GetFiles())
            {
                string input = f.Name;
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

        public Button READ_JSON_BUTTON(FileInfo path)
        {
            string s = File.ReadAllText(path.FullName);
            Button b = JsonSerializer.Deserialize<Button>(s);
            b.Update();
            return b;
        }

        public TextLine READ_JSON_TEXTLINE(FileInfo path)
        {
            string s = File.ReadAllText(path.FullName);
            TextLine t = JsonSerializer.Deserialize<TextLine>(s);
            return t;
        }

        public T READ_JSON_OBJECT<T>(FileInfo path)
        {
            string s = File.ReadAllText(path.FullName);
            object t = JsonSerializer.Deserialize<T>(s);
            return (T)t;
        }



    }
}
