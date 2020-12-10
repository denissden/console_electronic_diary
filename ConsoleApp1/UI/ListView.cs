using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class ListView : UI
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public TextLine Footer { get; set; }
        public List<dynamic> Items { get; set; }
        public int ItemsLength { get; set; }
        public int PageLength { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int CurrentItemOnPage { get; set; }
        public int CurrentItemGlobal { get; set; }
        public string Name { get; set; }

        ListView(string name, (int, int) pos, (int, int) size)
        {
            Name = name;
            (X, Y) = pos;
            (W, H) = size;
            PageLength = W - 1;
            Footer = new TextLine();
        }

        public void SetPage(int page)
        {
            CurrentPage = Functions.ClipInt(page, 0, TotalPages);
            Update();
        }

        public override void Update(bool fulldraw = true)
        {

            base.Update();
        }
    }
}
