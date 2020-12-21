using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class TeacherAddSubject
    {
        public static void MainScreen(dynamic p)
        {
            List<string> has_subjects = new List<string>();
            foreach (Subject s in p.Subjects)
                has_subjects.Add(s.Name);

            Validation.IsInListValidation = has_subjects;

            UI ui = DB.READ_JSON_UI("layout/TeacherAddSubject");

            ui.InterceptsInput = "GroupList";
            ui.DoInterceptInput = true;

            dynamic groups = DB.LOAD_ALL_GROUPS();
            List<ChoiceMapElement> groups_map = Functions.CreateChoiceMap(groups, default_choice: "Do Not Add");

            dynamic grouplist = ui.GetByName("GroupList");
            grouplist.SetItems(groups_map, true);

            ui.Update();
            ui.ValidateAll();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "Create")
                {
                    if (ui.AllValid())
                    {
                        string name = ui.GetByName("SubjectInputBox").OriginalText;
                        bool yes = WarningScreen.NoYes($"Are you sure you want to create subject \"{name}\"?");
                        if (!yes) { ui.Update(); continue; }

                        Subject s = new Subject(name);
                        foreach (ChoiceMapElement e in grouplist.Items)
                        {
                            if (e.Changed && e.State != e.InitState)
                            {   
                                s.Groups.Add(e.Element.Name);
                                DB.ADD_SUBJECT_TO_GROUP(e.Element, name, p.Id);
                            }
                        }
                        OutputStack.Push(s);
                        break;
                    }
                }
                else if (clicked == "Back")
                {
                    bool yes = WarningScreen.NoYes("All changes will be lost. Are you sure?");
                    if (yes) break;
                    else ui.Update();
                }
                else if (clicked == "UPDATE") ui.Update();
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            //DB.JSON_UI(ui, "layout/TeacherAddSubject");
        }
    }
}
