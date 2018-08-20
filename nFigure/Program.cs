using System;

namespace nFigure
{
    class Program
    {
        static void Main()
        {
            int modeFig;
            do
            {
                Console.Clear();
                ConsoleWriteColorText("Выберите режим:\n", ConsoleColor.DarkGreen);
                Console.WriteLine("\t1. Генерация геометрических фигур\n\t2. Анализ сгенерированных фигур\n\t3. Выход");
                Console.WriteLine("");
                modeFig = GetIntFromConsole();

                switch (modeFig)
                {
                    case 1:
                        {
                            ConsoleWriteColorText("Режим генерации фигур:\n", ConsoleColor.DarkGreen);
                            Console.Write("Сколько фигур Вы хотите создать? (Минимум 4) ");
                            int countFigure = GetIntFromConsole();
                            if (countFigure < 4)
                            {
                                ConsoleWriteColorText("Указано меньше 4 фигур!", ConsoleColor.DarkRed);
                                Console.ReadKey();
                                continue;
                            }

                            Object[] arrayOfFigures = GenerateFigures(countFigure);

                            Console.WriteLine("1. Сохранить фигуры в файл");
                            Console.WriteLine("2. Сохранить фигуры в БД");

                            int saveMode = GetIntFromConsole();
                            
                            switch (saveMode)
                            {
                                case 1:
                                {
                                    SaveFiguresToFile(ref arrayOfFigures);
                                }
                                    break;
                                case 2:
                                {
                                    SaveFiguresToDb(ref arrayOfFigures);
                                }
                                    break;
                            }

                        }
                        break;
                    case 2:
                    {
                        ConsoleWriteColorText("Режим анализа фигур:\n", ConsoleColor.DarkGreen);
                        Console.WriteLine("\t1. Загрузить фигуры из файла\n\t2. Загрузить фигуры из БД");
                        int loadMode = GetIntFromConsole();
                        Object[] arrayOfFigures = null;
                        switch (loadMode)
                        {
                            case 1:
                            {
                                arrayOfFigures = LoadFiguresFromFile();
                            }
                                break;
                            case 2:
                            {
                                arrayOfFigures = LoadFiguresFromDb();
                            }
                                break;
                        }

                        if (arrayOfFigures == null)
                        {
                            Console.ReadKey();
                            continue;
                        }

                        double sumSquareTriangle = 0;
                        double sumSquareRectangle = 0;
                        double sumSquareCircle = 0;

                        double sumPerimetrTriangle = 0;
                        double sumPerimetrRectangle = 0;

                        var arrayOfTriangle = arrayOfFigures[0] as Triangle[];
                        var arrayOfRectangle = arrayOfFigures[1] as Rectangle[];
                        var arrayOfCircle = arrayOfFigures[2] as Circle[];

                        Console.WriteLine("Фигуры загружены {0}! Количество: {1}", (loadMode == 1) ? "из файла" : (loadMode == 2) ? "из БД" : "",
                                                                                   arrayOfTriangle?.Length + arrayOfRectangle?.Length
                                                                                   + arrayOfCircle?.Length);
                        Console.WriteLine("Параметры какого типа фигур вывести?");
                        Console.WriteLine("1. Треугольников: {0}", arrayOfTriangle?.Length);
                        Console.WriteLine("2. Прямоугольников: {0}", arrayOfRectangle?.Length);
                        Console.WriteLine("3. Окружностей: {0}", arrayOfCircle?.Length);

                        int typeFigure = GetIntFromConsole();
                        switch (typeFigure)
                        {
                            case 1:
                            {
                                ShowTriangles(ref arrayOfTriangle, ref sumSquareTriangle, ref sumPerimetrTriangle);

                                Console.WriteLine("Сумма периметров всех треугольников равна {0}",
                                    sumPerimetrTriangle);
                                Console.WriteLine("Площадь всех треугольников равна {0}", sumSquareTriangle);
                            }
                                break;
                            case 2:
                            {
                                ShowRectangles(ref arrayOfRectangle, ref sumSquareRectangle, ref sumPerimetrRectangle);

                                Console.WriteLine("Сумма периметров всех прямоугольников равна {0}",
                                    sumPerimetrRectangle);
                                Console.WriteLine("Площадь всех прямоугольников равна {0}", sumSquareRectangle);
                            }
                                break;
                            case 3:
                            {
                                ShowCircles(ref arrayOfCircle, ref sumSquareCircle);

                                Console.WriteLine("Площадь всех окружностей равна {0}", sumSquareCircle);
                            }
                                break;
                        }

                    }
                        break;
                }
                if (modeFig != 3)
                    Console.ReadKey();
            } while (modeFig != 3);
        }

        /// <summary>
        /// Функция генерирует геометрические фигуры. Параметр countFigure принимает общее количество фигур. Возвращает массив с фигурами.
        /// </summary>
        /// <param name="countFigure">общее количество фигур</param>
        /// <returns>массив с фигурами</returns>
        public static Object[] GenerateFigures(int countFigure)
        {
            int[] arrayOfRnd = GetNewRndTolerance(1, countFigure, 3, 10);
            int countTriangles = arrayOfRnd[0];
            int countRectangles = arrayOfRnd[1];
            int countCircles = arrayOfRnd[2];

            Triangle[] arrayOfTriangles = new Triangle[countTriangles];
            Rectangle[] arrayOfRectangles = new Rectangle[countRectangles];
            Circle[] arrayOfCircles = new Circle[countCircles];

            int sWidth = Console.LargestWindowWidth;
            int sHeight = Console.LargestWindowHeight;
            Random r = new Random();
            for (int i = 0; i < countTriangles; i++)
            {
                arrayOfTriangles[i] = new Triangle(r.Next(sWidth), r.Next(sHeight), r.Next(sWidth), r.Next(sHeight),
                    r.Next(sWidth), r.Next(sHeight), i + 1);
            }

            for (int i = 0; i < countRectangles; i++)
            {
                arrayOfRectangles[i] = new Rectangle(r.Next(sWidth), r.Next(sHeight), r.Next(sWidth), r.Next(sHeight),
                    r.Next(sWidth), r.Next(sHeight), r.Next(sWidth), r.Next(sHeight), i + 1);
            }

            for (int i = 0; i < countCircles; i++)
            {
                arrayOfCircles[i] = new Circle(r.Next(sWidth), r.Next(sHeight), r.Next(30), i + 1);
            }

            Console.WriteLine("Треугольников создано: {0}", arrayOfTriangles.Length);
            Console.WriteLine("Прямоугольников создано: {0}", arrayOfRectangles.Length);
            Console.WriteLine("Окружностей создано: {0}", arrayOfCircles.Length);
            
            return new Object[] {arrayOfTriangles, arrayOfRectangles, arrayOfCircles};
        }

        /// <summary>
        /// Метод сохраняет массив типа Object[] в файл
        /// </summary>
        /// <param name="arrayOfFigures"></param>
        public static void SaveFiguresToFile(ref Object[] arrayOfFigures)
        {
            if (arrayOfFigures == null)
                throw new ArgumentNullException(nameof(arrayOfFigures));
            string currFullNameFile = Environment.CurrentDirectory + "\\FiguresOfGeometry.bin";
            Console.WriteLine("Сгенерированные объекты будут сохранены в файл {0}\n\tЖелаете изменить имя файла? (Y/N) N-",
                currFullNameFile);
            if (Console.ReadLine()?.ToUpper() == "Y")
            {
                do
                {
                    Console.WriteLine("Введите новое полное имя файла");
                    currFullNameFile = Console.ReadLine();
                } while (currFullNameFile == "");
            }

            using (FileManager fm = new FileManager())
            {
                StatusFile statusSave = fm.SaveToFile(currFullNameFile, arrayOfFigures);
                ConsoleWriteColorText((statusSave == StatusFile.Ok) ? "Фигуры сохранены!\n" :
                                                                      StatusFileMsg.Msg[(int)statusSave],
                                      (statusSave == StatusFile.Ok) ? ConsoleColor.DarkYellow :
                                                                      ConsoleColor.DarkRed);
            }
        }
        /// <summary>
        /// Метод сохраняет массив типа Object[] в БД
        /// </summary>
        /// <param name="arrayOfFigures"></param>
        public static void SaveFiguresToDb(ref Object[] arrayOfFigures)
        {
            if (arrayOfFigures == null)
                throw new ArgumentNullException(nameof(arrayOfFigures));
            using (DbManager db = new DbManager())
            {
                StatusDb statusSave = db.SaveToDb(arrayOfFigures);
                ConsoleWriteColorText((statusSave == StatusDb.Ok) ? "Фигуры сохранены в БД!\n" :
                                                                    "Не удалось сохранить фигуры в БД!\n",
                                      (statusSave == StatusDb.Ok) ? ConsoleColor.DarkYellow :
                                                                    ConsoleColor.DarkRed);
            }
        }

        /// <summary>
        /// Функция загружает фигуры из файла и возвращает массив типа Object[]
        /// </summary>
        public static Object[] LoadFiguresFromFile()
        {
            string currFullNameFile = Environment.CurrentDirectory + "\\FiguresOfGeometry.bin";
            Console.WriteLine("Будет загружен файл с сохранёнными фигурами {0}  (Y/N) Y-",
                currFullNameFile);
            if (Console.ReadLine()?.ToUpper() == "N")
            {
                do
                {
                    Console.WriteLine("Введите полное имя к файлу");
                    currFullNameFile = Console.ReadLine();
                } while (currFullNameFile == "");
            }
            Object[] arrayOfFigures;

            using (FileManager fm = new FileManager())
            {
                StatusFile statusLoad = fm.LoadFromFile(currFullNameFile, out arrayOfFigures);
                ConsoleWriteColorText(StatusFileMsg.Msg[(int) statusLoad] + "\n",
                    (statusLoad == StatusFile.Ok) ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed);
            }

            return arrayOfFigures;
        }
        /// <summary>
        /// Функция загружает фигуры из БД и возвращает массив типа Object[]
        /// </summary>
        public static Object[] LoadFiguresFromDb()
        {
            using (DbManager db = new DbManager())
            {
                Console.WriteLine("Содержимое БД:");
                int[] arrayEntryId = null;
                StatusDb statusView = db.ViewDb(ref arrayEntryId);
                if (statusView != StatusDb.Ok)
                {
                    ConsoleWriteColorText(StatusDbMsg.Msg[(int)statusView],
                        ConsoleColor.DarkRed);
                    return null;
                }

                foreach (var entryId in arrayEntryId)
                {
                    ConsoleWriteColorText("ID:", ConsoleColor.Yellow);
                    Console.WriteLine(" {0}", entryId);
                }

                Console.WriteLine("Сохранение с каким ID просмотреть?");

                StatusDb statusLoad;
                Object[] arrayOfFigures;
                do
                {
                    int viewId = GetIntFromConsole();
                    statusLoad = db.LoadFromDb(viewId, out arrayOfFigures);
                    string msgError = (statusLoad == StatusDb.ErrorId) ? StatusDbMsg.Msg[(int)statusLoad] +
                                                                         "\nВводите ID из предложенного списка!\n" :
                                                                         StatusDbMsg.Msg[(int)statusLoad] + "\n";
                    ConsoleWriteColorText(msgError, (statusLoad != StatusDb.Ok) ? ConsoleColor.DarkRed :
                                                                                  ConsoleColor.DarkGreen);
                } while (statusLoad == StatusDb.ErrorId);

                return arrayOfFigures;
            }
        }
        /// <summary>
        /// Метод выводит на экран параметры треугольников и подсчитывает общую площадь и периметр
        /// </summary>
        public static void ShowTriangles(ref Triangle[] arrayOfTriangle, ref double allSquare, ref double allPerimetr)
        {
            if (arrayOfTriangle != null)
                foreach (Triangle t in arrayOfTriangle)
                {
                    Console.WriteLine(
                        "Параметры треугольника {0}:\n\tСтороны: {1}, {2}, {3};\n\tP = {4}\n\tS = {5}",
                        t.IdTrian, t.LenghtOfSides[0], t.LenghtOfSides[1],
                        t.LenghtOfSides[2],
                        t.P, t.S);
                    allPerimetr += t.P;
                    allSquare += t.S;
                }
        }
        /// <summary>
        /// Метод выводит на экран параметры прямоугольников и подсчитывает общую площадь и периметр
        /// </summary>
        public static void ShowRectangles(ref Rectangle[] arrayOfRectangle, ref double allSquare, ref double allPerimetr)
        {
            if (arrayOfRectangle != null)
                foreach (Rectangle rc in arrayOfRectangle)
                {
                    Console.WriteLine(
                        "Параметры прямоугольника {0}:\n\tСтороны: {1}, {2}, {3}, {4};\n\tP = {5}\n\tS = {6}",
                        rc.IdRect,
                        rc.LenghtOfSides[0], rc.LenghtOfSides[1], rc.LenghtOfSides[2],
                        rc.LenghtOfSides[3],
                        rc.P, rc.S);
                    allPerimetr += rc.P;
                    allSquare += rc.S;
                }
        }
        /// <summary>
        /// Метод выводит на экран параметры окружностей и подсчитывает общую площадь
        /// </summary>
        public static void ShowCircles(ref Circle[] arrayOfCircle, ref double allSquare)
        {
            if (arrayOfCircle != null)
                foreach (Circle cr in arrayOfCircle)
                {
                    Console.WriteLine(
                        "Параметры окружности {0}:\n\tX: {1}, Y: {2};\n\tR = {3}\n\tS = {4}",
                        cr.IdCir, cr.X, cr.Y, cr.R, cr.S);
                    allSquare += cr.S;
                }
        }
        public static int GetIntFromConsole()
        {
            while (true)
                try { return Convert.ToInt32(Console.ReadLine()); }
                catch (FormatException) { ConsoleWriteColorText("Вводите только целочисленные значения: ", ConsoleColor.DarkRed); }
        }
        /// <summary>
        /// Метод позволяет выводить цветной текст на экран консоли.
        /// </summary>
        /// <param name="text">текст</param>
        /// <param name="colorForText">цвет текста из ConsoleColor</param>
        public static void ConsoleWriteColorText(string text, ConsoleColor colorForText)
        {
            Console.ForegroundColor = colorForText;
            Console.Write(text);
            Console.ResetColor();
        }
        /// <summary>
        /// Функция возвращает массив случайных чисел с заданной погрешностью между числами в процентах (последнее число в массиве - остаток).
        /// </summary>
        /// <param name="minValue">минимальное значение для генерирования</param>
        /// <param name="maxValue">максимальное значение для генерирования</param>
        /// <param name="countOfPart">количество генерируемых чисел</param>
        /// <param name="tolerancePercent">погрешность между числами</param>
        /// <returns></returns>
        public static int[] GetNewRndTolerance(int minValue, int maxValue, int countOfPart, int tolerancePercent = 0)
        {
            Random rnd = new Random();
            Random rndPlusMinus = new Random();
            int[] result = new int[countOfPart];
            int tolerance = (int)(maxValue / 100.00 * tolerancePercent);
            int sum = 0;
            int equalPart = maxValue / countOfPart;
            for (int i = 0; i < countOfPart - 1; i++)
            {
                if (tolerance < 1)
                {
                    result[i] = rnd.Next(minValue, (maxValue / 2 > 1) ? maxValue / 2 : maxValue);
                }
                else
                {
                    result[i] = (rndPlusMinus.Next(5) > 2) ? equalPart + rnd.Next(1, tolerance)
                                                           : equalPart - rnd.Next(1, tolerance);
                }
                sum += result[i];
            }

            result[countOfPart - 1] = maxValue - sum;
            return result;
        }
    }
}
