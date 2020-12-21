using System;
using System.Collections.Generic;
using System.IO;
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
            t = new TextLine("Header", "Admin Menu", (0, 0), 120, 0);
            ui.Elements.Add(t);


            // BUTTONS
            b = new Button("UserList", "List of users", (47, 5), (26, 3))
            {
                Style = 1,
                Color = 1,
                ActionType = "admin_user_list",
            };
            ui.Elements.Add(b);

            b = new Button("Appoval", "Waiting for approval", (47, 10), (26, 3))
            {
                Style = 1,
                Color = 1,
                ActionType = "admin_approval_list",
            };
            ui.Elements.Add(b);

            b = new Button("EditGroups", "Edit Groups", (47, 15), (26, 3))
            {
                Style = 1,
                Color = 1,
                ActionType = "admin_edit_groups",
            };
            ui.Elements.Add(b);

            b = new Button("Back", "Log out", (47, 24), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);


            // FOOTER
            t = new TextLine("Footer", "Total users: ", (0, 28), 120, 0);
            t.AddText($"{Directory.GetFiles(Constants.DB_Path + Constants.IDS_Path, "*", SearchOption.TopDirectoryOnly).Length}");

            ui.Elements.Add(t);


            ui.Update();
            ui.ValidateAll();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "Back") break;
                else if (clicked == "Appoval" || clicked == "UserList" || clicked == "EditGroups") ui.Update();
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            //DB.JSON_UI(ui, "layout/AdminScreen");
        }
    }
}
