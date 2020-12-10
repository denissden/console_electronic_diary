using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class StartScreen
    {
        public static void MainScreen(dynamic _)
        {
            UI ui = DB.READ_JSON_UI("layout/StartPage");

            ui.Update();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);
                if (clicked != "") ui.Update();
            } while (key.Key != ConsoleKey.Escape);

            // DB.JSON_UI(ui, "layout/StartPage");
        }
    }
}
