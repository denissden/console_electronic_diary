using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
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
        public bool Hidden { get; set; }
        public bool Selectable = false;


        public TextLine(string name, string text, (int, int) pos, int length, int fit = -1)
        {
            Name = name;
            Fit = fit;
            L = length;
            SetText(text);
            (X, Y) = pos;
            Color = 1;
            Style = 0;
            Hidden = false;
        }

        public TextLine() { }


        public void SetText(string s, bool set_original_text = true)
        {
            if (set_original_text)
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
                        l = 0; r = d;
                        break;
                }
                char c = Convert.ToChar(Constants.Styles[Style, 3]);
                Text = new String(c, l) + s + new String(c, r);
            }
        }

        public void AddText(string s)
        {
            if (s == "") s = "None";
            SetText(OriginalText + s, false);
        }

        public virtual void Draw()
        {
            if (Hidden) return;
            Functions.SetColor(Color);
            Functions.WriteAt(Text, X, Y);
        }

        public bool IsSelectable() { return Selectable; }

        public void Update() { SetText(OriginalText); }
    }
}
