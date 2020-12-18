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

            // UI ui = DB.READ_JSON_UI("layout/TeacherScreen");

            UI ui = new UI();

            TextLine t; InputBox i; Button b; ListView lv;

            // HEADER
            t = new TextLine("Header", "Teacher Menu", (0, 0), 120, 0);
            ui.Elements.Add(t);

            lv = new ListView("SubjectList", (0, 4), (120, 22));
            lv.SetOptions(new List<string>() { "Unchanged", "Changed" });
            lv.AddToValueAction = "teacher_edit_group";
            ui.Elements.Add(lv);

            b = new Button("Back", "Log out", (47, 27), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);

            ui.InterceptsInput = "SubjectList";
            ui.DoInterceptInput = true;


            dynamic groups = p.Subjects;
            List<ChoiceMapElement> groups_map = Functions.CreateChoiceMap(groups, default_choice: "Unchanged");

            dynamic subjectlist = ui.GetByName("SubjectList");
            subjectlist.SetItems(groups_map, false);
            subjectlist.AddToValueAction = "teacher_edit_subject";
            subjectlist.PersonToStringOptions = "login,_date,id";

            ui.Update();
            //ui.ValidateAll();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);

                /*if (clicked == "Back") break;
                else if (clicked == "UserList") ui.Update();*/
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            //DB.JSON_PERSON(p, Constants.USERS_Path);
            //DB.JSON_UI(ui, "layout/TeacherScreen");
        }
    }
}
