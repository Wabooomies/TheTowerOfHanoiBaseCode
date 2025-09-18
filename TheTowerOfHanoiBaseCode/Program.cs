using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TheTowerOfHanoiBaseCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stack<int>[] towers = new Stack<int>[3];
            int stackSize = 3;
            int moveCount = 0;
            int perfectMoveCount = (int)Math.Pow(2,stackSize) - 1;

            InitializeTowers(towers, stackSize);
            DisplayWinningMessage(stackSize, TowerGame(towers, stackSize, moveCount), perfectMoveCount);
        }

        /// <summary>
        /// After checking the move, it transfers the disc from one tower to another.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="towers"></param>
        /// <param name="input"></param>
        /// <param name="moveCount"></param>
        /// <returns></returns>
        static int DoMove(bool move, Stack<int>[] towers, int[] input, int moveCount)
        {
            if (move)
            {
                towers[input[1]].Push(towers[input[0]].Pop());
                moveCount++;
            }

            return moveCount;
        }

        /// <summary>
        /// Asks the user to move the disc from a tower.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isNum"></param>
        /// <returns></returns>
        static void MoveFromTower(int[] input)
        {
            bool isNum = true;

            do
            {
                Console.Write("\tFrom what tower would you like to move the disk from? [0/1/2]\t");
                isNum = int.TryParse(Console.ReadLine(), out input[0]);
                if (isNum && (input[0] > 2 || input[0] < 0))
                    isNum = false;
            } while (!isNum);
        }

        /// <summary>
        /// Asks the user to move the disc to a tower.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isNum"></param>
        /// <returns></returns>
        static void MoveToTower(int[] input)
        {
            bool isNum = true;

            do
            {
                Console.Write("\tFrom what tower would you like to move the disk to? [0/1/2]\t");
                isNum = int.TryParse(Console.ReadLine(), out input[1]);
                if (isNum && (input[1] > 2 || input[1] < 0))
                    isNum = false;
                if (isNum && input[0] == input[1])
                    isNum = false;
            } while (!isNum);
        }

        /// <summary>
        /// Checks if the tower has discs and checks if the disc is larger/smaller than the disc on it's going to be on top of.
        /// </summary>
        /// <param name="towers"></param>
        /// <param name="input"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        static bool CheckIfValidMove(Stack<int>[] towers, int[] input)
        {
            Console.WriteLine($"\tAttempting to move a disk from Tower {input[0]} to Tower {input[1]}");
            bool move = true;

            if (towers[input[0]].Count <= 0)
            {
                move = false;
                Console.WriteLine("\tThere is nothing to move from that tower...");
            }
            else if (towers[input[1]].Count > 0)
            {
                if (towers[input[0]].Peek() > towers[input[1]].Peek())
                {
                    move = false;
                    Console.WriteLine(move);
                    Console.WriteLine("\tYou are unable to place a larger disk on top of a smaller one...");
                }
            }

            return move;
        }

        /// <summary>
        /// Moves the disc from one tower to another.
        /// </summary>
        /// <param name="towers"></param>
        /// <param name="stackSize"></param>
        /// <param name="moveCount"></param>
        /// <returns></returns>
        static bool Moving(Stack<int>[] towers, int stackSize, int[] input)
        {
            Console.WriteLine("\n");

            MoveFromTower(input);
            MoveToTower(input);

            return CheckIfValidMove(towers, input);
        }

        /// <summary>
        /// Creates the empty towers by giving making every index of the array -1 which will be converted to "|".
        /// </summary>
        /// <param name="towers"></param>
        /// <param name="stackSize"></param>
        /// <param name="moveCount"></param>
        /// <param name="displayTowers"></param>
        static void CreateEmptyTowers(Stack<int>[] towers, int stackSize, int[][] displayTowers)
        {
            for (int x = 0; x < towers.Length; x++)
            {
                displayTowers[x] = new int[stackSize + 2];
                for (int y = 0; y < displayTowers[x].Length; y++)
                    displayTowers[x][y] = -1;
            }
        }

        /// <summary>
        /// Inserts the discs from towers.
        /// </summary>
        /// <param name="towers"></param>
        /// <param name="buffer"></param>
        /// <param name="displayTowers"></param>
        static void InsertDiscs(Stack<int>[] towers, Stack<int> buffer, int[][] displayTowers)
        {
            for (int x = 0; x < towers.Length; x++)
            {
                buffer = new Stack<int>();
                while (towers[x].Count > 0)
                {
                    displayTowers[x][displayTowers[x].Length - towers[x].Count] = towers[x].Peek();
                    buffer.Push(towers[x].Pop());
                }

                while (buffer.Count > 0)
                    towers[x].Push(buffer.Pop());
            }
        }

        /// <summary>
        /// Shows the towers.
        /// </summary>
        /// <param name="towers"></param>
        /// <param name="buffer"></param>
        /// <param name="displayTowers"></param>
        /// <param name="thing"></param>
        /// <param name="colors"></param>
        static void DisplayTowers(Stack<int>[] towers, Stack<int> buffer, int[][] displayTowers, string thing, ConsoleColor[] colors)
        {
            for (int x = 0; x < displayTowers[0].Length; x++)
            {
                for (int y = 0; y < displayTowers.Length; y++)
                {
                    thing = "|";
                    Console.ResetColor();
                    if (displayTowers[y][x] != -1)
                    {
                        Console.ForegroundColor = colors[displayTowers[y][x] % colors.Length];
                        thing = displayTowers[y][x] + "";
                    }

                    Console.Write($"\t-{thing}-");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Plays the game Tower of Hanoi.
        /// </summary>
        /// <param name="towers"></param>
        /// <param name="stackSize"></param>
        /// <param name="moveCount"></param>
        static int TowerGame(Stack<int>[] towers, int stackSize, int moveCount)
        {
            Stack<int> buffer = new Stack<int>();
            int[][] displayTowers = new int[3][];
            string thing = "|";
            ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Magenta };

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"\tMove Count: {moveCount}\n");

                CreateEmptyTowers(towers, stackSize, displayTowers);
                InsertDiscs(towers, buffer, displayTowers);
                DisplayTowers(towers, buffer, displayTowers, thing, colors);

                Console.ResetColor();
                Console.WriteLine("\t---\t---\t---");

                if (towers[0].Count == 0 && towers[1].Count == 0)
                    break;

                int[] input = { -1, -1 };
                moveCount = DoMove(Moving(towers, stackSize, input), towers, input, moveCount);
            }

            return moveCount;
        }


        /// <summary>
        /// Create the towers by initializing an array of stacks. Creates the discs on the first tower (tower[0]).
        /// </summary>
        /// <param name="towers"></param>
        /// <param name="stackSize"></param>
        /// <returns></returns>
        static Stack<int>[] InitializeTowers(Stack<int>[] towers, int stackSize)
        {
            for (int x = 0; x < towers.Length; x++)
                towers[x] = new Stack<int>();

            for (int x = 0; x < stackSize; x++)
                towers[0].Push(stackSize - x);

            return towers;
        }

        /// <summary>
        /// Show the winning message, the move count of player, the perfect move count, and score after completing the game.
        /// </summary>
        /// <param name="stackSize"></param>
        /// <param name="moveCount"></param>
        /// <param name="perfectMoveCount"></param>
        static void DisplayWinningMessage(int stackSize, int moveCount, int perfectMoveCount)
        {
            Console.WriteLine($"\tCONGRATULATIONS! You finished the game in {moveCount} moves!");
            Console.WriteLine($"\tThe perfect move count should be {perfectMoveCount}. . .");
            Console.WriteLine($"\tYour score is {100 - ((perfectMoveCount - moveCount) / perfectMoveCount)}%");
        }
    }
}
