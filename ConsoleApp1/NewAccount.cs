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
            // UI ui = db.READ_JSON_UI("layout/CreateAccount");

            UI ui = new UI();

            TextLine t; InputBox i;

            // HEADER
            t = new TextLine("Header", "Creating New Account", (0, 0), 120, 0);
            ui.Elements.Add(t);

            // NAME
            t = new TextLine("FirstNameInputLabel", "First Name", (3, 4), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("FirstNameInputBox", "", (3, 5), 36, 0)
            {
                Style = 1,
                ValidationType = "shorter30",
            };
            ui.Elements.Add(i);

            t = new TextLine("MiddleNameInputLabel", "Middle Name", (42, 4), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("MiddleNameInputBox", "", (42, 5), 36, 0)
            {
                Style = 1,
                ValidationType = "shorter30",
            };
            ui.Elements.Add(i);

            t = new TextLine("LastNameInputLabel", "Last Name", (81, 4), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("LastNameInputBox", "", (81, 5), 36, 0)
            {
                Style = 1,
                ValidationType = "shorter30",
            };
            ui.Elements.Add(i);

            // LOGIN
            t = new TextLine("LoginInputLabel", "Login", (3, 4), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("LoginInputBox", "", (3, 5), 36, 0)
            {
                Style = 1,
                ValidationType = "longer",
            };
            ui.Elements.Add(i);

            t = new TextLine("Password1InputLabel", "Password", (42, 4), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("Password1InputBox", "", (42, 5), 36, 0)
            {
                Style = 1,
                ValidationType = "password",
            };
            ui.Elements.Add(i);

            t = new TextLine("Password2InputLabel", "Repeat Password", (81, 4), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("Password2InputBox", "", (81, 5), 36, 0)
            {
                Style = 1,
                ValidationType = "password",
            };
            ui.Elements.Add(i);


            Console.Clear();
            ConsoleKeyInfo key;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                ui.SelectByKey(key);
            } while (key.Key != ConsoleKey.Escape);

            db.JSON_UI(ui, "layout/CreateAccount");
        }
    }
}
