using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Sudoku
{
    /// <summary>
    /// Класс для реализации кнопки.
    /// </summary>
    class Button
    {
        /// <summary>
        /// Позиция кнопки по x.
        /// </summary>
        int xPos;

        /// <summary>
        /// Позиция кнопки по y.
        /// </summary>
        int yPos;

        /// <summary>
        /// Отображаемый на кнопке текст.
        /// </summary>
        string displayedText;

        /// <summary>
        /// Светлый цвет для отрисовки.
        /// </summary>
        ConsoleColor brightColor;

        /// <summary>
        /// Темный цвет для отрисовки.
        /// </summary>
        ConsoleColor darkColor;

        /// <summary>
        /// Ширина кнопки.
        /// </summary>
        static int width = 19;

        /// <summary>
        /// Конструктор кнопки с параметрами.
        /// </summary>
        /// <param name="x">x позиция.</param>
        /// <param name="y">y позиция.</param>
        /// <param name="text">Текст на кнопке.</param>
        /// <param name="bright">Светлый цвет для отрисовки.</param>
        /// <param name="dark">Темный цвет для отрисовки.</param>
        public Button(int x, int y, string text, ConsoleColor bright, ConsoleColor dark)
        {
            xPos = x;
            yPos = y;
            displayedText = text;
            brightColor = bright;
            darkColor = dark;
        }

        /// <summary>
        /// Показ кнопки с анимацией.
        /// </summary>
        public void Show()
        {
            Console.ResetColor();
            Console.ForegroundColor = darkColor;
            for (int i = SudokuMain.windowWidth; i >= xPos; --i)
            {
                for (int j = i; j < i + width; ++j)
                {
                    if (j < SudokuMain.windowWidth)
                    {
                        Console.SetCursorPosition(j, yPos);
                        Console.Write('\u2588');
                        Console.SetCursorPosition(j, yPos + 1);
                        Console.Write('\u2588');
                        Console.SetCursorPosition(j, yPos + 2);
                        Console.Write('\u2588');
                    }
                }
                if (i + width < SudokuMain.windowWidth)
                {
                    Console.SetCursorPosition(i + width, yPos);
                    Console.Write(' ');
                    Console.SetCursorPosition(i + width, yPos + 1);
                    Console.Write(' ');
                    Console.SetCursorPosition(i + width, yPos + 2);
                    Console.Write(' ');
                }
            }
            Console.SetCursorPosition(width / 2 - (displayedText.Length / 2) + xPos, yPos + 1);
            Console.ForegroundColor = brightColor;
            Console.BackgroundColor = darkColor;
            Console.Write(displayedText);
        }

        /// <summary>
        /// Активация кнопки(меняет цвет и расширяется).
        /// </summary>
        public void Active()
        {
            Console.ResetColor();
            Console.ForegroundColor = brightColor;
            for (int i = xPos - 2; i < width + 2 + xPos; ++i)
            {
                Console.SetCursorPosition(i, yPos);
                Console.Write('\u2588');
                Console.SetCursorPosition(i, yPos + 1);
                Console.Write('\u2588');
                Console.SetCursorPosition(i, yPos + 2);
                Console.Write('\u2588');
            }
            Console.SetCursorPosition(width / 2 - (displayedText.Length / 2) + xPos, yPos + 1);
            Console.ForegroundColor = darkColor;
            Console.BackgroundColor = brightColor;
            Console.Write(displayedText);
        }

        /// <summary>
        /// Деактивация кнопки(возвращается в исходное состояние).
        /// </summary>
        public void Deactive()
        {
            Console.ResetColor();
            for (int i = xPos - 2; i < width + 2 + xPos; ++i)
            {
                Console.SetCursorPosition(i, yPos);
                Console.Write(' ');
                Console.SetCursorPosition(i, yPos + 1);
                Console.Write(' ');
                Console.SetCursorPosition(i, yPos + 2);
                Console.Write(' ');
            }
            Console.ForegroundColor = darkColor;
            for (int i = xPos; i < width + xPos; ++i)
            {
                Console.SetCursorPosition(i, yPos);
                Console.Write('\u2588');
                Console.SetCursorPosition(i, yPos + 1);
                Console.Write('\u2588');
                Console.SetCursorPosition(i, yPos + 2);
                Console.Write('\u2588');
            }
            Console.SetCursorPosition(width / 2 - (displayedText.Length / 2) + xPos, yPos + 1);
            Console.ForegroundColor = brightColor;
            Console.BackgroundColor = darkColor;
            Console.Write(displayedText);
        }

        /// <summary>
        /// Скрывает кнопку с экрана.
        /// </summary>
        public void Hide()
        {
            Console.ResetColor();
            for (int i = xPos - 2; i < width + 2 + xPos; ++i)
            {
                Console.SetCursorPosition(i, yPos);
                Console.Write(' ');
                Console.SetCursorPosition(i, yPos + 1);
                Console.Write(' ');
                Console.SetCursorPosition(i, yPos + 2);
                Console.Write(' ');
            }
        }
    }
}