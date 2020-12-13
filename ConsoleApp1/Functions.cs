using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Globalization;

namespace ConsoleApp1
{
    internal class Functions
    {

        // CONSOLE
        public static bool WriteAt(string s, int x, int y)
        {
            if (x > Console.WindowWidth || y > Console.WindowWidth) return false;
            if (s != null)
                if (x + s.Length > Console.WindowWidth && x <= Console.WindowWidth) s = s.Substring(0, Console.WindowWidth - x);
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
                return true;
            }
            catch
            {
                // Console.Clear();
                // Console.WriteLine("ERROR in UI.WriteAt");
            }
            return false;
        }

        public static void SetColor(int i, bool reverse = false)
        {
            if (reverse)
            {
                Console.ForegroundColor = Constants.Colors[i, 1];
                Console.BackgroundColor = Constants.Colors[i, 0];
            }
            else
            {
                Console.ForegroundColor = Constants.Colors[i, 0];
                Console.BackgroundColor = Constants.Colors[i, 1];
            }
        }

        // HASH
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }


        // BASIC
        public static void Exit(object _)
        {
            System.Environment.Exit(0);
        }

        public static void Pass(object _) { }


        public static void WriteTest(object _)
        {
            Console.WriteLine("Test");
        }

        public static DateTime ToDatetime(string s)
        {
            DateTime ret;
            DateTime.TryParseExact(s, Constants.DatePresets, CultureInfo.InvariantCulture, DateTimeStyles.None, out ret);
            return ret;
        }

        public static int ClipInt(int input, int a, int b)
        {
            if (input < a) return a;
            else if (input > b) return b;
            else return input;
        }

        public static bool IntInRange(int input, int a, int b)
        {
            return input >= a && input <= b;
        }

        public static List<ChoiceMapElement> CreateChoiceMap(List<dynamic> input,
                                            string default_choice = null,
                                            string get_property = null)
        {
            List<ChoiceMapElement> res = new List<ChoiceMapElement>();
            foreach(dynamic element in input)
            {
                string choice = "";
                if (default_choice != null)
                    choice = default_choice;
                else if (HasProperty(element, get_property))
                    choice = element.GetType().GetProperty(get_property).GetValue(element, null);

                res.Add(new ChoiceMapElement(element, choice, choice));
            }
            return res;
        }

        public static List<int> FindInChoiceMap(List<ChoiceMapElement> map, string property, dynamic value)
        {
            List<int> res = new List<int>();
            
            for (int i = 0; i < map.Count; i++)
            {
                dynamic element = map[i].Element;
                if (HasProperty(element, property))
                {
                    dynamic v = element.GetType().GetProperty(property).GetValue(element, null);
                    string _v = v.ToString().ToLower();
                    string _value = value.ToString().ToLower();
                    if (_value.Length > 0 && _value[0] == '_')
                    {
                        _value = _value.Substring(1, value.Length - 1);
                        if (_v.Contains(_value))
                        {
                            res.Add(i);
                        }
                    }
                    else
                        if (_v == _value)
                    {
                        res.Add(i);
                    }
                }
            }

            return res;
        }

        public static bool HasProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

    }
}
