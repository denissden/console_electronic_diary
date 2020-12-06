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
        public static readonly string USERS_Path = @"users\";
        public static readonly string IDS_Path = @"id_lib\";
        public static readonly string[,] Styles = { { "-", "|", "+", " " }, { "=", " ", "#", " " }, { "#", "#", "#", "-" } };
        public static readonly ConsoleColor[,] Colors = { { ConsoleColor.Black, ConsoleColor.White },
        { ConsoleColor.White, ConsoleColor.Black }, { ConsoleColor.Red, ConsoleColor.DarkRed } };
        //public static readonly Action[] DefaultActions = { Functions.Pass, Functions.Exit, Functions.WriteTest};
        //public Stack OutputStack = new Stack();

        public static readonly string[] DatePresets = new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd.MM.yyyy", "dd MM yyyy" };

        
        public static readonly Dictionary<string, Action<dynamic>> Actions = new Dictionary<string, Action<dynamic>>()
        {
            {"default", Functions.Pass },
            {"exit", Functions.Exit },
            {"test", Functions.WriteTest },
            {"new_account", NewAccount.MainScreen },
            {"login", Login.MainScreen },
        };
        public static readonly Dictionary<string, Func<string, bool>> Validations = new Dictionary<string, Func<string, bool>>()
        {
            {"any", Validation.True},
            {"none", Validation.False},
            {"longer8", Validation.Longer8},
            {"longer3", Validation.Longer3},
            {"nonempty", Validation.NonEmpty},
            {"shorter30", Validation.Shorter30},   
            {"shorter10", Validation.Shorter10},   
            {"date", Validation.Date},   
            {"login", Validation.NewLogin},
            {"password", Validation.Password},
            {"passwords_match1", Validation.PasswordsMatchAdd},
            {"passwords_match2", Validation.PasswordsMatchCheck},
            {"login_exists", Validation.LoginExists},
        };
        public static readonly Dictionary<string, Action<dynamic>> UserMainScreens = new Dictionary<string, Action<dynamic>>()
        {
            {"Guest", GuestScreen.MainScreen },
            {"Student", Functions.Pass },
            {"Teacher", Functions.Pass },
            {"Admin", AdminScreen.MainScreen },
        };


        public static readonly bool SkipVerificationAtAccountCreation = true;
    }

    class Program
    {
        static void Main(string[] args) // 120 x 30
        {
            /*Test.Run();
            Console.ReadKey();*/

            UI ui = DB.READ_JSON_UI("layout/StartPage");

            ui.Update();
            ConsoleKeyInfo key;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                ui.SelectByKey(key);
            } while (key.Key != ConsoleKey.Escape);

            //db.JSON_UI(ui, "layout/StartPage");

            Console.ReadKey(true);
        }
    }
}
