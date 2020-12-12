using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Test
    {
        public static void Run()
        {
            //UI ui = DB.READ_JSON_UI("layout/Login");

            UI ui = new UI();

            Button b = new Button("B", "0", (47, 15), (26, 3))
            {
                Style = 0,
                Color = 1,
                ValueClipMin = 10,
                Fill = false,
            };
            b.Value = 10;
            ui.Elements.Add(b);

            ui.Update();
            ui.ValidateAll();
            Functions.SetColor(1);
            Console.Clear();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                Functions.SetColor(1);
                Console.Clear();
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);
                dynamic bu = ui.GetByName("B");
                int x = Convert.ToInt32(bu.Value);
                bu.W = x;
                bu.Update();
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            // DB.JSON_UI(ui, "layout/Login");
        }

        public static void Run2()
        {
            while (true)
                Console.WriteLine(Console.ReadKey().Key.ToString());
        }
    }
}
