using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Sudoku
{
    /// <summary>
    /// Перечисление уровней сложности.
    /// </summary>
    enum Difficulty { Easy, Medium, Hard };

    /// <summary>
    /// Перечисление для направлений движения.
    /// </summary>
    enum Direction { Up, Down, Left, Right };

    /// <summary>
    /// Структура для хранения позиции по строке и по столбцу.
    /// </summary>
    struct Position
    {
        public Position(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
        public int row;
        public int column;
    }

    /// <summary>
    /// Главный класс игры.
    /// </summary>
    public class SudokuMain
    {
        /// <summary>
        /// Опорная X точка для отрисовки программы.
        /// </summary>
        public static int startDrawingX = 0;

        /// <summary>
        /// Опорная Y точка для отрисовки программы.
        /// </summary>
        public static int startDrawingY = 0;

        /// <summary>
        /// Ширина окна в символах.
        /// </summary>
        public static int windowWidth = 56;

        /// <summary>
        /// Высота окна в символах.
        /// </summary>
        public static int windowHeight = 50;

        /// <summary>
        /// Ширина игрового поля.
        /// </summary>
        public static int fieldWidth = 9;

        /// <summary>
        /// Высота игрового поля.
        /// </summary>
        public static int fieldHeight = 9;

        /// <summary>
        /// Общее количество ячеек в поле.
        /// </summary>
        public static int allDigit = fieldWidth * fieldHeight;

        /// <summary>
        /// Точка входа в программу, настройка окна консоли.
        /// </summary>
        static void Main()
        {
            Console.Title = "Sudoku";
            Console.CursorVisible = false;
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.SetBufferSize(windowWidth, windowHeight);

            while(MainMenu());
        }

        /// <summary>
        /// Вывод главного меню на экран.
        /// </summary>
        static bool MainMenu()
        {
            Console.ResetColor();
            Console.Clear();
            ShowLogo();

            Button[] mainMenuButtons = new Button[4];
            mainMenuButtons[0] = new Button(18, 18, "Новая  игра", ConsoleColor.Green, ConsoleColor.DarkGreen);
            mainMenuButtons[1] = new Button(18, 26, "Правила", ConsoleColor.Cyan, ConsoleColor.DarkCyan);
            mainMenuButtons[2] = new Button(18, 34, "Об игре", ConsoleColor.Red, ConsoleColor.DarkRed);
            mainMenuButtons[3] = new Button(18, 42, "Выход", ConsoleColor.Gray, ConsoleColor.DarkGray);
            foreach (Button butt in mainMenuButtons)
                butt.Show();
            int currButton = 0;
            mainMenuButtons[0].Active();
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                mainMenuButtons[currButton].Deactive();

                if (key == ConsoleKey.UpArrow)
                    --currButton;
                else if (key == ConsoleKey.DownArrow)
                    ++currButton;
                else if (key == ConsoleKey.Enter)
                {
                    if (currButton == 0)
                    {
                        foreach (Button butt in mainMenuButtons)
                            butt.Hide();
                        SelectDifficulty();
                        return true;
                    }
                    else if (currButton == 1)
                    {
                        Rules();
                        return true;
                    }
                    else if (currButton == 2)
                    {
                        AboutGame();
                        return true;
                    }
                    else if (currButton == 3)
                        return false;
                }
                else if (key == ConsoleKey.Escape)
                    return false;

                if (currButton < 0)
                    currButton = 3;
                else if (currButton > 3)
                    currButton = 0;

                mainMenuButtons[currButton].Active();
            }
        }

        /// <summary>
        /// Выбор сложности игры.
        /// </summary>
        static void SelectDifficulty()
        {
            Button[] diffButtons = new Button[4];
            diffButtons[0] = new Button(18, 18, "Легкий", ConsoleColor.Green, ConsoleColor.DarkGreen);
            diffButtons[1] = new Button(18, 24, "Средний", ConsoleColor.Yellow, ConsoleColor.DarkYellow);
            diffButtons[2] = new Button(18, 30, "Тяжелый", ConsoleColor.Red, ConsoleColor.DarkRed);
            diffButtons[3] = new Button(18, 44, "Назад", ConsoleColor.Gray, ConsoleColor.DarkGray);
            foreach (Button butt in diffButtons)
                butt.Show();

            int currButton = 0;
            diffButtons[0].Active();
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                diffButtons[currButton].Deactive();

                if (key == ConsoleKey.UpArrow)
                    --currButton;
                else if (key == ConsoleKey.DownArrow)
                    ++currButton;
                else if (key == ConsoleKey.Enter)
                {
                    if (currButton == 0)
                        Game(Difficulty.Easy);
                    else if (currButton == 1)
                        Game(Difficulty.Medium);
                    else if (currButton == 2)
                        Game(Difficulty.Hard);
                    return;
                }
                else if (key == ConsoleKey.Escape)
                    return;

                if (currButton < 0)
                    currButton = 3;
                else if (currButton > 3)
                    currButton = 0;

                diffButtons[currButton].Active();
            }
        }

        /// <summary>
        /// Главный игровой метод. 
        /// </summary>
        static void Game(Difficulty diff)
        {
            // Создание и генерация поля
            DigitField digitField = new DigitField();
            digitField.Generate();
            // Настройка уровня сложности
            if (diff == Difficulty.Easy)
            {
                digitField.Hide(10);
                GameField.ShowGrid(ConsoleColor.Green, ConsoleColor.DarkGreen);
                GameField.ShowDigitField(digitField, ConsoleColor.Green);
            }
            else if (diff == Difficulty.Medium)
            {
                digitField.Hide(40);
                GameField.ShowGrid(ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                GameField.ShowDigitField(digitField, ConsoleColor.Yellow);
            }
            else if (diff == Difficulty.Hard)
            {
                digitField.Hide(80);
                GameField.ShowGrid(ConsoleColor.Red, ConsoleColor.DarkRed);
                GameField.ShowDigitField(digitField, ConsoleColor.Red);
            }
            GameField.ShowControlsInfo();

            Cursor cursor = new Cursor(0, 0, digitField);
            cursor.Show();

            while(true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        goto case ConsoleKey.NumPad1;
                    case ConsoleKey.D2:
                        goto case ConsoleKey.NumPad2;
                    case ConsoleKey.D3:
                        goto case ConsoleKey.NumPad3;
                    case ConsoleKey.D4:
                        goto case ConsoleKey.NumPad4;
                    case ConsoleKey.D5:
                        goto case ConsoleKey.NumPad5;
                    case ConsoleKey.D6:
                        goto case ConsoleKey.NumPad6;
                    case ConsoleKey.D7:
                        goto case ConsoleKey.NumPad7;
                    case ConsoleKey.D8:
                        goto case ConsoleKey.NumPad8;
                    case ConsoleKey.D9:
                        goto case ConsoleKey.NumPad9;
                    case ConsoleKey.NumPad1:
                        cursor.Inject(1);
                        break;
                    case ConsoleKey.NumPad2:
                        cursor.Inject(2);
                        break;
                    case ConsoleKey.NumPad3:
                        cursor.Inject(3);
                        break;
                    case ConsoleKey.NumPad4:
                        cursor.Inject(4);
                        break;
                    case ConsoleKey.NumPad5:
                        cursor.Inject(5);
                        break;
                    case ConsoleKey.NumPad6:
                        cursor.Inject(6);
                        break;
                    case ConsoleKey.NumPad7:
                        cursor.Inject(7);
                        break;
                    case ConsoleKey.NumPad8:
                        cursor.Inject(8);
                        break;
                    case ConsoleKey.NumPad9:
                        cursor.Inject(9);
                        break;
                    case ConsoleKey.UpArrow:
                        cursor.Move(Direction.Up);
                        break;
                    case ConsoleKey.DownArrow:
                        cursor.Move(Direction.Down);
                        break;
                    case ConsoleKey.LeftArrow:
                        cursor.Move(Direction.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        cursor.Move(Direction.Right);
                        break;
                    case ConsoleKey.Delete:
                        cursor.Clear();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
                if (digitField.IsWin())
                {
                    ShowWin();
                    return;
                }
            }
        }

        /// <summary>
        /// Показывает логотип игры.
        /// </summary>
        static void ShowLogo()
        {
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                StreamReader logoFile = new StreamReader("logo.txt");
                string logo = logoFile.ReadToEnd();
                logoFile.Close();
                Console.Write(logo);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Меню правила
        /// </summary>
        static void Rules()
        {
            Console.ResetColor();
            Console.Clear();
            ShowLogo();

            Console.SetCursorPosition(3, 14);
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                StreamReader logoFile = new StreamReader("rules.txt");
                string rules = logoFile.ReadToEnd();
                logoFile.Close();
                Console.Write(rules);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            for (int i = 10; i < 45; ++i)
                for (int j = 0; j < 55; ++j)
                {
                    Console.SetCursorPosition(j, i);
                    if (i == 10 || i == 44 || j == 0 || j == 54)
                        Console.Write('\u2588');
                }

            Button back = new Button(18, 46, "Назад", ConsoleColor.Gray, ConsoleColor.DarkGray);
            back.Show();
            back.Active();
            ConsoleKey key;
            while (true)
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape || key == ConsoleKey.Enter)
                    return;
            }
        }

        /// <summary>
        /// Меню об игре
        /// </summary>
        static void AboutGame()
        {
            Console.ResetColor();
            Console.Clear();
            ShowLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            for (int i = 10; i < 45; ++i)
                for (int j = 0; j < 55; ++j)
                {
                    Console.SetCursorPosition(j, i);
                    if (i == 10 || i == 44 || j == 0 || j == 54)
                        Console.Write('\u2588');
                }

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(23, 19);
            Console.Write("Sudoku 1.0");
            Console.SetCursorPosition(26, 21);
            Console.Write("2012");
            Console.SetCursorPosition(15, 25);
            Console.Write("Разработчик: Артем Алексеев");
            Console.SetCursorPosition(15, 27);
            Console.Write("E-mail: artzaleks@gmail.com");
            Console.SetCursorPosition(20, 29);
            Console.Write("Skype: artzaleks");

            Button back = new Button(18, 46, "Назад", ConsoleColor.Gray, ConsoleColor.DarkGray);
            back.Show();
            back.Active();
            ConsoleKey key;
            while (true)
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape || key == ConsoleKey.Enter)
                    return;
            }
        }

        /// <summary>
        /// Показывает сообщение о победе игрока.
        /// </summary>
        static void ShowWin()
        {
            Console.ResetColor();
            for (int i = 40; i < 49; ++i)
                for (int j = 0; j < 55; ++j)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(' ');
                }
            for (int i = 0; i < 10; ++i)
            {
                if (i % 2 == 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(21, 42);
                Console.Write("Вы победили!!!");
                Thread.Sleep(500);
            }
            Console.ReadKey();
        }
    }
}