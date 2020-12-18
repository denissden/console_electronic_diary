using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Constants
    {
        public static readonly string DB_Path = @"D:\_C# projects\Elektronniy_Dnevnik\ConsoleApp1\data\";
        // public static readonly string DB_Path = @"C:\Users\user\Desktop\data\";
        public static readonly string USERS_Path = @"users\";
        public static readonly string IDS_Path = @"id_lib\";
        public static readonly string GROUPS_Path = @"groups\";
        public static readonly string[,] Styles = { { "-", "|", "+", " " }, { "=", " ", "#", " " }, { "#", "#", "#", "-" } };
        public static readonly ConsoleColor[,] Colors = {
            { ConsoleColor.Black, ConsoleColor.White },
            { ConsoleColor.White, ConsoleColor.Black },
            { ConsoleColor.Red, ConsoleColor.DarkRed },
            { ConsoleColor.Black, ConsoleColor.Green },
            { ConsoleColor.DarkMagenta, ConsoleColor.Black },
        };
        public static readonly Dictionary<string, ConsoleColor> RoleColors = new Dictionary<string, ConsoleColor> {
            { "Guest", ConsoleColor.DarkGray },
            { "Student", ConsoleColor.Cyan },
            { "Teacher", ConsoleColor.Yellow },
            { "Admin", ConsoleColor.Red},
            { "Unchanged", ConsoleColor.DarkGray},
            { "Changed", ConsoleColor.DarkMagenta},
            { "Removed", ConsoleColor.DarkRed},
        };
        public static readonly List<string> ColoringOrder = new List<string>() {
            "Unchanged",
            "Guest",
            "Student",
            "Teacher",
            "Admin",
            "Changed",
            "Removed",
        };
        public static readonly List<string> UserTypes = new List<string>() { "Guest", "Student", "Teacher", "Admin" };

        public static readonly string[] DatePresets = new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd.MM.yyyy", "dd MM yyyy" };


        public static readonly Dictionary<string, Action<dynamic>> Actions = new Dictionary<string, Action<dynamic>>()
        {
            {"default", Functions.Pass },
            {"exit", Functions.Exit },
            {"test", Functions.WriteTest },
            {"start_screen", StartScreen.MainScreen },
            {"new_account", NewAccountScreen.MainScreen },
            {"login", LoginScreen.MainScreen },
            {"guest_screen", GuestScreen.MainScreen },
            {"admin_screen", AdminScreen.MainScreen },
            {"admin_user_list", AdminUserListScreen.MainScreen },
            {"admin_approval_list", AdminApprovalListScreen.MainScreen },
            {"admin_edit_user", AdminEditUser.MainScreen },
            {"admin_edit_groups", AdminEditGroups.MainScreen },
            {"admin_edit_group", AdminEditGroup.MainScreen },
            {"teacher_screen", TeacherScreen.MainScreen },
            {"teacher_edit_subject", TeacherEditSubject.MainScreen },
            {"teacher_edit_group", TeacherEditGroup.MainScreen },
        };

        public static readonly Dictionary<string, Func<string, bool>> Validations = new Dictionary<string, Func<string, bool>>()
        {
            {"any", Validation.True},
            {"none", Validation.False},
            {"longer8", Validation.Longer8},
            {"longer3", Validation.Longer3},
            {"nonempty", Validation.NonEmpty},
            {"shorter30", Validation.Shorter30},
            {"shorter20", Validation.Shorter20},
            {"shorter10", Validation.Shorter10},
            {"date", Validation.Date},
            {"group", Validation.GroupExists},
            {"login", Validation.NewLogin},
            {"password", Validation.Password},
            {"passwords_match1", Validation.PasswordsMatchAdd},
            {"passwords_match2", Validation.PasswordsMatchCheck},
            {"login_exists", Validation.LoginExists},
            {"person_has_property", Validation.PersonHasProperty},
            {"person_can_display", Validation.PersonCanDisplay},
            {"new_group", Validation.NewGroup},
        };
        public static readonly Dictionary<string, Action<dynamic>> UserMainScreens = new Dictionary<string, Action<dynamic>>()
        {
            {"Guest", GuestScreen.MainScreen },
            {"Student", Functions.Pass },
            {"Teacher", TeacherScreen.MainScreen },
            {"Admin", AdminScreen.MainScreen },
        };

        public static readonly bool SkipVerificationAtAccountCreation = true;
        public static readonly int GroupDisplayInListViewMaxLength = 100;
        
    }


    class Program
    {
        static void Main(string[] args) // 120 x 30
        {
            //Test.Run3();
            // Console.ReadKey();
            //GenerateUsers.Generate(0);
            //Console.ReadKey();
            AdminScreen.MainScreen("admin");
            //TeacherScreen.MainScreen("Teacher");
            Console.WriteLine(Constants.SkipVerificationAtAccountCreation);
            StartScreen.MainScreen(null);

            Console.ReadKey(true);
        }
    }
}
