using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class TeacherEditSubject
    {
        public static void MainScreen(dynamic e)
        {
            dynamic s = e.Element;
            List<GroupContainer> groups = new List<GroupContainer>();
            foreach (string g in s.S.Groups)
            {
                groups.Add(new GroupContainer(g, s.S.Name, s.CreatorId));
            }

            // UI ui = DB.READ_JSON_UI("layout/TeacherEditSubject");

            UI ui = new UI();

            TextLine t; InputBox i; Button b; ListView lv;

            // HEADER
            t = new TextLine("Header", "Teacher Menu", (0, 0), 120, 0);
            ui.Elements.Add(t);

            lv = new ListView("GroupList", (0, 2), (120, 24));
            lv.SetOptions(new List<string>() { "Unchanged", "Changed" });
            lv.AddToValueAction = "teacher_edit_group";
            ui.Elements.Add(lv);

            b = new Button("Back", "Back", (47, 27), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);


            ui.GetByName("Header").SetText(s.S.Name);

            ui.InterceptsInput = "GroupList";
            ui.DoInterceptInput = true;

            List<ChoiceMapElement> groups_map = Functions.CreateChoiceMap(groups, default_choice: " ");

            dynamic grouplist = ui.GetByName("GroupList");
            grouplist.SetItems(groups_map, false);
            grouplist.AddToValueAction = "teacher_edit_group";
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

                if (clicked == "Back") break;
                else if (clicked == "UPDATE") ui.Update();
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();
        }
    }
}
