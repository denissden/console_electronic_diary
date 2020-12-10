using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class NewAccountScreen
    {
        public static void MainScreen(dynamic _)
        {
            UI ui = DB.READ_JSON_UI("layout/CreateAccount");

            ui.Update();
            ui.ValidateAll();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "CreateAccount")
                {
                    if (ui.AllValid() || Constants.SkipVerificationAtAccountCreation)
                    {
                        string name_first = ui.GetByName("FirstNameInputBox").OriginalText;
                        string name_middle = ui.GetByName("MiddleNameInputBox").OriginalText;
                        string name_last = ui.GetByName("LastNameInputBox").OriginalText;
                        DateTime date = Functions.ToDatetime(ui.GetByName("DateInputBox").OriginalText);
                        string group = ui.GetByName("GroupInputBox").OriginalText;
                        string login = ui.GetByName("LoginInputBox").OriginalText;
                        string hash = Functions.GetHashString(ui.GetByName("Password2InputBox").OriginalText);

                        DB.NEW_PERSON(
                            (name_first, name_middle, name_last),
                            date,
                            group,
                            login,
                            hash
                            );

                        break;
                    }
                }
                else if (clicked == "Back") break;
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            // DB.JSON_UI(ui, "layout/CreateAccount");
        }
    }
}
