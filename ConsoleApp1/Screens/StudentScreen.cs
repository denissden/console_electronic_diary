using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class StudentScreen
    {
        public static void MainScreen(dynamic login)
        {
            Person p = DB.READ_PERSON_BY_LOGIN<Person>(login);

            UI ui = new UI();

            TextLine t; InputBox i; Button b; ListView lv;


            // HEADER
            t = new TextLine("Header", "You are logged in as ", (0, 0), 120, 0);
            ui.Elements.Add(t);

            lv = new ListView("SubjectList", (0, 2), (120, 15));
            lv.SetOptions(new List<string>() { " " });
            ui.Elements.Add(lv);

            b = new Button("Back", "Log out", (47, 27), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);


            ui.InterceptsInput = "SubjectList";
            ui.DoInterceptInput = true;

            ui.GetByName("Header").AddText(p.GetShortName());

            List<ChoiceMapElement> groups_map = Functions.CreateChoiceMap(p.SubjectMarks, default_choice: " ");

            dynamic subjectlist = ui.GetByName("SubjectList");
            subjectlist.AddToValueAction = "student_view_subject";
            subjectlist.SetItems(groups_map, false);

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
                else if (clicked == "UPDATE") ui.Update();
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            //DB.JSON_UI(ui, "layout/TeacherScreen");
        }
    }
}

