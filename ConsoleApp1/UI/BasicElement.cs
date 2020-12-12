using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class BasicElement
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public string OriginalText { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public bool Selectable = false;




        public int GetMiddleX() { return X + W / 2; }
        public int GetMiddleY() { return Y + H / 2; }
        public bool IsSelectable() { return Selectable; }

        public virtual void SetText(string s, string added_text = "", bool set_original_text = true)
        {
            if (set_original_text)
                OriginalText = s;
            Text = s + added_text;
        }

        public virtual void Update(bool fulldraw = true) { }

        public virtual void Draw(bool _ = true) { }

        public virtual void AddToValue(int i) { }


        public override string ToString()
        {
            return $"name:{Name} pos: {X} {Y} size: {W} {H}\n{OriginalText}";
        }
    }
}
