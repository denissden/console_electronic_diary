using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AdminApprovalListScreen
    {
        public static void MainScreen(dynamic _)
        {
            // UI ui = DB.READ_JSON_UI("layout/AdminApprovalListScreen");

            UI ui = new UI();

            TextLine t; InputBox i; Button b; ListSelect l;

            // HEADER
            t = new TextLine("Header", "Admin Menu", (0, 0), 120, 0);
            ui.Elements.Add(t);


            // BUTTONS
            b = new Button("UserList", "List of users", (47, 5), (26, 3))
            {
                Style = 1,
                Color = 1,
            };
            ui.Elements.Add(b);

            /*b = new Button("Back", "Back", (47, 19), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);*/

            List<Person> users = DB.SELECT_PERSON_BY_TYPE<Person>("Guest");

            for (int index = 0; index < users.Count; index++)
            {
                Person p = users[index];
                l = new ListSelect($"user{index}", $"{p.Login}, {p.GetFullName()}, {p.BirthYear.ToString("dd.MM.yyyy")}.  Set role to ", (0, 10 + index), (120, 1))
                {

                    Color = 1,
                    DrawBorder = false,
                    Options = Constants.UserTypes,
                    Fit = -1,
                };
                ui.Elements.Add(l);
            }

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

            //DB.JSON_UI(ui, "layout/AdminApprovalListScreen");
        }
    }
}
