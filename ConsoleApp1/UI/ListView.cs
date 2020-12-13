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
        public List<ChoiceMapElement> Items;
        public int ItemsLength = 0;
        public int PageLength = 1;
        public int TotalPages = 0;
        public int CurrentPage = 0;
        public int CurrentItemOnPage = 0;
        public int CurrentItemGlobal = 0;
        public List<string> Options { get; set; }
        public string HiddenPropertyType { get; set; }
        public List<int> SearchResults = new List<int>();
        public int SearchResultIndex = 0;
        public string AddToValueAction = "";
        public string FooterInfo = "";

        public ListView(string name, (int, int) pos, (int, int) size)
        {
            Name = name;
            (X, Y) = pos;
            (W, H) = size;
            PageLength = H - 2;

            Header = new TextLine("Header", "Use up/down keys to select, left/right to change pages, period/comma to move between search results", (X, Y), W, 0);
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
            HiddenPropertyType = "";
        }

        public void SetItems(List<ChoiceMapElement> items, bool create_ui = true)
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
            SelectButton(CurrentItemGlobal);
            Update();
        }

        public void SetIndex(int index)
        {
            CurrentItemGlobal = Functions.ClipInt(index, 0, ItemsLength - 1);
            CurrentPage = CurrentItemGlobal / PageLength;
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

        public override string SelectByKey(ConsoleKeyInfo key)
        {
            int id = SelectedButtonId;
            bool update = false;
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    Console.WriteLine(SelectedButtonId);
                    return ClickSelected();

                case ConsoleKey.F5:
                    Update(true, true);
                    Draw(true);
                    break;

                case ConsoleKey.K:
                    id = SelectVertical(false);
                    break;
                case ConsoleKey.L:
                    id = SelectNext();
                    break;
                case ConsoleKey.J:
                    id = SelectVertical(true);
                    break;
                case ConsoleKey.H:
                    id = SelectPrevious();
                    break;
                case ConsoleKey.Oem6: // ]
                    if (SelectedButtonId != -1)
                        AddToValue(1);
                    return "UPDATE";
                case ConsoleKey.Oem4: // [
                    if (SelectedButtonId != -1)
                        AddToValue(-1);
                    return "UPDATE";
                case ConsoleKey.OemComma:
                    id = ChangeSearchResultIndex(-1);
                    update = true;
                    break;
                case ConsoleKey.OemPeriod:
                    id = ChangeSearchResultIndex(1);
                    update = true;
                    break;

                default:
                    id = GetByFirst(key.KeyChar);
                    break;
            }

            SelectButton(id);
            if (update)
            {
                Update();
            }
            return Name;
        }

        public override void Update(bool fulldraw = true, bool clear = true)
        {
            PageLength = H - 2;
            int start_index = CurrentPage * PageLength;
            for (int i = 0; i <= PageLength; i++)
            {
                dynamic element = Elements[i];
                int current_index = start_index + i;
                string v = GetValueAtIndex(current_index);
                string c = GetChoiceAtIndex(current_index);
                string h = GetHiddenProperty(current_index);
                element.SetText(v);
                element.SetOptionSelect(c);
                element.SetHiddenProperty(h);
                if (IsInSearchResults(current_index))
                    element.Color = 3;
                else
                    element.Color = 1;
            }
            base.Update(fulldraw, false);
            if (fulldraw)
                SetOptions();
            Footer.SetText($"Page {CurrentPage} of {TotalPages}", FooterInfo);
            Draw(true);
            SelectButton(CurrentItemGlobal - CurrentPage * PageLength);
        }

        public override void Draw(bool reset = false)
        {
            base.Draw(reset);
            Header.Selected = Selected;
            Footer.Selected = Selected;
            Header.Draw();
            Footer.Draw();
        }

        public string Click()
        {
            FooterInfo = "  Edit Mode";
            Header.Color = 4;
            Footer.Color = 4;
            Update();
            return Name;
            /*Selected = false;
            string clicked = "";
            ConsoleKeyInfo key;
            do
            {
                Draw(clicked == "UPDATE");
                key = Console.ReadKey(true);
                clicked = SelectByKey(key);
            } while (key.Key != ConsoleKey.Escape);
            Selected = true;*/
        }

        public virtual string GetValueAtIndex(int index)
        {
            if (Functions.IntInRange(index, 0, ItemsLength - 1))
                return Items[index].Element.ToString();
            return "";
        }

        public virtual string GetChoiceAtIndex(int index)
        {
            if (Functions.IntInRange(index, 0, ItemsLength - 1))
                return Items[index].State;
            return "";
        }

        public virtual string GetHiddenProperty(int index)
        {
            if (Functions.IntInRange(index, 0, ItemsLength - 1))
            {
                dynamic e = Items[index].Element;
                if (Functions.HasProperty(e, HiddenPropertyType))
                {
                    string ret = e.GetType().GetProperty(HiddenPropertyType).GetValue(e, null).ToString();
                    return ret;
                }
            }      
            return "";
        }

        public virtual void SetChoiceAtIndex(int index, dynamic choice)
        {
            if (Functions.IntInRange(index, 0, ItemsLength - 1))
            {
                ChoiceMapElement tmp = Items[index];
                tmp.State = choice;
                tmp.Changed = true;
                Items[index] = tmp;
            }
        }

        public bool IsInSearchResults(int index)
        {
            return SearchResults.Contains(index);
        }

        public override void AddToValue(int i)
        {
            CurrentItemGlobal = CurrentPage * PageLength + SelectedButtonId;
            var e = Elements[SelectedButtonId];
            if (Constants.Actions.ContainsKey(AddToValueAction)) 
            {
                ChoiceMapElement item = Items[CurrentItemGlobal];
                Constants.Actions[AddToValueAction](item);
                dynamic res = OutputStack.Pop();
                
                Items[CurrentItemGlobal] = res;
                Update();
            }
            else
            {
                e.AddToValue(i);
                SetChoiceAtIndex(CurrentItemGlobal, e.AddedText);
            }
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

        public int ChangeSearchResultIndex(int index)
        {
            int l = SearchResults.Count;
            if (l != 0)
            {
                SearchResultIndex = Functions.ClipInt(SearchResultIndex + index, 0, l - 1);
                SetIndex(SearchResults[SearchResultIndex]);
                return CurrentItemGlobal - CurrentPage * PageLength;
            }
            return 0;
        }

        public string Search(string query, string property)
        {
            List<int> result = Functions.FindInChoiceMap(Items, property, query);
            SearchResults = result;
            int c = result.Count;
            if (c > 0)
            {
                SetIndex(result[0]);
            }
            return c.ToString();
        }

        public void Sort(string property)
        {
            bool reverse = false;
            int l = property.Length;
            if (l > 0 && property[l - 1] == '+')
            {
                reverse = true;
                property = property.Substring(0, l - 1);
            }
                
            Items.Sort((x, y) => 
            {
                dynamic x_e = x.Element;
                dynamic y_e = y.Element;
                if (Functions.HasProperty(x_e, property) && Functions.HasProperty(y_e, property))
                {
                    dynamic x_p = x_e.GetType().GetProperty(property).GetValue(x_e, null);
                    dynamic y_p = y_e.GetType().GetProperty(property).GetValue(y_e, null);
                    int c;
                    if (reverse)
                        c = y_p.CompareTo(x_p);
                    else 
                        c = x_p.CompareTo(y_p);
                    return c;
                }
                return 0;
            });
            SearchResults.Clear();
            SearchResultIndex = 0;
            Update(true);
        }

        public void UnchangeAll()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                ChoiceMapElement tmp = Items[i];
                tmp.Changed = false;
                Items[i] = tmp;
            }
        }
    }
}
