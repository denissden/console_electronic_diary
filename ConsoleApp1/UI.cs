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
        Input inp = new Input();
        int StyleIndex = 0;


        public void DrawBorder((int, int) pos, (int, int) size, bool fill = true)
        {
            (int x, int y) = Clip(pos);
            (int w, int h) = size;
            w--; h--;


            string c = Constants.Styles[StyleIndex, 0];
            for (int i = x + 1; i < x + w; i++) Functions.WriteAt(c, i, y);
            for (int i = x + 1; i < x + w; i++) Functions.WriteAt(c, i, y + h);
            c = Constants.Styles[StyleIndex, 1];
            for (int i = y + 1; i < y + h; i++) Functions.WriteAt(c, x, i);
            for (int i = y + 1; i < y + h; i++) Functions.WriteAt(c, x + w, i);
            c = Constants.Styles[StyleIndex, 2];
            Functions.WriteAt(c, x, y);
            Functions.WriteAt(c, x + w, y);
            Functions.WriteAt(c, x, y + h);
            Functions.WriteAt(c, x + w, y + h);

            if (fill)
            {
                c = new String(Convert.ToChar(Constants.Styles[StyleIndex, 3]), w - 1);
                for (int i = y + 1; i < y + h; i++)
                    Functions.WriteAt(c, x + 1, i);
            }



        }

        public void DrawButton(string text, (int, int) pos, (int, int) size, int maxw = -1)
        {
            DrawBorder(pos, size);
            (int w, int h) = size;
            (int x, int y) = Clip(pos);
            if (maxw != -1) w = Input.ClipInt(w - 4, 0, maxw);

            int c = w - 4;
            if (c + x > WindowX) c = w + x - WindowX;
            if (StyleIndex == 1)
            {
                text = Crop(text, c + 2);
                Functions.WriteAt(text, x + 1, y + h / 2);
            }
            else
            {
                text = Crop(text, c);
                Functions.WriteAt(text, x + 2, y + h / 2);
            }

            // for (int i = x + 2; i < x + w - 3; i++) WriteAt("|", i, y + w / 2 + 1);
        }

        public void SetStyle(int i)
        {
            StyleIndex = i;
        }

        public string Crop(string s, int l, int dots = 2)
        {
            if (l <= dots) dots = Input.ClipInt(dots - s.Length, 0, 2);
            else if (l >= s.Length) dots = 0;
            else l -= dots;
            string res = s.Substring(0, Input.ClipInt(l, 0, s.Length)) + new String('.', dots);
            int d = l - s.Length;
            if (d > 1) res = new String(Convert.ToChar(Constants.Styles[StyleIndex, 3]), d / 2) + res;
            return res;
        }

        public (int, int) Clip((int, int) pos)
        {
            (int x, int y) = pos;
            return (Input.ClipInt(x, 0, WindowX), Input.ClipInt(y, 0, WindowY));
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
