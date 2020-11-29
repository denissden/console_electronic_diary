using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Constants
    {
        public static readonly string DB_Path = @"D:\_C# projects\Elektronniy_Dnevnik\ConsoleApp1\data\";
        public static readonly string[,] Styles = { { "-", "|", "+", " " }, { "=", " ", "#", " " }, { "#", "#", "#", "-" } };
        public static readonly ConsoleColor[,] Colors = { { ConsoleColor.Black, ConsoleColor.White },
        { ConsoleColor.White, ConsoleColor.Black }, { ConsoleColor.Red, ConsoleColor.DarkRed } };
        //public static readonly Action[] DefaultActions = { Functions.Pass, Functions.Exit, Functions.WriteTest};
        public static readonly Dictionary<string, Action[]> Actions = new Dictionary<string, Action[]>()
        {
            {"default", new Action[] { Functions.Pass, Functions.Exit, Functions.WriteTest} },
            {"ui", new Action[] { NewAccount.MainScreen } }
        };
        public static readonly Dictionary<string, Func<string, bool>> Validations = new Dictionary<string, Func<string, bool>>()
        {
            {"any", Validation.True},
            {"none", Validation.False},
            {"longer8", Validation.Longer8},
            {"shorter30", Validation.Shorter30},
            {"login", Validation.Login},
            {"password", Validation.Password},
        };
    }

    class Program
    {
        static void Main(string[] args) // 120 x 30
        {
            /*Test.Run();
            Console.ReadKey();*/



            DB db = new DB();
            db.DB_Path = Constants.DB_Path;
            UI ui = db.READ_JSON_UI("layout/StartPage");

            /*UI ui = new UI();

            Button b = new Button("New", "New account", (45, 7), (31, 5));
            b.Style = 1;
            b.Value = 0;
            ui.Buttons.Add(b);
            b.SetOnClick(2);
            b = new Button("Login", "Login", (45, 11), (31, 5));
            b.Style = 1;
            ui.Buttons.Add(b);
            b = new Button("Exit", "Exit", (45, 18), (31, 5));
            b.Style = 1;
            b.Color = 2;
            b.SetOnClick(1);
            ui.Buttons.Add(b);

            TextLine t = new TextLine("Header", "Welcome to the Electronic Diary!", (0, 0), 120, 0);
            ui.TextLines.Add(t);*/

            ConsoleKeyInfo key;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                ui.SelectByKey(key);
            } while (key.Key != ConsoleKey.Escape);

            //Button bt = new Button("One", Convert.ToString((char)(65)), (10, 10), (5, 5));
            //db.JSON_UI(ui, "layout/StartPage");

            /*for (int i = 0; i < 13; i++)
            {
                ui.SetColor(i % 3);
                ui.SetStyle(i % 3);
                ui.DrawButton("TEST", (i * 8 + i * i / 2, 0), (5 + i, 5));

                
            }*/
            Console.ReadKey(true);
        }
    }
}
