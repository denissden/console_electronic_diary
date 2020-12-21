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
            UI ui = DB.READ_JSON_UI("layout/AdminApprovalListScreen");

            // UI ui = new UI();

            ui.InterceptsInput = "UserList";
            ui.DoInterceptInput = true;

            List<dynamic> users = DB.SELECT_PERSON_BY_TYPE("Guest");
            List<ChoiceMapElement> users_map = Functions.CreateChoiceMap(users, get_property: "Type");

            ui.GetByName("UserList").SetItems(users_map, true);
            ui.GetByName("UserList").PersonToStringOptions = "login,_date,id";
            
            ui.Update();
            ui.ValidateAll();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "Exit")
                {
                    bool yes = WarningScreen.NoYes("All changes will be lost. Are you sure?");
                    if (yes) break;
                    else ui.Update();
                }
                else if (clicked == "Save")
                {
                    bool yes = WarningScreen.NoYes("You are going to save changes. There is no undo. Are you sure?");
                    if (yes) DB.SAVE_PERSON_CHOICE_MAP(ui.GetByName("UserList").Items);
                    ui.Update();
                }
                else if (clicked == "SaveExit")
                {
                    bool yes = WarningScreen.NoYes("You are going to save changes. There is no undo. Are you sure?");
                    if (yes)
                    {
                        DB.SAVE_PERSON_CHOICE_MAP(ui.GetByName("UserList").Items);
                        break;
                    }
                    else ui.Update();
                }
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            // DB.JSON_UI(ui, "layout/AdminApprovalListScreen");
        }
    }
}
