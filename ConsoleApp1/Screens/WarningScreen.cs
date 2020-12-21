using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class WarningScreen
    {
        public static bool NoYes(dynamic warning)
        {
            UI ui = new UI();

            Button b; TextLine t;

            t = new TextLine("Warning", warning.ToString(), (0, 10), 120, 0);
            ui.Elements.Add(t);

            b = new Button("No", "No", (34, 12), (26, 5))
            {
                Style = 1,
                Color = 1,
            };
            ui.Elements.Add(b);

            b = new Button("Yes", "Yes", (62, 12), (26, 5))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);

            ui.Update();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "Yes") return true;
                if (clicked == "No") return false;
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            return false;
        }

        public static void Error(dynamic error)
        {
            UI ui = new UI();

            Button b; TextLine t;

            t = new TextLine("Header", "ERROR!", (45, 7), 30, 0)
            {
                Color = 2,
            };
            ui.Elements.Add(t);

            t = new TextLine("Warning", error.ToString(), (0, 10), 120, 0);
            ui.Elements.Add(t);

            b = new Button("Ok", "Ok", (47, 12), (26, 5))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);

            ui.Update();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "Ok") return;
            } while (key.Key != ConsoleKey.Escape);
        }
    }
}
