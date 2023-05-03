using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class GameField
    {
        /// <summary>
        /// Отрисовка игровой сетки.
        /// </summary>
        /// <param name="bright">Цвет линий отделяющих блоки цифр.</param>
        /// <param name="dark">Цвет обычных линий.</param>
        public static void ShowGrid(ConsoleColor bright, ConsoleColor dark)
        {
            Console.ResetColor();
            Console.Clear();
            Console.SetCursorPosition(SudokuMain.startDrawingX, SudokuMain.startDrawingY);
            int localDrawingY = SudokuMain.startDrawingY;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            for (int i = 0; i <= 36; ++i)
            {
                for (int j = 0; j <= 54; ++j)
                {
                    if (i % 12 == 0 || j % 18 == 0)
                        Console.ForegroundColor = bright;
                    else
                        Console.ForegroundColor = dark;

                    if (j == 0 || i == 0 || i == 36 || j == 54)
                        Console.Write('\u2588');
                    else if (i % 4 == 0 && j % 6 == 0)
                        Console.Write('╬');
                    else if (i == 0 || i % 4 == 0)
                        Console.Write('═');
                    else if (j == 0 || j % 6 == 0)
                        Console.Write('║');
                    else
                        Console.Write(' ');
                }
                Console.SetCursorPosition(SudokuMain.startDrawingX, ++localDrawingY);
            }
        }

        /// <summary>
        /// Выводит на экран массив чисел.
        /// </summary>
        /// <param name="digitField">Массив чисел.</param>
        /// <param name="color">Цвет для отрисовки чисел.</param>
        public static void ShowDigitField(DigitField digitField, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            for (int i = SudokuMain.startDrawingY; i < SudokuMain.fieldHeight; ++i)
                for (int j = 0; j < SudokuMain.fieldWidth; ++j)
                {
                    if (digitField.Array[i, j] != 0)
                    {
                        Console.SetCursorPosition(((j + 1) * 6) - 3, (i + 1) * 4 - 2);
                        Console.Write(digitField.Array[i, j]);
                    }
                }
        }

        /// <summary>
        /// Показ на экране информацию об управлении.
        /// </summary>
        public static void ShowControlsInfo()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(27, 40);
            Console.Write('↑');
            Console.SetCursorPosition(23, 42);
            Console.Write('←');
            Console.SetCursorPosition(31, 42);
            Console.Write('→');
            Console.SetCursorPosition(27, 44);
            Console.Write('↓');
            Console.SetCursorPosition(22, 46);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Перемещение");

            Console.SetCursorPosition(2, 42);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Delete");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("(Стереть)");

            Console.SetCursorPosition(39, 42);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("1 - 9");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("(Написать)");

            Console.SetCursorPosition(2, 48);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Escape");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("(Выход)"); 
        }
    }
}
