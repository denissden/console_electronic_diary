using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AdminUserListScreen
    {
        public static void MainScreen(dynamic _)
        {
            UI ui = DB.READ_JSON_UI("layout/AdminUserListScreen");

            //UI ui = new UI();

            //TextLine t; InputBox i; Button b; ListSelect l; ListView lv;

            ui.InterceptsInput = "UserList";
            ui.DoInterceptInput = true;

            List<dynamic> users = DB.LOAD_ALL_PEOPLE();
            List<ChoiceMapElement> users_map = Functions.CreateChoiceMap(users, default_choice: "Unchanged");

            dynamic userlist = ui.GetByName("UserList");
            userlist.SetItems(users_map, true);
            userlist.AddToValueAction = "admin_edit_user";
            userlist.PersonToStringOptions = "login,_date,id";

            ui.Update();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "SortPropertyInputBox")
                {
                    string property = ui.GetByName("SortPropertyInputBox").OriginalText;
                    userlist.Sort(property);
                }
                else if (clicked == "Search")
                {
                    string query = ui.GetByName("SearchInputBox").OriginalText;
                    string property = ui.GetByName("SearchPropertyInputBox").OriginalText;

                    string count = userlist.Search(query, property);

                    ui.GetByName("SearchQueryInfo").AddText(query);
                    ui.GetByName("SearchPropertyInfo").AddText(property);
                    ui.GetByName("SearchResultsInfo").AddText(count);
                    ui.Update(clear: false);
                }
                else if (clicked == "ShowPropertyInputBox")
                {
                    userlist.PersonToStringOptions = ui.GetByName("ShowPropertyInputBox").OriginalText;
                    userlist.Update();
                }
                else if (clicked == "Exit")
                {
                    bool yes = WarningScreen.NoYes("All changes will be lost. Are you sure?");
                    if (yes) break;
                    else ui.Update();
                }
                else if (clicked == "Save")
                {
                    bool yes = WarningScreen.NoYes("You are going to save changes. There is no undo. Are you sure?");
                    if (yes)DB.SAVE_PERSON_CHOICE_MAP(ui.GetByName("UserList").Items, false);
                    userlist.UnchangeAll();
                    ui.Update();
                }
                else if (clicked == "SaveExit")
                {
                    bool yes = WarningScreen.NoYes("You are going to save changes. There is no undo. Are you sure?");
                    if (yes)
                    {
                        DB.SAVE_PERSON_CHOICE_MAP(ui.GetByName("UserList").Items, false);
                        break;
                    }
                    else ui.Update();
                }
                else if (clicked == "UPDATE") ui.Update();
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();


            // DB.JSON_UI(ui, "layout/AdminUserListScreen");
        }
    }
}
