using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Button : TextLine
    {
        public int? Value { get; set; }
        public int ValueClipMax { get; set; }
        public int ValueClipMin { get; set; }
        public bool Fill { get; set; }
        public bool DrawBorder { get; set; }

        public bool Selected = false;
        Action<dynamic> OnClick = Functions.Pass;
        public string ActionType { get; set; }
        //string OnClick = "";

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
            Selectable = true;
            DrawBorder = true;
        }

        public Button()
        {
            ActionType = "default";
            Selectable = true;
            DrawBorder = true;
        }

        public void Resize((int, int) size)
        {
            (W, H) = size;
            W--; H--;
            SetText(OriginalText);
        }

        public override void SetText(string s, string added_text = "", bool set_original_text = true)
        {
            W++;
            base.SetText(s, added_text, set_original_text);
            W--;
        }

        public override void Draw()
        {
            if (Color == 2 && !Selected)
                Functions.SetColor(1, Selected);
            else
                Functions.SetColor(Color, Selected);

            string c;

            if (Fill)
            {
                c = new String(Convert.ToChar(Constants.Styles[Style, 3]), Functions.ClipInt(W - 1, 0, 1000));
                for (int i = Y + 1; i < Y + H; i++)
                    Functions.WriteAt(c, X + 1, i);
            }

            Functions.WriteAt(Text, X, (Y + Y + H) / 2);

            if (DrawBorder)
            {
                c = Constants.Styles[Style, 0];
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
        }

        public void SetOnClick(Action<dynamic> a)
        {
            OnClick = a;
        }

        public void SetOnClick(string s) { ActionType = s; }

        public override void Update()
        {
            SetText(OriginalText);
        }

        public void Click()
        {
            if (Constants.Actions.ContainsKey(ActionType)) 
                Constants.Actions[ActionType](1);
            else OnClick(null);
        }

        public override void AddToValue(int i)
        {
            if (Value.HasValue)
            {
                ;
                Value = Functions.ClipInt(Convert.ToInt32(Value) + i, ValueClipMin, ValueClipMax);
                SetText(Convert.ToString(Value));
            }
        }

        public bool HasValue() { return Value.HasValue; }
    }
}
