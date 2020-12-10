using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class AdminUserListScreen
    {
        public static void MainScreen(dynamic _)
        {
            // UI ui = DB.READ_JSON_UI("layout/AdminApprovalListScreen");

            UI ui = new UI();

            TextLine t; InputBox i; Button b; ListSelect l;

            // HEADER
            t = new TextLine("Header", "Admin Menu", (0, 0), 120, 0);
            ui.Elements.Add(t);


            // BUTTONS
            b = new Button("UserList", "List of users", (47, 5), (26, 3))
            {
                Style = 1,
                Color = 1,
            };
            ui.Elements.Add(b);

            /*b = new Button("Back", "Back", (47, 19), (26, 3))
            {
                Style = 1,
                Color = 2,
            };
            ui.Elements.Add(b);*/


            //DB.JSON_UI(ui, "layout/AdminApprovalListScreen");
        }
    }
}
