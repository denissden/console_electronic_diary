using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ConsoleApp1
{

    class Functions
    {
        public static void WriteAt(string s, int x, int y)
        {
            if (x > Console.WindowWidth || y > Console.WindowWidth) return;
            if (x + s.Length > Console.WindowWidth && x <= Console.WindowWidth) s = s.Substring(0, Console.WindowWidth - x);
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
            catch
            {
                // Console.Clear();
                // Console.WriteLine("ERROR in UI.WriteAt");
            }
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

        public static void Exit()
        {
            System.Environment.Exit(0);
        }

        public static void Pass() { }


        public static void WriteTest()
        {
            Console.WriteLine("Test");
        }

    }



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

        public int GetByFirst(char c)
        {
            for (int i = 0; i < Buttons.Count; i++)
                if (Char.ToLower(c) == Char.ToLower(Buttons[i].OriginalText[0])) return i;
            return -1;
        }

        public dynamic GetByName(string name)
        {
            foreach (dynamic o in Elements) if (o.Name == name) return o;
            return null;
        }

        public void SelectByKey(ConsoleKeyInfo key)
        {
            int id = SelectedButtonId;
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    ClickSelected();
                    break;

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

        public void ClickSelected()
        {
            if (SelectedButtonId != -1) Elements[SelectedButtonId].Click();
        }


    }

    public class Button
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        int TextX;
        public int? Value { get; set; }
        public int ValueClipMax { get; set; }
        public int ValueClipMin { get; set; }
        public string OriginalText { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public int Color { get; set; }
        public int Style { get; set; }
        public bool Fill { get; set; }
        public bool DrawBorder { get; set; }

        public bool Selected = false;
        Action OnClick = Functions.Pass;
        public int ActionId { get; set; }
        public string ActionType { get; set; }
        //string OnClick = "";
        public static readonly bool Selectable = true;

        public Button(string name, string text, (int, int) pos, (int, int) size, bool fill = true)
        {
            Name = name;
            OriginalText = text;
            (X, Y) = pos;
            Resize(size);
            Fill = fill;
            DrawBorder = true;
            Color = 1;
            Style = 0;
            ActionId = -1;
            ActionType = "default";
            ValueClipMax = int.MaxValue;
            ValueClipMin = int.MinValue;
        }

        public Button()
        {
            ActionId = -1;
            ActionType = "default";
        }

        public void Move((int, int) pos)
        {
            (X, Y) = pos;
        }

        public void Resize((int, int) size)
        {
            (W, H) = size;
            W--; H--;
            SetText(OriginalText);
        }

        public void SetText(string s)
        {
            OriginalText = s;
            if (s.Length > W - 4)
            {
                Text = s.Substring(0, W - 3);
                TextX = 2;
            }
            else
            {
                Text = s;
                int d = W - s.Length + 2;
                TextX = d / 2;
            }
        }

        public void Draw()
        {
            if (Color == 2 && !Selected)
                Functions.SetColor(1, Selected);
            else
                Functions.SetColor(Color, Selected);
            string c = Constants.Styles[Style, 0];


            if (DrawBorder)
            {
                for (int i = X + 1; i < X + W; i++) Functions.WriteAt(c, i, Y);
                for (int i = X + 1; i < X + W; i++) Functions.WriteAt(c, i, Y + H);
                c = Constants.Styles[Style, 1];
                for (int i = Y + 1; i < Y + H; i++) Functions.WriteAt(c, X, i);
                for (int i = Y + 1; i < Y + H; i++) Functions.WriteAt(c, X + W, i);
                c = Constants.Styles[Style, 2];
                Functions.WriteAt(c, X, Y);
                Functions.WriteAt(c, X + W, Y);
                Functions.WriteAt(c, X, Y + H);
                Functions.WriteAt(c, X + W, Y + H);
            }


            if (Fill)
            {
                c = new String(Convert.ToChar(Constants.Styles[Style, 3]), Input.ClipInt(W - 1, 0, 1000));
                for (int i = Y + 1; i < Y + H; i++)
                    Functions.WriteAt(c, X + 1, i);
            }

            Functions.WriteAt(Text, X + TextX, Y + H / 2);

        }

        public void SetOnClick(Action a)
        {
            OnClick = a;
        }

        public void SetOnClick(int a) { ActionId = a; }

        public void SetOnClick(string s) { ActionType = s; }

        public void SetOnClick(string s, int a)
        {
            ActionType = s;
            ActionId = a;
        }


        public void Click()
        {
            if (ActionId != -1 && ActionType != "") Constants.Actions[ActionType][ActionId]();
            else OnClick();
        }

        public void AddToValue(int i)
        {
            if (Value.HasValue)
            {;
                Value = Input.ClipInt(Convert.ToInt32(Value) + i, ValueClipMin, ValueClipMax);
                SetText(Convert.ToString(Value));
            }
        }

        public bool HasValue() { return Value.HasValue; }

        public bool IsSelectable() { return Selectable; }

        public void Update()
        {
            SetText(OriginalText);
        }
    }


    public class TextLine
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int L { get; set; }
        public string OriginalText { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public int Fit { get; set; }
        public int Color { get; set; }
        public int Style { get; set; }
        public static readonly bool Selectable = false;


        public TextLine(string name, string text, (int, int) pos, int length, int fit = -1)
        {
            Name = name;
            Fit = fit;
            L = length;
            SetText(text);
            (X, Y) = pos;
            Color = 1;
            Style = 0;
        }

        public TextLine() { }


        public void SetText(string s)
        {
            OriginalText = s;
            if (s.Length > L)
            {
                Text = s.Substring(0, L);
            }
            else
            {
                int d = L - s.Length;
                int l, r;
                switch (Fit)
                {
                    case 0:
                        l = d / 2; r = (d + 1) / 2;
                        break;
                    case 1:
                        l = d; r = 0;
                        break;
                    default:
                        l = 0; r = d / 2;
                        break;
                }
                char c = Convert.ToChar(Constants.Styles[Style, 3]);
                Text = new String(c, l) + s + new String(c, r);
            }
        }


        public void Draw()
        {
            Functions.SetColor(Color);
            Functions.WriteAt(Text, X, Y);
        }

        public bool IsSelectable() { return Selectable; }


    }


    public class InputBox : TextLine
    {
        public static new readonly bool Selectable = false;
        public bool Selected = false;

        public InputBox()
        {
            SetText(new String('-', L));
            OriginalText = "";
            Y -= 1;
        }
        
        public void Click()
        {
            
        }

        public override void Draw()
        {
            Functions.SetColor(Color);
            Functions.WriteAt(Text, X, Y);
        }

    }
}
