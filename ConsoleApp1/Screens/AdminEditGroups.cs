using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AdminEditGroups
    {
        public static void MainScreen(dynamic e)
        {
            // UI ui = DB.READ_JSON_UI("layout/TeacherScreen");

            UI ui = new UI();

            TextLine t; InputBox i; Button b; ListView lv;

            // NEW GROUP
            t = new TextLine("NewGroupInputLabel", "Add Group", (39, 0), 20, 0);
            ui.Elements.Add(t);
            i = new InputBox("NewGroupInputBox", "", (39, 1), 20, 0)
            {
                Style = 1,
                ValidationType = new List<string>() { "shorter20", "new_group" },
            };
            ui.Elements.Add(i);

            b = new Button("NewGroup", "Add Group", (61, 1), (20, 3))
            {
                Style = 1,
                Color = 1,
            };
            ui.Elements.Add(b);

            // MESSAGE
            t = new TextLine("Message", "", (83, 2), 30, 0);
            ui.Elements.Add(t);

            // LIST
            lv = new ListView("GroupList", (0, 4), (120, 22));
            lv.SetOptions(new List<string>() { "Removed", "Unchanged", "Changed", });
            //lv.AddToValueAction = "teacher_edit_group";
            ui.Elements.Add(lv);

            // BUTTONS
            b = new Button("Save", "Save", (18, 27), (26, 3))
            {
                Style = 1,
                Color = 1,
            };
            ui.Elements.Add(b);

            b = new Button("SaveExit", "Save and exit", (47, 27), (26, 3))
            {
                Style = 1,
                Color = 1,
            };
            ui.Elements.Add(b);

            b = new Button("Exit", "Exit without saving", (76, 27), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);

            ui.InterceptsInput = "GroupList";
            ui.DoInterceptInput = true;


            dynamic groups = DB.LOAD_ALL_GROUPS();
            List<ChoiceMapElement> groups_map = Functions.CreateChoiceMap(groups, default_choice: "Unchanged");

            dynamic grouplist = ui.GetByName("GroupList");
            grouplist.SetItems(groups_map, false);
            grouplist.AddToValueAction = "admin_edit_group";
            grouplist.PersonToStringOptions = "login,_date,id";

            ui.Update();
            //ui.ValidateAll();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "NewGroup")
                {
                    string name = ui.GetByName("NewGroupInputBox").OriginalText;
                    Group g = new Group(name);
                    bool success = DB.NEW_GROUP(g);
                    if (success)
                    {
                        grouplist.Add(new ChoiceMapElement(g, "Unchanged", "Unchanged"));
                        ui.GetByName("Message").SetText("Successfully created");
                    }
                    else ui.GetByName("Message").SetText("Group already exists!");
                    ui.Draw(true);
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
                    if (yes) DB.SAVE_GROUP_CHOICE_MAP(ui.GetByName("GroupList").Items);
                    grouplist.RemoveAtState("Removed");
                    grouplist.UnchangeAll();
                    ui.Update();
                }
                else if (clicked == "SaveExit")
                {
                    bool yes = WarningScreen.NoYes("You are going to save changes. There is no undo. Are you sure?");
                    if (yes)
                    {
                        DB.SAVE_GROUP_CHOICE_MAP(ui.GetByName("GroupList").Items);
                        break;
                    }
                    else ui.Update();
                }
                else if (clicked == "UPDATE") ui.Update();
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            //DB.JSON_PERSON(p, Constants.USERS_Path);
            //DB.JSON_UI(ui, "layout/TeacherScreen");
        }
    }
}
