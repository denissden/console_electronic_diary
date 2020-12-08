using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class ListSelect : Button
    {
        public List<string> Options { get; set; }
        public bool OptionsUpdated { get; set; }
        public int Fit { get; set; }


        public ListSelect(string name, string text, (int, int) pos, (int, int) size, bool fill = true)
        {
            Name = name;
            OriginalText = text;
            Fit = 0;
            (X, Y) = pos;
            Resize(size);
            Fill = fill;
            DrawBorder = true;
            Color = 1;
            Style = 0;
            ActionType = "default";
            Options = new List<string>() { "" };
            OptionsUpdated = true;
            Value = 0;
            ValueClipMax = int.MaxValue;
            ValueClipMin = 0;
        }

        public ListSelect() 
        {
            ValueClipMax = int.MaxValue;
            ValueClipMin = 0;
        }

        public override void Update()
        {
            AddText(Options[Value.HasValue ? Value.Value : 0]);
        }

        public override void AddToValue(int i)
        {
            ValueClipMax = Options.Count - 1;
            if (Value.HasValue)
            {
                Value = Functions.ClipInt(Convert.ToInt32(Value) + i, ValueClipMin, ValueClipMax);
                Update();
            }
        }

        public override void SetText(string s, bool set_original_text = true)
        {
            if (set_original_text)
                OriginalText = s;
            if (s.Length > W)
            {
                Text = s.Substring(0, W);
            }
            else
            {
                int d = W - s.Length;
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
                        l = 0; r = d;
                        break;
                }
                char c = Convert.ToChar(Constants.Styles[Style, 3]);
                Text = new String(c, l) + s + new String(c, r);
            }
        }
    }
}
