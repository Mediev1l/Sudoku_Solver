using System;
using System.Collections.Generic;
using System.IO;

namespace Sudoku
{
    class Program
    {
        public static List<int> Load(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("File couldn't be found");
                return new List<int>(81);
            }
            else
            {
                using(TextReader sr = File.OpenText(path))
                {
                    List<int> output = new List<int>(81);

                    while(sr.Peek() >= 0)
                    {
                        int number = sr.Read();

                        if(number > 47 && number < 58)
                            output.Add(number % 48);
                    }

                    return new List<int>(output);
                }
            }
        }

        public static bool CheckIfPossible(List<int> container, int x, int y, int value)
        {
            for (int i = 0; i < 9; i++)
            {
                //x axis check
                if(container[9 * y + i] == value)
                    return false;

                //y axis check
                if (container[9 * i + x] == value)
                    return false;
            }


            // 3x3 cell check
            var x0 = x / 3 * 3;
            var y0 = y / 3 * 3;

            for (int yi = 0; yi < 3; yi++)
            {
                for (int xi = 0; xi < 3; xi++)
                {
                    if(container[9 * (y0 + yi) + (x0 + xi)] == value)
                        return false;
                }
            }
            return true;
        }

        public static bool IsSolved(List<int> sudoku)
        {
            foreach(var i in sudoku)
                if (i == 0)
                    return false;

            return true;
        }

        public static void Solve(List<int> sudoku)
        {
            int counter = 0;
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if(sudoku[y * 9 + x] == 0)
                    {
                        for (int i = 1; i < 10; i++)
                        {
                            if (CheckIfPossible(sudoku, x, y, i))
                            {
                                sudoku[y * 9 + x] = i;
                                Solve(sudoku);
                                sudoku[y * 9 + x] = 0;
                            }

                        }
                        return;
                    }
                }
            }

            if(IsSolved(sudoku))
            {
                counter++;
                using(StreamWriter sw = new StreamWriter(@$"C:\Users\Asus\source\repos\Sudoku\Solved{counter}.txt"))
                {
                    for (int i = 0; i < sudoku.Count; i++)
                    {
                        sw.Write(sudoku[i]);

                        if ((i + 1) % 9 == 0 && i > 0)
                            sw.Write('\n');
                        else
                            sw.Write(',');
                           
                    }
                }
            }
            
        }

        static void Main(string[] args)
        {
            Solve( 
                Load(@"C:\Users\Asus\source\repos\Sudoku\input.txt")
                );
        }
    }
}
