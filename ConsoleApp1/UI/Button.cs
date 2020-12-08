using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
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
        Action<dynamic> OnClick = Functions.Pass;
        public string ActionType { get; set; }
        //string OnClick = "";
        public bool Selectable = true;

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
            ActionType = "default";
            ValueClipMax = int.MaxValue;
            ValueClipMin = int.MinValue;
        }

        public Button()
        {
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

        public virtual void SetText(string s, bool set_original_text = true)
        {
            if (set_original_text)
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

        public void AddText(string s)
        {
            if (s == "") s = "None";
            SetText(OriginalText + s, false);
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
                c = new String(Convert.ToChar(Constants.Styles[Style, 3]), Functions.ClipInt(W - 1, 0, 1000));
                for (int i = Y + 1; i < Y + H; i++)
                    Functions.WriteAt(c, X + 1, i);
            }

            Functions.WriteAt(Text, X + TextX, Y + H / 2);

        }

        public void SetOnClick(Action<dynamic> a)
        {
            OnClick = a;
        }

        public void SetOnClick(string s) { ActionType = s; }

        public void Click()
        {
            if (Constants.Actions.ContainsKey(ActionType)) 
                Constants.Actions[ActionType](1);
            else OnClick(null);
        }

        public virtual void AddToValue(int i)
        {
            if (Value.HasValue)
            {
                ;
                Value = Functions.ClipInt(Convert.ToInt32(Value) + i, ValueClipMin, ValueClipMax);
                SetText(Convert.ToString(Value));
            }
        }

        public bool HasValue() { return Value.HasValue; }

        public bool IsSelectable() { return Selectable; }

        public virtual void Update()
        {
            SetText(OriginalText);
        }
    }
}
