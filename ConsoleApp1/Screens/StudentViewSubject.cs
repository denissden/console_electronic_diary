using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class StudentViewSubject
    {
        public static void MainScreen(dynamic m)
        {
            MarkList marks = m.Element;

            UI ui = new UI();

            TextLine t; InputBox i; Button b; ListView lv;

            if(marks.Marks.Count == 0)
            {
                WarningScreen.Error("You have no marks in this subject!");
                return;
            }

            // HEADER
            t = new TextLine("Header", "", (0, 0), 120, 0);
            ui.Elements.Add(t);

            lv = new ListView("SubjectList", (0, 2), (120, 24));
            lv.SetOptions(new List<string>() { " " });
            ui.Elements.Add(lv);

            b = new Button("Back", "Back", (47, 27), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);


            ui.InterceptsInput = "SubjectList";
            ui.DoInterceptInput = true;

            ui.GetByName("Header").SetText(marks.SubjectName);

            List<dynamic> marks_show = new List<dynamic>();
            foreach (Mark mark in marks.Marks.Values)
            {
                marks_show.Add(new SimplifiedMark(mark.Value, mark.Date));
            }

            List<ChoiceMapElement> groups_map = Functions.CreateChoiceMap(marks_show, get_property: "Value");

            dynamic subjectlist = ui.GetByName("SubjectList");
            subjectlist.SetOptions(new List<string> { " ", "H", "2", "3", "4", "5" });
            subjectlist.SetItems(groups_map, false);
            subjectlist.AllowEdit = false;

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
        }
    }
}
