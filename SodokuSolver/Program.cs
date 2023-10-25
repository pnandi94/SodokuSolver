using System;
using System.IO;

class SudokuSolver
{
    private static int step = 0;

    public static void Main(string[] args)
    {
        string filePath = "SudokuTable.txt";

        if (File.Exists(filePath))
        {
            int[,] puzzle = ReadSudokuFromFile(filePath);

            if (puzzle != null)
            {
                int[,] solvedPuzzle = new int[9, 9];
                Array.Copy(puzzle, solvedPuzzle, 9 * 9);

                if (SolveSudoku(solvedPuzzle))
                {
                    Console.WriteLine("Solved Sudoku:");
                    PrintSudoku(solvedPuzzle, puzzle);

                    // Save the solved Sudoku to a new file
                    WriteSudokuToFile("SolvedSudoku.txt", solvedPuzzle);
                }
                else
                {
                    Console.WriteLine("No solution exists.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Sudoku format in the file.");
            }
        }
        else
        {
            Console.WriteLine("File not found: " + filePath);
        }
    }

    static int[,] ReadSudokuFromFile(string filePath)
    {
        int[,] puzzle = new int[9, 9];

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < 9; i++)
            {
                string line = lines[i];
                if (line.Length != 9)
                {
                    return null; // Invalid Sudoku format
                }

                for (int j = 0; j < 9; j++)
                {
                    if (line[j] == '.')
                    {
                        puzzle[i, j] = 0;
                    }
                    else if (char.IsDigit(line[j]))
                    {
                        puzzle[i, j] = int.Parse(line[j].ToString());
                    }
                    else
                    {
                        return null; // Invalid character in Sudoku
                    }
                }
            }
        }
        catch (Exception)
        {
            return null; // Error while reading the file
        }

        return puzzle;
    }

    static bool SolveSudoku(int[,] puzzle)
    {
        int row, col;
        if (!FindEmptyCell(puzzle, out row, out col))
        {
            return true; // All cells filled; puzzle solved
        }

        for (int num = 1; num <= 9; num++)
        {
            if (IsSafe(puzzle, row, col, num))
            {
                step++;
                puzzle[row, col] = num;

                if (SolveSudoku(puzzle))
                {
                    return true;
                }

                puzzle[row, col] = 0; // Unsuccessful attempt; reset the cell
            }
        }
        return false; // Backtrack
    }

    static bool FindEmptyCell(int[,] puzzle, out int row, out int col)
    {
        row = col = -1;
        for (row = 0; row < 9; row++)
        {
            for (col = 0; col < 9; col++)
            {
                if (puzzle[row, col] == 0)
                {
                    return true; // Found an empty cell
                }
            }
        }
        return false; // All cells are filled
    }

    static bool IsSafe(int[,] puzzle, int row, int col, int num)
    {
        return !UsedInRow(puzzle, row, num) &&
               !UsedInCol(puzzle, col, num) &&
               !UsedInBox(puzzle, row - row % 3, col - col % 3, num);
    }

    static bool UsedInRow(int[,] puzzle, int row, int num)
    {
        for (int col = 0; col < 9; col++)
        {
            if (puzzle[row, col] == num)
            {
                return true;
            }
        }
        return false;
    }

    static bool UsedInCol(int[,] puzzle, int col, int num)
    {
        for (int row = 0; row < 9; row++)
        {
            if (puzzle[row, col] == num)
            {
                return true;
            }
        }
        return false;
    }

    static bool UsedInBox(int[,] puzzle, int boxStartRow, int boxStartCol, int num)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (puzzle[row + boxStartRow, col + boxStartCol] == num)
                {
                    return true;
                }
            }
        }
        return false;
    }

    static void PrintSudoku(int[,] puzzle, int[,] initialPuzzle)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (initialPuzzle[i, j] == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write(puzzle[i, j] + " ");
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        Console.WriteLine("Steps to solve the puzzle: " + step);
    }

    static void WriteSudokuToFile(string filePath, int[,] puzzle)
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    file.Write(puzzle[i, j] + " ");
                }
                file.WriteLine();
            }
        }
        Console.WriteLine("Solved Sudoku written to " + filePath);
    }
}
