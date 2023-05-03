using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Sudoku
{
    /// <summary>
    /// Класс для работы с числовым полем.
    /// </summary>
    class DigitField
    {
        /// <summary>
        /// Полностью заполненный массив чисел.
        /// </summary>
        int[,] arrayDigit;

        /// <summary>
        /// Начальный массив чисел со скрытыми ячейками.
        /// </summary>
        int[,] arrayDigitStarted;

        /// <summary>
        /// Текущий массив чисел с учетом вводимых цифр.
        /// </summary>
        int[,] arrayDigitCurrent;

        /// <summary>
        /// Доступ к текущему массиву.
        /// </summary>
        public int[,] Array
        {
            get { return arrayDigitCurrent; }
        }

        /// <summary>
        /// Создание массива.
        /// </summary>
        public DigitField()
        {
            arrayDigit = new int[9, 9];
            arrayDigitStarted = new int[9, 9];
            arrayDigitCurrent = new int[9, 9];
        }

        /// <summary>
        /// Генерирует полную раскладку поля.
        /// </summary>
        public void Generate()
        {
            bool repeat = false;// Индикатор повторения числа
            // Цикл заполнения первой строки
            for (int i = 0; i < 9; ++i)
            {
                repeat = false;
                int digit = new Random().Next(1, 10);
                for (int j = 0; j < i; ++j)
                {
                    if (arrayDigit[0, j] == digit)// Встречалось ли число ранее
                    {
                        repeat = true;
                        break;
                    }
                }
                if (repeat)
                    --i;// Не даем сдвинутся с этой позиции
                else
                    arrayDigit[0, i] = digit;// Запись уникального числа в массив
            }

            /// Заполняем вторую строку путем перемещивания первой блоками по три цифры
            for (int i = 0; i < 3; ++i)
                arrayDigit[1, i] = arrayDigit[0, i + 6];
            for (int i = 3; i < 6; ++i)
                arrayDigit[1, i] = arrayDigit[0, i - 3];
            for (int i = 6; i < 9; ++i)
                arrayDigit[1, i] = arrayDigit[0, i - 3];

            /// Заполняем третью строку путем перемещивания первой блоками по три цифры
            for (int i = 0; i < 3; ++i)
                arrayDigit[2, i] = arrayDigit[0, i + 3];
            for (int i = 3; i < 6; ++i)
                arrayDigit[2, i] = arrayDigit[0, i + 3];
            for (int i = 6; i < 9; ++i)
                arrayDigit[2, i] = arrayDigit[0, i - 6];

            // Заполняем четвертую строку путем сдвига первой на одну цифру вправо
            for (int i = 0; i < 9; ++i)
            {
                if (i == 0)
                    arrayDigit[3, i] = arrayDigit[0, 8];
                else
                    arrayDigit[3, i] = arrayDigit[0, i - 1];
            }

            /// Заполняем пятую строку путем перемещивания четвертой блоками по три цифры
            for (int i = 0; i < 3; ++i)
                arrayDigit[4, i] = arrayDigit[3, i + 3];
            for (int i = 3; i < 6; ++i)
                arrayDigit[4, i] = arrayDigit[3, i + 3];
            for (int i = 6; i < 9; ++i)
                arrayDigit[4, i] = arrayDigit[3, i - 6];

            /// Заполняем шестую строку путем перемещивания четвертой блоками по три цифры
            for (int i = 0; i < 3; ++i)
                arrayDigit[5, i] = arrayDigit[3, i + 6];
            for (int i = 3; i < 6; ++i)
                arrayDigit[5, i] = arrayDigit[3, i - 3];
            for (int i = 6; i < 9; ++i)
                arrayDigit[5, i] = arrayDigit[3, i - 3];

            // Заполняем седьмую строку путем сдвига первой на две цифры вправо
            for (int i = 0; i < 9; ++i)
            {
                if (i == 0)
                    arrayDigit[6, i] = arrayDigit[0, 7];
                else if (i == 1)
                    arrayDigit[6, i] = arrayDigit[0, 8];
                else
                    arrayDigit[6, i] = arrayDigit[0, i - 2];
            }

            /// Заполняем восьмую строку путем перемещивания седьмой блоками по три цифры
            for (int i = 0; i < 3; ++i)
                arrayDigit[7, i] = arrayDigit[6, i + 6];
            for (int i = 3; i < 6; ++i)
                arrayDigit[7, i] = arrayDigit[6, i - 3];
            for (int i = 6; i < 9; ++i)
                arrayDigit[7, i] = arrayDigit[6, i - 3];

            /// Заполняем девятую строку путем перемещивания первой блоками по три цифры
            for (int i = 0; i < 3; ++i)
                arrayDigit[8, i] = arrayDigit[6, i + 3];
            for (int i = 3; i < 6; ++i)
                arrayDigit[8, i] = arrayDigit[6, i + 3];
            for (int i = 6; i < 9; ++i)
                arrayDigit[8, i] = arrayDigit[6, i - 6];
        }

        /// <summary>
        /// Скрывает определенное количество чисел из поля. 
        /// Для повышения производительности не учитывает уже скрытые ячейки, 
        /// поэтому итоговое количество скрытых ячеек может быть меньше указанного.
        /// </summary>
        /// <param name="count">
        /// Количество скрываемых ячеек.
        /// </param>
        public void Hide(int count)
        {
            if (count < 0 || count > SudokuMain.allDigit)
                throw new Exception("Число меньше нуля или больше общего количества ячеек");

            arrayDigitStarted = (int[,])arrayDigit.Clone();
            bool[,] maskHidden = new bool[9, 9];
            int row;
            int column;
            Random rnd = new Random();

            // Заполняется маска скрытых ячеек
            for (int i = 0; i < count; ++i)
            {
                row = rnd.Next(0, 9);
                column = rnd.Next(0, 9);
                // Код для проверки на повторяемость
                //if (maskHidden[row, column] == true)
                //    --i;
                //else
                maskHidden[row, column] = true;
            }

            // Обнуляются скрываемые ячейки
            for (int i = 0; i < SudokuMain.fieldHeight; ++i)
                for (int j = 0; j < SudokuMain.fieldWidth; ++j)
                    if (maskHidden[i, j] == true)
                        arrayDigitStarted[i, j] = 0;

            arrayDigitCurrent = (int[,])arrayDigitStarted.Clone();
        }

        /// <summary>
        /// Получает значение, показывающее было ли число изначально на поле.
        /// </summary>
        /// <param name="row">Строка массива.</param>
        /// <param name="column">Столбец массива.</param>
        /// <returns>True если число находится в стартовом массиве, false если отсутствует.</returns>
        public bool IsStartedDigit(int row, int column)
        {
            if (arrayDigitStarted[row, column] == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Пытается записать число в заданную ячейку.
        /// </summary>
        /// <param name="candidate">Цифра кандидат на запись.</param>
        /// <param name="row">Строка для записи.</param>
        /// <param name="column">Столбец для записи.</param>
        /// <param name="busyCells">Список с координатами занятых ячеек.</param>
        /// <returns>Логический итог записи(true успех, false провал).</returns>
        public bool Inject(int candidate, int row, int column, List<Position> busyCells)
        {
            bool busyCellFound = false;

            // Поиск в строке уже имеющегося числа.
            for (int i = 0; i < SudokuMain.fieldWidth; ++i)
                if (arrayDigitCurrent[row, i] == candidate && i != column)
                {
                    busyCells.Add(new Position(row, i));
                    busyCellFound = true;
                }

            // Поиск в столбце уже имеющегося числа.
            for (int i = 0; i < SudokuMain.fieldWidth; ++i)
                if (arrayDigitCurrent[i, column] == candidate && i != row)
                {
                    busyCells.Add(new Position(i, column));
                    busyCellFound = true;
                }

            // Поиск в квадрате уже имеющегося числа.
            int squareRowForCheck = row / 3;
            int squareColumnForCheck = column / 3;
            for (int i = squareRowForCheck * 3; i < squareRowForCheck * 3 + 3; ++i)
                for (int j = squareColumnForCheck * 3; j < squareColumnForCheck * 3 + 3; ++j)
                    if (arrayDigitCurrent[i, j] == candidate && j != column && i != row)
                    {
                        busyCells.Add(new Position(i, j));
                        busyCellFound = true;
                    }
            
            // При отсутствии повторений вносим введеное число в массив.
            if (busyCellFound)
                return false;
            else
            {
                arrayDigitCurrent[row, column] = candidate;
                return true;
            }
        }

        /// <summary>
        /// Проверяет заполнил ли игрок игровое поле полностью.
        /// </summary>
        /// <returns></returns>
        public bool IsWin()
        {
            for (int i = 0; i < SudokuMain.fieldHeight; ++i)
                for (int j = 0; j < SudokuMain.fieldWidth; ++j)
                    if(arrayDigitCurrent[i, j] == 0)
                        return false;
            return true;
        }
    }
}
