using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AdminScreen
    {
        public static void MainScreen(dynamic login)
        {
            Person p = DB.READ_PERSON_BY_LOGIN<Person>(login);

            // UI ui = DB.READ_JSON_UI("layout/AdminScreen");

            UI ui = new UI();

            TextLine t; InputBox i; Button b;

            // HEADER
            t = new TextLine("Header", "Login Page", (0, 0), 120, 0);
            ui.Elements.Add(t);

            // LOGIN
            t = new TextLine("LoginInputLabel", "Login", (42, 5), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("LoginInputBox", "", (42, 6), 36, 0)
            {
                Style = 1,
                ValidationType = new List<string>() { "nonempty", "login_exists" },
            };
            ui.Elements.Add(i);


            t = new TextLine("PasswordInputLabel", "Password", (42, 10), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("PasswordInputBox", "", (42, 11), 36, 0)
            {
                Style = 1,
                ValidationType = new List<string>() { "nonempty" },
            };
            ui.Elements.Add(i);

            b = new Button("Log in", "Log in", (47, 15), (26, 3))
            {
                Style = 1,
                Color = 1,
            };
            ui.Elements.Add(b);

            b = new Button("Back", "Back", (47, 19), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);


            ui.Update();
            ui.ValidateAll();
            Functions.SetColor(1);
            Console.Clear();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "Back") break;
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            //DB.JSON_UI(ui, "layout/AdminScreen");
        }
    }
}
