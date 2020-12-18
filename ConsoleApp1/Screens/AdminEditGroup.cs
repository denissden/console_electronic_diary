using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AdminEditGroup
    {
        public static void MainScreen(dynamic e)
        {
            dynamic g = e.Element;

            // UI ui = DB.READ_JSON_UI("layout/AdminScreen");

            UI ui = new UI();

            TextLine t; InputBox i; Button b; ListSelect l; ListView lv;

            // HEADER
            t = new TextLine("Header", $"Group: {g.Name}", (0, 0), 120, 0);
            ui.Elements.Add(t);

            // MESSAGE
            t = new TextLine("Message", "", (83, 2), 30, 0);
            ui.Elements.Add(t);

            // LIST
            lv = new ListView("GroupList", (0, 2), (120, 22));
            lv.SetOptions(new List<string>() { "Removed", "Unchanged", });
            //lv.AddToValueAction = "teacher_edit_group";
            ui.Elements.Add(lv);

            // BUTTONS
            b = new Button("Apply", "Apply", (19, 27), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);

            b = new Button("Remove", "Remove Group", (47, 27), (26, 3))
            {
                Style = 1,
                Color = 1,
            };
            ui.Elements.Add(b);

            b = new Button("Exit", "Exit", (75, 27), (26, 3))
            {
                Style = 1,
                Color = 1,
            };
            ui.Elements.Add(b);



            ui.InterceptsInput = "GroupList";
            ui.DoInterceptInput = true;

            List<dynamic> users = new List<dynamic>();
            foreach (dynamic id in g.People)
            {
                dynamic p = DB.READ_PERSON_BY_ID<Person>(id);
                users.Add(p);
            }
            List<ChoiceMapElement> users_map = Functions.CreateChoiceMap(users, default_choice: "Unchanged");

            dynamic userlist = ui.GetByName("GroupList");
            userlist.SetItems(users_map, false);
            userlist.PersonToStringOptions = "login,id,_date";

            dynamic remove_button = ui.GetByName("Remove");
            if (e.State == "Removed")
            {
                remove_button.SetText("Restore");
            }

            ui.Update();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "Exit")
                {
                    bool yes = WarningScreen.NoYes("Changes will be lost. Are you sure?");
                    if (yes) break;
                    else ui.Update();
                }
                else if (clicked == "Remove")
                {
                    if (e.State != "Removed")
                    {
                        bool yes = WarningScreen.NoYes("You are going to delete a group. You can undo it only before you save changes.");
                        if (yes)
                        {
                            e.State = "Removed";
                            e.Changed = true;
                            OutputStack.Push(e);
                            break;
                        }
                        else ui.Update();
                    }
                    else
                    {
                        e.State = "Changed";
                        e.Changed = true;
                        OutputStack.Push(e);
                        break;
                    }
                    
                }
                else if (clicked == "Apply")
                {
                    List<ulong> updated_users = new List<ulong>();
                    foreach (ChoiceMapElement cme in userlist.Items)
                    {
                        if (cme.State != "Removed")
                        {
                            updated_users.Add(cme.Element.Id);
                        }
                    }
                    g.People = updated_users;

                    e.Element = g;
                    e.State = "Changed";
                    e.Changed = true;
                    OutputStack.Push(e);
                    break;
                }
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();
        }
    }
}
