using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class GuestScreen
    {
        public static void MainScreen(dynamic login)
        {
            Person p = DB.READ_PERSON_BY_LOGIN<Person>(login);

            UI ui = DB.READ_JSON_UI("layout/GuestScreen");

            ui.GetByName("Login").AddText(p.Login);
            ui.GetByName("FirstName").AddText(p.FirstName);
            ui.GetByName("MiddleName").AddText(p.MiddleName);
            ui.GetByName("LastName").AddText(p.LastName);
            ui.GetByName("Group").AddText(p.Group);
            ui.GetByName("BirthDate").AddText(p.BirthYear.ToString("dd.MM.yyyy"));

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

            // DB.JSON_UI(ui, "layout/GuestScreen");
        }
    }
}
