using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class UI_old
    {
        int WindowX = Console.WindowWidth, WindowY = Console.WindowHeight;
        Functions inp = new Functions();
        int StyleIndex = 0;

        public void SetStyle(int i)
        {
            StyleIndex = i;
        }

        public string Crop(string s, int l, int dots = 2)
        {
            if (l <= dots) dots = Functions.ClipInt(dots - s.Length, 0, 2);
            else if (l >= s.Length) dots = 0;
            else l -= dots;
            string res = s.Substring(0, Functions.ClipInt(l, 0, s.Length)) + new String('.', dots);
            int d = l - s.Length;
            if (d > 1) res = new String(Convert.ToChar(Constants.Styles[StyleIndex, 3]), d / 2) + res;
            return res;
        }

        public (int, int) Clip((int, int) pos)
        {
            (int x, int y) = pos;
            return (Functions.ClipInt(x, 0, WindowX), Functions.ClipInt(y, 0, WindowY));
        }
    }


    public class UI
    {
        public List<Button> Buttons { get; set; }
        public List<TextLine> TextLines { get; set; }

        public List<dynamic> Elements { get; set; }

        public int SelectedButtonId { get; set; }

        public UI()
        {
            Buttons = new List<Button>();
            TextLines = new List<TextLine>();
            Elements = new List<dynamic>();
            SelectedButtonId = -1;
        }

        public void Draw(bool reset = false)
        {
            foreach (dynamic o in Elements) o.Draw();
            /*foreach (Button o in Buttons) o.Draw();
            foreach (TextLine o in TextLines) o.Draw();*/
            if (SelectedButtonId != -1) Elements[SelectedButtonId].Draw();
        }

        public void Update()
        {
            foreach (dynamic o in Elements) o.Update();
        }

        public int GetByFirst(char c)
        {
            for (int i = 0; i < Elements.Count; i++)
                if (Elements[i].IsSelectable() && Char.ToLower(c) == Char.ToLower(Elements[i].Name[0])) return i;
            return -1;
        }

        public dynamic GetByName(string name)
        {
            foreach (dynamic o in Elements) if (o.Name == name) return o;
            return null;
        }

        public string SelectByKey(ConsoleKeyInfo key)
        {
            int id = SelectedButtonId;
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    return ClickSelected();

                case ConsoleKey.RightArrow:
                    id = SelectNext();
                    break;
                case ConsoleKey.LeftArrow:
                    id = SelectPrevious();
                    break;
                case ConsoleKey.UpArrow:
                    if (SelectedButtonId != -1)
                        Elements[SelectedButtonId].AddToValue(1);
                    break;
                case ConsoleKey.DownArrow:
                    if (SelectedButtonId != -1)
                        Elements[SelectedButtonId].AddToValue(-1);
                    break;

                default:
                    id = GetByFirst(key.KeyChar);
                    break;
            }

            SelectButton(id);
            return "";
        }

        public int SelectNext()
        {
            int i = SelectedButtonId;
            bool found = false;
            while (-1 <= i && i < Elements.Count - 1)
            {
                i++;
                dynamic e = Elements[i];
                if (e.IsSelectable())
                {
                    found = true;
                    break;
                }
            }
            if (found)
                return i;
            else return SelectedButtonId;
        }

        public int SelectPrevious()
        {
            int i = SelectedButtonId;
            bool found = false;
            while (1 <= i && i < Elements.Count)
            {
                i--;
                if (Elements[i].IsSelectable())
                {
                    found = true;
                    break;
                }
            }
            if (found)
                return i;
            else return SelectedButtonId;
        }

        public void SelectButton(int id)
        {
            if (SelectedButtonId != -1)
            {
                DeselectButton(SelectedButtonId);
            }
            try
            {
                Elements[id].Selected = true;
                SelectedButtonId = id;
            }
            catch { }
        }

        public void DeselectButton(int id)
        {
            Elements[id].Selected = false;
            try
            {
                Elements[id].Selected = false;
            }
            catch { }
        }

        public string ClickSelected()
        {
            if (SelectedButtonId != -1) {
                var e = Elements[SelectedButtonId];
                e.Click();
                return e.Name; 
            }
            return "";
        }

        public void ValidateAll()
        {
            foreach (dynamic o in Elements)
                if (o is InputBox) o.ValidateSelf();
                    
        }

        public bool AllValid()
        {
            bool ret = true;
            foreach (dynamic o in Elements)
                if (o is InputBox) ret = ret && o.IsValid();
            return ret;
        }

    }
}
