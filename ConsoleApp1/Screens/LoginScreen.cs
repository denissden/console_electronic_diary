using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class LoginScreen
    {
        public static void MainScreen(dynamic _)
        {
            UI ui = DB.READ_JSON_UI("layout/Login");

            ui.Update();
            ui.ValidateAll();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);
                if (clicked == "Log in")
                {
                    string login = ui.GetByName("LoginInputBox").OriginalText;
                    string hash = Functions.GetHashString(ui.GetByName("PasswordInputBox").OriginalText);
                    bool success = ValidateLogin.Validate(login, hash);
                    if (success) break;
                    else ui.GetByName("PasswordInputBox").InputValid = false;

                }
                if (clicked == "Back") break;
                // if (clicked != "") ui.Update();
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            // DB.JSON_UI(ui, "layout/Login");
        }
    }
}
