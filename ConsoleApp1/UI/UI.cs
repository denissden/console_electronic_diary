using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    public class UI : BasicElement
    {
        public List<dynamic> Elements;

        public int SelectedButtonId { get; set; }
        public int PrevSelectedButtonId { get; set; }

        public string InterceptsInput = "";
        public bool DoInterceptInput = false;

        public UI()
        {
            Elements = new List<dynamic>();
            SelectedButtonId = -1;
        }

        public override void Draw(bool reset = false)
        {
            if (reset)
            {
                foreach (dynamic o in Elements) o.Draw(reset);
                if (SelectedButtonId != -1) Elements[SelectedButtonId].Draw(reset);
            }
            else
            {
                if (PrevSelectedButtonId != -1) Elements[PrevSelectedButtonId].Draw(reset);
                if (SelectedButtonId != -1) Elements[SelectedButtonId].Draw(reset);
            }
        }

        public virtual void Update(bool fulldraw = true, bool clear = true)
        {
            foreach (dynamic o in Elements) o.Update();
            if (fulldraw) 
            {
                Functions.SetColor(1);
                if (clear)
                    Console.Clear();
                Draw(true); 
            }
        }

        public int GetByFirst(char c)
        {
            for (int i = 0; i < Elements.Count; i++)
                if (Elements[i].IsSelectable() && Char.ToLower(c) == Char.ToLower(Elements[i].Name[0])) return i;
            return -1;
        }

        public dynamic GetByName(string name)
        {
            foreach (dynamic o in Elements) if (o.Name == name) return o;
            return null;
        }

        public string GetName(int index)
        {
            if (Functions.IntInRange(index, 0, Elements.Count - 1))
                return Elements[index].Name;
            return "";
        }

        public virtual string SelectByKey(ConsoleKeyInfo key)
        {
            int id = SelectedButtonId;
            string ret;
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    ret = ClickSelected();
                    /*if (ret == InterceptsInput)
                        DoInterceptInput = true;*/

                    return ret;

                case ConsoleKey.F5:
                    Update(true, true);
                    Draw(true);
                    break;

                case ConsoleKey.DownArrow:
                    id = SelectVertical(false);
                    break;
                case ConsoleKey.RightArrow:
                    id = SelectNext();
                    break;
                case ConsoleKey.UpArrow:
                    id = SelectVertical(true);
                    break;
                case ConsoleKey.LeftArrow:
                    id = SelectPrevious();
                    break;
                case ConsoleKey.OemPlus:
                    if (SelectedButtonId != -1)
                        AddToValue(1);
                    break;
                case ConsoleKey.OemMinus:
                    if (SelectedButtonId != -1)
                        AddToValue(-1);
                    break;

                default:
                    if (DoInterceptInput) 
                    {
                        dynamic inter = GetByName(InterceptsInput);
                        ret = inter.SelectByKey(key);
                        if (ret == "StopIntercepting")
                            DoInterceptInput = false;
                        if (InterceptsInput != GetName(SelectedButtonId) && InterceptsInput != GetName(SelectedButtonId))
                            inter.Draw();
                        return ret;
                    }
                    else
                        id = GetByFirst(key.KeyChar);
                    break;
            }

            SelectButton(id);
            return "";
        }

        public override void AddToValue(int i)
        {
            Elements[SelectedButtonId].AddToValue(i);
        }

        public virtual int SelectNext()
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

        public virtual int SelectPrevious()
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

        public virtual int SelectVertical(bool up = true)
        {
            int id = SelectedButtonId;
            if (id == -1) return SelectNext();

            dynamic element = Elements[id];
            int idx = element.GetMiddleX();
            int idy = element.GetMiddleY();
            int closest_id = id; double closest_score = double.MaxValue;
            for (int i = 0; i < Elements.Count; i++)
            {
                dynamic e = Elements[i];
                if (e.IsSelectable())
                {
                    int x = e.GetMiddleX();
                    int y = e.GetMiddleY();
                    double score = Math.Abs(idx - x) + Math.Abs(idy - y) * 2;
                    
                    if ((y < idy && up) || (y > idy && !up))
                    if (score < closest_score)
                    {
                        closest_id = i;
                        closest_score = score;
                    }
                }
            }
            return closest_id;
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
            PrevSelectedButtonId = id;
            Elements[id].Selected = false;
        }

        public virtual string ClickSelected()
        {
            if (SelectedButtonId != -1) {
                var e = Elements[SelectedButtonId];
                e.Click();
                return e.Name; 
            }
            return "";
        }

        public void ValidateAll()
        {
            foreach (dynamic o in Elements)
                if (o is InputBox) o.ValidateSelf();
                    
        }

        public bool AllValid()
        {
            bool ret = true;
            foreach (dynamic o in Elements)
                if (o is InputBox) ret = ret && o.IsValid();
            return ret;
        }

    }
}
