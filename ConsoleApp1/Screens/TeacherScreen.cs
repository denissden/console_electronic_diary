using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class TeacherScreen
    {
        public static void MainScreen(dynamic login)
        {
            Person p = DB.READ_PERSON_BY_LOGIN<Person>(login);

            UI ui = DB.READ_JSON_UI("layout/TeacherScreen");

            ui.InterceptsInput = "SubjectList";
            ui.DoInterceptInput = true;


            dynamic subjects = new List<SubjectContainer>();
            foreach (Subject s in p.Subjects)
            {
                subjects.Add(new SubjectContainer(s, p.Id));
            }
            List<ChoiceMapElement> groups_map = Functions.CreateChoiceMap(subjects, default_choice: " ");

            dynamic subjectlist = ui.GetByName("SubjectList");
            subjectlist.SetItems(groups_map, true);
            subjectlist.AddToValueAction = "teacher_edit_subject";

            ui.Update();
            //ui.ValidateAll();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                if (clicked == "Back") break;
                else if (clicked == "AddSubject")
                {
                    TeacherAddSubject.MainScreen(p);
                    Subject? tmp = OutputStack.Pop();
                    if (tmp.HasValue)
                    {
                        p.Subjects.Add(tmp.Value);
                        DB.JSON_PERSON(p, Constants.USERS_Path);
                        subjectlist.Add(new ChoiceMapElement(new SubjectContainer(tmp.Value, p.Id), " ", " "));
                    }
                    ui.Update();
                }
                else if (clicked == "UPDATE") ui.Update();
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            //DB.JSON_UI(ui, "layout/TeacherScreen");
        }
    }
}
