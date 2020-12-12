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

        public ListSelect(string name, string text, (int, int) pos, int length, bool fill = true)
        {
            Name = name;
            OriginalText = text;
            Fit = 2;
            (X, Y) = pos;
            Resize((length, 1));
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

        public override void Update(bool fulldraw = true)
        {
            AddText(Options[Value.HasValue && Value != -1 ? Value.Value : 0]);
        }

        public void SetOptionSelect(string option)
        {
            Value = Options.IndexOf(option);
        }

        public override void Draw(bool _ = true)
        {
            if (Hidden) return;
            Functions.SetColor(Color, Selected);
            if (Constants.RoleColors.ContainsKey(AddedText))
                if (Selected)
                {
                    Console.BackgroundColor = Constants.RoleColors[AddedText];
                }
                else
                {
                    Console.ForegroundColor = Constants.RoleColors[AddedText];
                }
            Functions.WriteAt(Text, X, Y);
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
    }
}
