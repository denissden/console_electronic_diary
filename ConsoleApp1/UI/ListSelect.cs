using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ListSelect : Button
    {
        public List<string> Options { get; set; }
        public string HiddenProperty;
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
            SetText(OriginalText);
            AddText(Value.HasValue && Value != -1 ? Options[Value.Value] : " ");
        }

        public void SetOptionSelect(string option, bool set_default = false)
        {
            Value = Options.IndexOf(option);
            if (Value == -1 && set_default)
                Value = 0;
        }

        public void SetHiddenProperty(string property)
        {
            HiddenProperty = property;
        }

        public override void Draw(bool _ = true)
        {
            if (Hidden) return;
            Functions.SetColor(Color, Selected);
            int a_i = Constants.ColoringOrder.IndexOf(AddedText);
            int h_i = Constants.ColoringOrder.IndexOf(HiddenProperty);

            if (a_i != -1 || h_i != -1)
                if (Selected)
                    Console.BackgroundColor = Constants.RoleColors[a_i > h_i ? AddedText : HiddenProperty];
                else
                    Console.ForegroundColor = Constants.RoleColors[a_i > h_i ? AddedText : HiddenProperty];

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

        public string GetChoice()
        {
            return Options[Value.HasValue && Value != -1 ? Value.Value : 0];
        }

        public override void Click()
        {
            //Functions.WriteAt(HiddenProperty, 0, 32);
        }
    }
}
