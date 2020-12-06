using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public class InputBox : TextLine
    {
        public bool Selected = false;
        public bool OnlySelectText = false;
        public bool InputValid { get; set; }
        public string AcceptedCharacters { get; set; }
        public List<string> ValidationType { get; set; }


        public InputBox(string name, string text, (int, int) pos, int length, int fit = -1)
        {
            Name = name;
            Fit = fit;
            L = length;
            SetText(text);
            (X, Y) = pos;
            Color = 1;
            Style = 0;
            Selectable = true;
            InputValid = true;
            AcceptedCharacters = @"[A-Za-z0-9А-Яа-я_@.#&+-]";
            ValidationType = new List<string>();
        }

        public InputBox()
        {
            SetText(new String('-', L));
            OriginalText = "";
            Y -= 1;
            Selectable = true;
            ValidationType = new List<string>();
        }

        public void Click()
        {
            string s = OriginalText;
            OnlySelectText = true;
            Regex reg = new Regex(AcceptedCharacters);
            Draw();
            do
            {
                SetInputCursor(s);

                char c = Console.ReadKey(true).KeyChar;
                if (c == '\b')
                {
                    if (s.Length != 0) s = s.Substring(0, s.Length - 1);
                }
                else if (c == '\n' || c == '\r') break;
                else if (reg.IsMatch(Convert.ToString(c))) s += c;

                InputValid = ValidateString(s);
                SetText(s);
                Draw();
            } while (s.Length <= L);
            OriginalText = s;
            OnlySelectText = false;
        }

        public void SetInputCursor(string s)
        {
            int l = s.Length;
            switch (Fit)
            {
                case 0:
                    Console.SetCursorPosition((X + X + L) / 2 + l / 2, Y + 1);
                    break;
                case 1:
                    Console.SetCursorPosition(X + L - 1, Y + 1);
                    break;
                default:
                    Console.SetCursorPosition(X + l, Y + 1);
                    break;
            }
        }

        public override void Draw()
        {
            Functions.SetColor(Color, OnlySelectText ? false : Selected);
            string c = Constants.Styles[Style, 0];
            for (int i = X + 1; i < X + L; i++) Functions.WriteAt(c, i, Y);
            for (int i = X + 1; i < X + L; i++) Functions.WriteAt(c, i, Y + 2);
            c = Constants.Styles[Style, 2];
            Functions.WriteAt(c, X, Y);
            Functions.WriteAt(c, X + L - 1, Y);
            Functions.WriteAt(c, X, Y + 2);
            Functions.WriteAt(c, X + L - 1, Y + 2);

            Functions.SetColor(InputValid ? Color : 2, Selected);
            Functions.WriteAt(Text, X, Y + 1);

        }

        public void AddToValue(int i) { }

        public bool ValidateString(string s)
        {
            bool res = true;
            foreach (string v in ValidationType)
            if (Constants.Validations.ContainsKey(v)) res = res && Constants.Validations[v](s);
            return res;
        }

        public void ValidateSelf()
        {
            bool res = true;
            foreach (string v in ValidationType)
                if (Constants.Validations.ContainsKey(v)) res = res && Constants.Validations[v](OriginalText);
            InputValid = res;
        }

        public bool IsValid() { return InputValid; }

        
    }
}
