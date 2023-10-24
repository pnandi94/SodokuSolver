using System;

class SudokuSolver
{
    private static int step = 0;

    public static void Main(string[] args)
    {
        int[,] puzzle = {
            {5, 3, 0, 0, 7, 0, 0, 0, 0},
            {6, 0, 0, 1, 9, 5, 0, 0, 0},
            {0, 9, 8, 0, 0, 0, 0, 6, 0},
            {8, 0, 0, 0, 6, 0, 0, 0, 3},
            {4, 0, 0, 8, 0, 3, 0, 0, 1},
            {7, 0, 0, 0, 2, 0, 0, 0, 6},
            {0, 6, 0, 0, 0, 0, 2, 8, 0},
            {0, 0, 0, 4, 1, 9, 0, 0, 5},
            {0, 0, 0, 0, 8, 0, 0, 7, 9}
        };

        int[,] solvedPuzzle = new int[9, 9];
        Array.Copy(puzzle, solvedPuzzle, 9 * 9);

        if (SolveSudoku(solvedPuzzle))
        {
            Console.WriteLine("Solved Sudoku:");
            PrintSudoku(solvedPuzzle, puzzle);
        }
        else
        {
            Console.WriteLine("No solution exists.");
        }
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
}
