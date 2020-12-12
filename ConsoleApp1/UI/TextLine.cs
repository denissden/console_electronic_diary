using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class TextLine : BasicElement
    {
        public int Fit { get; set; }
        public int Color { get; set; }
        public int Style { get; set; }
        public bool Hidden { get; set; }
        public string AddedText = "";


        public TextLine(string name, string text, (int, int) pos, int length, int fit = -1)
        {
            Name = name;
            Fit = fit;
            W = length;
            SetText(text);
            (X, Y) = pos;
            Color = 1;
            Style = 0;
            Hidden = false;
        }

        public TextLine() { }


        public override void SetText(string s, string added_text = "", bool set_original_text = true)
        {
            if (set_original_text)
                OriginalText = s;
            int length = s.Length + added_text.Length;
            if (length > W)
            {
                Text = s.Substring(0, W);
            }
            else
            {
                int d = W - length;
                int l, m, r; // left, mid, right
                switch (Fit)
                {
                    case 0:
                        l = d / 2; m = 0; r = (d + 1) / 2;
                        break;
                    case 1:
                        l = d; m = 0; r = 0;
                        break;
                    case 2:
                        l = 0; m = d; r = 0;
                        break;
                    default:
                        l = 0; m = 0; r = d;
                        break;
                }
                char c = Convert.ToChar(Constants.Styles[Style, 3]);
                Text = new String(c, l) + s + new String(c, m) + added_text + new String(c, r);
            }
        }

        public void AddText(string s = "")
        {
            if (s == "") s = "None";
            AddedText = s;
            SetText(OriginalText, s, false);
        }

        public override void Draw(bool _ = true)
        {
            if (Hidden) return;
            Functions.SetColor(Color);
            Functions.WriteAt(Text, X, Y);
        }

        public override void Update(bool _ = true) { SetText(OriginalText, AddedText, false); }
    }
}
