using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Table : BasicElement
    {
        public int ElementSizeX { get; set; }
        public int ElementSizeY { get; set; }
        public List<Label> RowLabels = new List<Label>();
        public string RowProperty { get; set; }
        public List<Label> ColumnLabels = new List<Label>();
        public string ColumnProperty { get; set; }
        public List<string> Options { get; set; }

        public List<List<ChoiceMapElement>> Items = new List<List<ChoiceMapElement>>();
        public CellMatrix Elements = new CellMatrix(true);
        public List<Cell> RowLabelCells = new List<Cell>();
        public List<Cell> ColLabelCells = new List<Cell>();
        public Pos Selection = new Pos(0, 0);
        public Pos SelectionGlobal = new Pos(0, 0);
        public Pos PrevSelection = new Pos(0, 0);
        public Pos CurrentPos = new Pos(0, 0);
        public string Subject = "";
        public ulong CreatorId;

        public Table(string name, (int, int) pos, (int, int) size, int element_size = 5)
        {
            Name = name;
            (X, Y) = pos;
            (W, H) = size;
            ElementSizeX = element_size;
        }

        public void SetCredentials(string s, ulong id)
        {
            Subject = s;
            CreatorId = id;
        }

        public void SetOptions(List<string> options)
        {
            Options = options;
            foreach (List<Cell> l in Elements.Elements)
                foreach (Cell c in l)
                    c.Options = options;
        }

        public void GenerateCells(int element_size, int row_label_length, string text = "")
        {
            ElementSizeX = element_size;
            int size_x = (W - row_label_length) / element_size;
            Elements.CreateNull(size_x, H - 1);
            RowLabelCells.Clear();
            ColLabelCells.Clear();
            for (int row = 0; row < H - 1; row++)
            {
                for (int col = 0; col < size_x; col++)
                    Elements.Elements[row][col] = new Cell($"{row}_{col}",
                        text,
                        (X + row_label_length + col * element_size, Y + row + 1),
                        element_size)
                    { Fit = 0 };
            }
            for (int row = 0; row < H - 1; row++)
            {
                Cell c = new Cell($"{row}_Label",
                        text,
                        (X, Y + row + 1),
                        row_label_length);
                RowLabelCells.Add(c);
            }
            for (int col = 0; col < size_x; col++)
            {
                Cell c = new Cell($"Label_{col}",
                        text,
                        (X + row_label_length + col * element_size, Y),
                        row_label_length);
                ColLabelCells.Add(c);
            }
        }

        public void AddLabelsRow(List<Label> labels, string prop)
        {
            RowProperty = prop;
            RowLabels = labels;
        }

        public void AddDatesToCols(string subject, ulong creator_id)
        {
            SetCredentials(subject, creator_id);

            HashSet<DateTime> all_dates = new HashSet<DateTime>();

            foreach (Label row in RowLabels)
            {
                Person p = row.Item;
                MarkList? found = null;
                foreach (MarkList marks in p.SubjectMarks)
                {
                    if (marks.SubjectName == subject && marks.CreatorId == creator_id)
                    {
                        found = marks;
                        Console.WriteLine("Found");
                        break;
                    }
                }
                if (found.HasValue)
                {
                    foreach (DateTime tmp in found.Value.Marks.Keys)
                    {
                        all_dates.Add(tmp);
                    }
                }
            }

            int c = 0;
            foreach (DateTime date in all_dates)
            {
                ColumnLabels.Add(new Label(date, date.ToString("dd.MM"), c++));
            }
            ColumnLabels.Sort((x, y) =>
            {
                return x.Item.CompareTo(y.Item);
            });

        }

        public string SelectByKey(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.F5:
                    Update(true);
                    Draw(true);
                    break;

                case ConsoleKey.S:
                    MoveSelection(0, 1);
                    break;
                case ConsoleKey.W:
                    MoveSelection(0, -1);
                    break;
                case ConsoleKey.A:
                    MoveSelection(-1, 0);
                    break;
                case ConsoleKey.D:
                    MoveSelection(1, 0);
                    break;

                case ConsoleKey.K:
                    MoveView(0, 1);
                    break;
                case ConsoleKey.I:
                    MoveView(0, -1);
                    break;
                case ConsoleKey.J:
                    MoveView(-1, 0);
                    break;
                case ConsoleKey.L:
                    MoveView(1, 0);
                    break;

                case ConsoleKey.E:
                    AddToValue(1);
                    break;
                case ConsoleKey.Q:
                    AddToValue(-1);
                    break;
            }
            return Name;
        }

        public override void Update(bool fulldraw = true)
        {
            for (int row = CurrentPos.Y; row < Elements.H + CurrentPos.Y; row++)
            {
                Label cur_label = GetRowLabel(row);
                dynamic p = cur_label.Item;
                if (p == null)
                    continue;
                MarkList marks = p.GetMarks(Subject, CreatorId);

                for (int i = CurrentPos.X; i < CurrentPos.X + RowLabelCells.Count; i++)
                {
                    Label col_label = GetColLabel(i);
                    dynamic item = col_label.Item;
                    if (item == null)
                        continue;

                    Cell c = Elements.Get(new Pos(i - CurrentPos.X, row - CurrentPos.Y));
                    if (marks.Marks.ContainsKey(item)) 
                        c.SetOptionSelect(marks.Marks[item].Value);  
                    else
                        c.SetOptionSelect(" ");
                    c.Update();
                }
            }
            for (int row = 0; row < RowLabelCells.Count; row++)
            {
                Cell c = RowLabelCells[row];
                c.SetText(GetRowLabel(row + CurrentPos.Y).ToString());
            }
            for (int col = 0; col < ColLabelCells.Count; col++)
            {
                Cell c = ColLabelCells[col];
                c.SetText(GetColLabel(col + CurrentPos.X).ToString());
            }
            Draw(true);
        }

        public Label GetRowLabel(int index)
        {
            if (Functions.IntInRange(index, 0, RowLabels.Count - 1))
            {
                return RowLabels[index];
            }
            return new Label(null, "", -1);
        }

        public Label GetColLabel(int index)
        {
            if (Functions.IntInRange(index, 0, ColumnLabels.Count - 1))
            {
                return ColumnLabels[index];
            }
            return new Label(null, "", -1); ;
        }

        public override void Draw(bool reset = false)
        {
            if (reset)
            {
                foreach (Cell c in RowLabelCells) c.Draw(reset);
                foreach (Cell c in ColLabelCells) c.Draw(reset);
                foreach (List<Cell> l in Elements.Elements)
                    foreach (Cell c in l)
                        c.Draw(reset);
            }
            else
            {
                if (PrevSelection.Valid()) Elements.Get(PrevSelection).Draw(reset);
                if (Selection.Valid()) Elements.Get(Selection).Draw(reset);
            }
        }

        public override void AddToValue(int i)
        {
            Cell c = Elements.Get(Selection);
            c.AddToValue(i);
            string value = c.GetChoice();
            Label label_row = GetRowLabel(Selection.Y - CurrentPos.Y);
            Label label_col = GetColLabel(Selection.X - CurrentPos.X);
            Person p = label_row.Item;
            p.SetMark(Subject, CreatorId, value, label_col.Item);
        }

        public void MoveSelection(int x, int y)
        {
            Deselect(Selection);

            Selection += new Pos(x, y);
            Selection.Clip(ColumnLabels.Count - 1, RowLabels.Count - 1);
            SelectionGlobal = CurrentPos + Selection;

            Select(Selection);
        }

        public void MoveView(int x, int y)
        {
            CurrentPos += new Pos(x, y);
            MoveSelection(-x, -y);
            CurrentPos.Clip(ColumnLabels.Count - 1, RowLabels.Count - 1);
            Update();
        }

        public void Select(Pos pos)
        {
            Cell c = Elements.Get(pos);
            if (c != null)
                c.Selected = true;
        }

        public void Deselect(Pos pos)
        {
            PrevSelection = pos;
            Cell c = Elements.Get(pos);
            if (c != null)
                c.Selected = false;
        }
    }

    public class Cell : ListSelect
    {
        dynamic Item { get; set; }
        int Row { get; set; }
        int Col { get; set; }

        public Cell(string name, string text, (int, int) pos, int length) :
            base(name, text, pos, length)
        {  }

        public void Set(string value)
        {
            SetOptionSelect(value);
        }

        public string Get()
        {
            return AddedText;
        }
    }

    public struct Pos
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }

        internal bool Valid()
        {
            return X >= 0 && Y >= 0;
        }

        internal bool InRange(int row, int col)
        {
            return Functions.IntInRange(Y, 0, row) && Functions.IntInRange(X, 0, col);
        }

        internal void Clip(int max_x, int max_y)
        {
            X = Functions.ClipInt(X, 0, max_x);
            Y = Functions.ClipInt(Y, 0, max_y);
        }

        public static Pos operator +(Pos a) => a;
        public static Pos operator -(Pos a) => new Pos(-a.X, -a.Y);
        public static Pos operator +(Pos a, Pos b)
            => new Pos(a.X + b.X, a.Y + b.Y);
        public static Pos operator -(Pos a, Pos b)
            => a + (-b);
        public static Pos operator *(Pos a, Pos b)
            => new Pos(a.X * b.X, a.Y * b.Y);
        public static Pos operator /(Pos a, Pos b)
        {
            if (b.X == 0 || b.Y == 0)
            {
                throw new DivideByZeroException();
            }
            return new Pos(a.X / b.X, a.Y / b.Y);
        }

    }

    public struct CellMatrix
    {
        public List<List<Cell>> Elements { get; set; }
        public int W; public int H;

        public CellMatrix(bool _ = true)
        {
            Elements = new List<List<Cell>>();
            W = -1;
            H = -1;
        }

        internal void CreateNull(int w, int h)
        {
            for (int row = 0; row < h; row++) {
                Elements.Add(new List<Cell>());
                for (int col = 0; col < w; col++)
                    Elements[row].Add(null);
            }
            Update();
                
        }

        internal Cell Get(Pos pos)
        {
            Update();
            if (pos.InRange(W, H))
                return Elements[pos.Y][pos.X];
            else
                return null;
        }

        void Update()
        {
            if (Elements.Count != 0)
            {
                H = Elements.Count - 1;
                if (Elements[0].Count != 0)
                    W = Elements[0].Count - 1;
                else
                    W = -1;
            }
            else
            {
                H = -1;
                W = -1;
            }
        }
    }

    public struct Label
    {
        public dynamic Item { get; set; }
        string Show { get; set; }
        int Index { get; set; }

        public Label(dynamic item, string show, int index)
        {
            Item = item;
            Show = show;
            Index = index;
        }

        public override string ToString()
        {
            return Show;
        }
    }
}
