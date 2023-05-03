using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Sudoku
{
    /// <summary>
    /// Класс для создания и управления курсором.
    /// </summary>
    class Cursor
    {
        /// <summary>
        /// Позиция курсора по горизонтали.
        /// </summary>
        int columnPos;

        /// <summary>
        /// Позиция курсора по вертикали.
        /// </summary>
        int rowPos;

        /// <summary>
        /// Список с занятыми ячейками
        /// </summary>
        List<Position> busyCells;

        /// <summary>
        /// Числовое поле.
        /// </summary>
        DigitField digitField;

        /// <summary>
        /// Принимает начальные координаты курсора и числовое поле.
        /// </summary>
        /// <param name="column">Начальная строка.</param>
        /// <param name="row">Начальный столбец.</param>
        /// <param name="digField">Сгенерированное числовое роле.</param>
        public Cursor(int column, int row, DigitField digField)
        {
            columnPos = column;
            rowPos = row;
            busyCells = new List<Position>();
            digitField = digField;
        }

        /// <summary>
        /// Передвигает курсор в указанном направлении.
        /// </summary>
        /// <param name="dir">Направление движения.</param>
        public void Move(Direction dir)
        {
            if (dir == Direction.Up && (rowPos - 1) >= 0)
            {
                Hide();
                --rowPos;
                Show();
            }
            else if (dir == Direction.Down && (rowPos + 1) < SudokuMain.fieldHeight)
            {
                Hide();
                ++rowPos;
                Show();
            }
            else if (dir == Direction.Left && (columnPos - 1) >= 0)
            {
                Hide();
                --columnPos;
                Show();
            }
            else if (dir == Direction.Right && (columnPos + 1) < SudokuMain.fieldWidth)
            {
                Hide();
                ++columnPos;
                Show();
            }
        }

        /// <summary>
        /// Очищает текущую позицию от числа.
        /// </summary>
        public void Clear()
        {
            if (digitField.IsStartedDigit(rowPos, columnPos))
                return;

            digitField.Array[rowPos, columnPos] = 0;
            Console.SetCursorPosition(((columnPos + 1) * 6) - 3, (rowPos + 1) * 4 - 2);
            Console.Write(" ");
        }

        /// <summary>
        /// Запись числа в позицию курсора.
        /// </summary>
        /// <param name="digit">Число для записи.</param>
        /// <param name="digitField">Числовое поле.</param>
        public void Inject(int digit)
        {
            if(digitField.IsStartedDigit(rowPos, columnPos))
               return;

            busyCells.Clear();
            if (digitField.Inject(digit, rowPos, columnPos, busyCells))
            {
                Console.SetCursorPosition(((columnPos + 1) * 6) - 3, (rowPos + 1) * 4 - 2);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(digit);
            }
            else
                HighlightBusyCell(busyCells);
        }

        /// <summary>
        /// Подстветка уже занятой ячейки.
        /// </summary>
        void HighlightBusyCell(List<Position> busyCells)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (Position busyCell in busyCells)
            {
                Console.SetCursorPosition(((busyCell.column + 1) * 6) - 3, (busyCell.row + 1) * 4 - 3);
                Console.Write("*");
                Console.SetCursorPosition(((busyCell.column + 1) * 6) - 5, (busyCell.row + 1) * 4 - 2);
                Console.Write("*");
                Console.SetCursorPosition(((busyCell.column + 1) * 6) - 1, (busyCell.row + 1) * 4 - 2);
                Console.Write("*");
                Console.SetCursorPosition(((busyCell.column + 1) * 6) - 3, (busyCell.row + 1) * 4 - 1);
                Console.Write("*");
            }
            Thread.Sleep(200);
            foreach (Position busyCell in busyCells)
            {
                Console.SetCursorPosition(((busyCell.column + 1) * 6) - 3, (busyCell.row + 1) * 4 - 3);
                Console.Write(" ");
                Console.SetCursorPosition(((busyCell.column + 1) * 6) - 5, (busyCell.row + 1) * 4 - 2);
                Console.Write(" ");
                Console.SetCursorPosition(((busyCell.column + 1) * 6) - 1, (busyCell.row + 1) * 4 - 2);
                Console.Write(" ");
                Console.SetCursorPosition(((busyCell.column + 1) * 6) - 3, (busyCell.row + 1) * 4 - 1);
                Console.Write(" ");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Скрывает курсор.
        /// </summary>
        void Hide()
        {
            Console.SetCursorPosition(((columnPos + 1) * 6) - 3, (rowPos + 1) * 4 - 3);
            Console.Write(" ");
            Console.SetCursorPosition(((columnPos + 1) * 6) - 5, (rowPos + 1) * 4 - 2);
            Console.Write(" ");
            Console.SetCursorPosition(((columnPos + 1) * 6) - 1, (rowPos + 1) * 4 - 2);
            Console.Write(" ");
            Console.SetCursorPosition(((columnPos + 1) * 6) - 3, (rowPos + 1) * 4 - 1);
            Console.Write(" ");
        }

        /// <summary>
        /// Показ курсора.
        /// </summary>
        /// <param name="digitField">Числовое поле.</param>
        public void Show()
        {
            if(digitField.IsStartedDigit(rowPos, columnPos))
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(((columnPos + 1) * 6) - 3, (rowPos + 1) * 4 - 3);
            Console.Write("↓");
            Console.SetCursorPosition(((columnPos + 1) * 6) - 5, (rowPos + 1) * 4 - 2);
            Console.Write("→");
            Console.SetCursorPosition(((columnPos + 1) * 6) - 1, (rowPos + 1) * 4 - 2);
            Console.Write("←");
            Console.SetCursorPosition(((columnPos + 1) * 6) - 3, (rowPos + 1) * 4 - 1);
            Console.Write("↑");

            Console.ResetColor();
        }
    }
}
