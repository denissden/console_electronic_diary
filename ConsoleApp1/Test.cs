using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Test
    {
        public static void Run()
        {
            //UI ui = DB.READ_JSON_UI("layout/Login");

            UI ui = new UI();

            Button b = new Button("B", "0", (47, 15), (26, 3))
            {
                Style = 0,
                Color = 1,
                ValueClipMin = 10,
                Fill = false,
            };
            b.Value = 10;
            ui.Elements.Add(b);

            ui.Update();
            ui.ValidateAll();
            Functions.SetColor(1);
            Console.Clear();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                Functions.SetColor(1);
                Console.Clear();
                ui.Draw();
                key = Console.ReadKey(true);
                clicked = ui.SelectByKey(key);
                dynamic bu = ui.GetByName("B");
                int x = Convert.ToInt32(bu.Value);
                bu.W = x;
                bu.Update();
            } while (key.Key != ConsoleKey.Escape);

            Functions.SetColor(1);
            Console.Clear();

            // DB.JSON_UI(ui, "layout/Login");
        }

        public static void Run2()
        {
            while (true)
                Console.WriteLine(Console.ReadKey().Key.ToString());
        }

        public static void Run3()
        {
            UI ui = new UI();

            TextLine t; Button b;

            //строки для демострации

            // строка на координатах 0, 0 с длиной в 40
            t = new TextLine("Test1", "Текст по левому краю", (0, 0), 60, -1);
            ui.Elements.Add(t);

            t = new TextLine("Test2", "Текст посередине", (0, 1), 60, 0);
            ui.Elements.Add(t);

            t = new TextLine("Test3", "Текст по правому краю", (0, 2), 60, 1);
            ui.Elements.Add(t);

            t = new TextLine("Test4", "Текст с fit=2", (0, 3), 60, 2);
            ui.Elements.Add(t);
            t.AddText("fit = 2");

            // кнопка на координатах 0, 5 длиной в 60 символов и высотой в 3
            b = new Button("Knopka1", "кнопка 1", (0, 5), (60, 3));
            ui.Elements.Add(b);

            // вторая кнопка немного ниже
            b = new Button("Knopka2", "кнопка 2", (0, 8), (60, 3));
            ui.Elements.Add(b);

            // эта строка выводит имя нажатой кнопки
            t = new TextLine("Vivod", "Была нажата строка с именем", (0, 11), 60, 2);
            t.Color = 2; // меняем цвет
            ui.Elements.Add(t);

            ui.Update();
            ConsoleKeyInfo key;
            string clicked;
            do
            {
                ui.Draw(true); // рисуем все элементы
                key = Console.ReadKey(true); // ввод с клавиатуры
                clicked = ui.SelectByKey(key); // UI сам решает, что с этой кнопкой делать
                // clicked = название нажатого элемента

                // получаем элемент с названием Vivod
                // и с его тексту добавляем название нажатого элемента
                ui.GetByName("Vivod").AddText(clicked);
            } while (key.Key != ConsoleKey.Escape);
        }
    }
}
