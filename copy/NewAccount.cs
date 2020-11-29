using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class NewAccount
    {
        public static void MainScreen()
        {
            DB db = new DB();
            db.DB_Path = Constants.DB_Path;
            //UI ui = db.READ_JSON_UI("layout/LoginPage");

            UI ui = new UI();
            Button b = new Button("New", "New account", (45, 7), (31, 5));
            b.Style = 1;
            b.Value = 0;
            ui.Elements.Add(b);
            b.SetOnClick(2);
            b = new Button("Login", "Login", (45, 11), (31, 5));
            b.Style = 1;
            ui.Elements.Add(b);
            b = new Button("Back", "Back", (45, 18), (31, 5));
            b.Style = 1;
            b.Color = 2;
            b.SetOnClick(1);
            ui.Elements.Add(b);

            TextLine t = new TextLine("Header", "Creating New Account", (0, 0), 120, 0);
            ui.Elements.Add(t);

            ui.GetByName("Header").SetText("Suck my dick");

            Functions.SetColor(1);
            Console.Clear();

            ConsoleKeyInfo key;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                ui.SelectByKey(key);
            } while (key.Key != ConsoleKey.Escape);
        }
    }
}
