using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class ListView : UI
    {
        public TextLine Header { get; set; }
        public TextLine Footer { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public List<dynamic> Items;
        public int ItemsLength = 0;
        public int PageLength = 1;
        public int TotalPages = 0;
        public int CurrentPage = 0;
        public int CurrentItemOnPage = 0;
        public int CurrentItemGlobal = 0;
        public List<string> Options { get; set; }
        public bool Selected = false;

        public ListView(string name, (int, int) pos, (int, int) size)
        {
            Selectable = true;
            Name = name;
            (X, Y) = pos;
            (W, H) = size;
            PageLength = H - 2;

            Header = new TextLine("Header", "Use up/down keys to select, left/right to change pages", (X, Y), W, 0);
            for (int i = 0; i <= PageLength; i++)
            {
                ListSelect e = new ListSelect($"{Name}_{i}", "", (X, Y + i + 1), W);
                e.Selectable = true;
                Elements.Add(e);
            }
            Footer = new TextLine("Footer", "", (X, Y + H), W, 0);
            Options = new List<string>() { "" };
        }

        public ListView() {
            Selectable = true;
        }

        public void SetItems(List<dynamic> items, bool create_ui = true)
        {
            Items = items;
            ItemsLength = items.Count();
            PageLength = H - 2;
            TotalPages = (ItemsLength - 1) / PageLength;
            if (create_ui)
            {
                for (int i = 0; i <= PageLength; i++)
                {
                    ListSelect e = new ListSelect($"{Name}_{i}", "", (X, Y + i + 1), W);
                    e.Selectable = true;
                    Elements.Add(e);
                }
                SetOptions();
            }
        }

        public void SetPage(int page)
        {
            CurrentPage = Functions.ClipInt(page, 0, TotalPages);
            CurrentItemGlobal = CurrentPage * TotalPages;
            Update();
        }

        public void SetOptions()
        {
            foreach (dynamic e in Elements)
                if (e.GetType() == typeof(ListSelect))
                    e.Options = Options;
        }

        public void SetOptions(List<string> options)
        {
            Options = options;
            SetOptions();
        }

        public override void Update(bool fulldraw = true, bool clear = true)
        {
            PageLength = H - 2;
            int start_index = CurrentPage * PageLength;
            for (int i = 0; i <= PageLength; i++)
            {
                string v = GetValueAtIndex(start_index + i);
                string c = GetChoiceAtIndex(start_index + i);
                Elements[i].SetText(v);
                Elements[i].SetOptionSelect(c);
            }
            base.Update(fulldraw, false);
            if (fulldraw)
                SetOptions();
            Footer.SetText($"Page {CurrentPage} of {TotalPages}");
            Draw(true);
        }

        public override void Draw(bool reset = false)
        {
            base.Draw(reset);
            Header.Color = Selected ? 0 : 1;
            Footer.Color = Selected ? 0 : 1;
            Header.Draw();
            Footer.Draw();
        }

        public void Click()
        {
            Selected = false;
            ConsoleKeyInfo key;
            do
            {
                Draw();
                key = Console.ReadKey(true);
                SelectByKey(key);
            } while (key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Enter);
            Selected = true;
        }

        public virtual string GetValueAtIndex(int index)
        {
            if (Functions.IntInRange(index, 0, ItemsLength - 1))
                return Items[index][0].ToString();
            return "";
        }

        public virtual string GetChoiceAtIndex(int index)
        {
            if (Functions.IntInRange(index, 0, ItemsLength - 1))
                return Items[index][1].ToString();
            return "";
        }

        public virtual void SetChoiceAtIndex(int index, dynamic choice)
        {
            if (Functions.IntInRange(index, 0, ItemsLength - 1))
            {
                Items[index][1] = choice;
                Items[index][3] = true;
            }
        }

        public override void AddToValue(int i)
        {
            var e = Elements[SelectedButtonId];
            e.AddToValue(i);
            CurrentItemGlobal = CurrentPage * PageLength + SelectedButtonId;
            SetChoiceAtIndex(CurrentItemGlobal, e.AddedText);
        }

        public override int SelectPrevious()
        {
            SetPage(CurrentPage - 1);
            return 0;
        }

        public override int SelectNext()
        {
            SetPage(CurrentPage + 1);
            return 0;
        }

        public override int SelectVertical(bool up = true)
        {
            if (up) return base.SelectPrevious();
            else return base.SelectNext();
        }
    }
}
