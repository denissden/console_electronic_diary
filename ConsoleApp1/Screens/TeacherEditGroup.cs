using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    // here teacher edits marks
    class TeacherEditGroup
    {
        public static void MainScreen(dynamic e)
        {
            GroupContainer container = e.Element;
            Group group = DB.READ_GROUP(container.Name);
            if (group == null)
            {
                WarningScreen.Error("The group doesn't exist anymore!");
                return;
            }

            List<Person> people = DB.LOAD_PEOPLE_FROM_GROUP(group);
            people.Sort((x, y) =>
            {
                return x.ToString().CompareTo(y.ToString());
            });

            // UI ui = DB.READ_JSON_UI("layout/TeacherEditSubject");

            UI ui = new UI();

            TextLine t; InputBox i; Button b; Table tb;

            // HEADER
            t = new TextLine("Header", "Teacher Menu", (0, 0), 120, 0);
            ui.Elements.Add(t);

            tb = new Table("MarkTable", (0, 1), (120, 25));
            ui.Elements.Add(tb);

            b = new Button("Exit", "Exit", (20, 27), (20, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);

            b = new Button("Save", "Save", (43, 27), (20, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);

            // DATE
            t = new TextLine("DateInputLabel", "Date (DD MM YYYY)", (66, 26), 20, 0);
            ui.Elements.Add(t);
            i = new InputBox("DateInputBox", "", (66, 27), 20, 0)
            {
                Style = 1,
                ValidationType = new List<string>() { "shorter20", "date", "nonempty" },
            };
            ui.Elements.Add(i);

            b = new Button("AddDate", "Add Date", (89, 27), (20, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);


            ui.GetByName("Header").SetText(container.Name);

            ui.InterceptsInput = "MarkTable";
            ui.DoInterceptInput = true;

            List<Label> RowLabels = new List<Label>();

            int c = 0;
            foreach (Person p in people)
            {
                Label l = new Label(p, p.ToString(), c++);
                RowLabels.Add(l);
            }

            dynamic table = ui.GetByName("MarkTable");
            table.GenerateCells(7, 20, "");
            table.SetOptions(new List<string> { " ", "H", "2", "3", "4", "5" });
            table.AddLabelsRow(RowLabels, "SubjectMarks");
            table.AddDatesToCols(container.SubjectName, container.CreatorId);
            Console.WriteLine(table.Elements.Elements.Count);
            table.Update();
            table.Draw(true);

            ui.ValidateAll();
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
                    bool yes = WarningScreen.NoYes("All changes will be lost. Are you sure?");
                    if (yes) break;
                    ui.Update();
                }
                else if (clicked == "Save")
                {
                    DB.SAVE_TABLE(table);
                }
                else if (clicked == "AddDate")
                {
                    if (!ui.GetByName("DateInputBox").IsValid())
                        continue;
                    DateTime date = Functions.ToDatetime(ui.GetByName("DateInputBox").OriginalText);
                    Label l = new Label(date, date.ToString("dd.MM"), -1);
                    table.ColumnLabels.Add(l);
                    table.Update();
                }
                else if (clicked == "UPDATE") ui.Update();
            } while (key.Key != ConsoleKey.Escape);
        }
    }
}
