using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AdminEditUser
    {
        public static void MainScreen(dynamic e)
        {
            // UI ui = DB.READ_JSON_UI("layout/AdminScreen");
            dynamic p = e.Element;


            UI ui = new UI();

            TextLine t; InputBox i; Button b; ListSelect l;

            // HEADER
            t = new TextLine("Header", "Edit user info", (0, 0), 120, 0);
            ui.Elements.Add(t);

            // NAME
            t = new TextLine("FirstNameInputLabel", "First Name", (3, 4), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("FirstNameInputBox", "", (3, 5), 36, 0)
            {
                Style = 1,
                ValidationType = new List<string>() { "shorter30", "nonempty" },
            };
            ui.Elements.Add(i);

            t = new TextLine("MiddleNameInputLabel", "Middle Name (optional)", (42, 4), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("MiddleNameInputBox", "", (42, 5), 36, 0)
            {
                Style = 1,
                ValidationType = new List<string>() { "shorter30" },
            };
            ui.Elements.Add(i);

            t = new TextLine("LastNameInputLabel", "Last Name", (81, 4), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("LastNameInputBox", "", (81, 5), 36, 0)
            {
                Style = 1,
                ValidationType = new List<string>() { "shorter30", "nonempty" },
            };
            ui.Elements.Add(i);

            // DATE
            t = new TextLine("DateInputLabel", "Date Of Birth (DD MM YYYY)", (16, 10), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("DateInputBox", "", (15, 11), 38, 0)
            {
                Style = 1,
                ValidationType = new List<string>() { "shorter30", "date", "nonempty" },
            };
            ui.Elements.Add(i);

            // GROUP
            t = new TextLine("GroupInputLabel", "Group (optional)", (67, 10), 36, 0);
            ui.Elements.Add(t);
            i = new InputBox("GroupInputBox", "", (67, 11), 36, 0)
            {
                Style = 1,
                ValidationType = new List<string>() { "shorter30", "group" },
            };
            ui.Elements.Add(i);

            t = new TextLine("IsInGroup", "Is in group: ", (67, 14), 36, 0);
            ui.Elements.Add(t);

            t = new TextLine("IdLabel", "Id: ", (47, 15), 26, 2);
            ui.Elements.Add(t);

            l = new ListSelect($"TypeSelect", "Account type: ", (47, 16), 26);
            l.Options = Constants.UserTypes;
            ui.Elements.Add(l);

            // BUTTONS
            b = new Button("Apply", "Apply", (31, 27), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);

            b = new Button("Exit", "Exit", (63, 27), (26, 3))
            {
                Style = 1,
                Color = 1,
            };
            ui.Elements.Add(b);

            

            ui.GetByName("FirstNameInputBox").OriginalText = p.FirstName;
            ui.GetByName("MiddleNameInputBox").OriginalText = p.MiddleName;
            ui.GetByName("LastNameInputBox").OriginalText = p.LastName;
            ui.GetByName("DateInputBox").OriginalText = p.BirthYear.ToString("dd.MM.yyyy");
            ui.GetByName("GroupInputBox").OriginalText = p.Group;
            ui.GetByName("IsInGroup").AddText(DB.PERSON_IN_GROUP(p.Id, p.Group).ToString());
            ui.GetByName("IdLabel").AddText(p.Id.ToString());
            ui.GetByName("TypeSelect").SetOptionSelect(p.Type);

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
                    bool yes = WarningScreen.NoYes("Changes will be lost. Are you sure?");
                    if (yes) break;
                    else ui.Update();
                }
                else if (clicked == "Apply" && ui.AllValid())
                {
                    ui.Update();
                    p.FirstName = ui.GetByName("FirstNameInputBox").OriginalText;
                    p.MiddleName = ui.GetByName("MiddleNameInputBox").OriginalText;
                    p.LastName = ui.GetByName("LastNameInputBox").OriginalText;
                    p.BirthYear = Functions.ToDatetime(ui.GetByName("DateInputBox").OriginalText);
                    p.OldGroup = p.Group;
                    p.Group = ui.GetByName("GroupInputBox").OriginalText;
                    p.Type = ui.GetByName("TypeSelect").AddedText;

                    e.Element = p;
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
