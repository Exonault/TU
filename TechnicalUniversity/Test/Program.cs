using System;

namespace Test
{
    using System;
    using System.Collections.Generic;

    namespace Dots_And_Boxes
    {
        class CustomBoardException : Exception
        {
            public CustomBoardException()
            {
            }

            public CustomBoardException(string message)
                : base(message)
            {
            }

            public CustomBoardException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }

        public class Player
        {
            private int points;
            private bool turn;

            public Player(bool turn)
            {
                this.turn = turn;
                this.points = 0;
            }

            public bool Turn
            {
                get { return this.turn; }
                set { this.turn = value; }
            }

            public int Points
            {
                get { return this.points; }
            }

            public void AddPoint()
            {
                this.points++;
            }

            public void SwapTurns()
            {
                this.turn = !this.turn;
            }
        }

        public class Board
        {
            string[,] matrix;
            Player p1;
            int computerPoints = 0;
            private List<int> rowsPoint = new List<int>() {1, 3, 3, 1};
            private List<int> colsPoint = new List<int>() {1, 1, 3, 3};
            List<int> computerRowMoves = new List<int>() {0, 1, 1, 2, 0, 2, 1, 3, 3, 4, 4, 3};
            List<int> computerColMoves = new List<int>() {1, 0, 2, 1, 3, 3, 4, 0, 2, 1, 3, 4};
            private int maxRows;
            private int maxCols;

            public Board(Player p1)
            {
                this.matrix = new string[5, 5];
                this.GenerateMatrix();
                this.p1 = p1;
                this.maxRows = this.rowsPoint.Count;
                this.maxCols = this.colsPoint.Count;
            }

            public int ComputerPoints
            {
                get { return computerPoints; }
            }

            private void GenerateMatrix()
            {
                for (int row = 0; row < 5; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        if (row % 2 == 0 && col % 2 != 0)
                            this.matrix[row, col] = "";
                        else if (row % 2 == 0 && col % 2 == 0)
                            this.matrix[row, col] = ".";
                        else if (row % 2 != 0 && col % 2 == 0)
                            this.matrix[row, col] = "";
                        else if (row % 2 != 0 && col % 2 != 0)
                            this.matrix[row, col] = " ";
                    }
                }
            }

            public void executeTurn(int row, int col)
            {
                if (row > this.maxRows || col > this.maxCols ||
                    this.matrix[row, col] == "_" || this.matrix[row, col] == "|")
                {
                    throw new CustomBoardException("Enter a valid coordinate");
                }
                else if (row % 2 == 0 && col % 2 != 0)
                {
                    this.matrix[row, col] = "_";
                    removeAvailableTurns(row, col);
                }
                else if (row % 2 != 0 && col % 2 == 0)
                {
                    this.matrix[row, col] = "|";
                    removeAvailableTurns(row, col);
                }
                else
                    throw new CustomBoardException("Enter a valid coordinate");
            }

            public bool CheckForPoint()
            {
                for (int i = 0; i < rowsPoint.Count; i++)
                {
                    if (this.matrix[this.rowsPoint[i] - 1, this.colsPoint[i]] == "_" &&
                        this.matrix[this.rowsPoint[i], this.colsPoint[i] - 1] == "|" &&
                        this.matrix[this.rowsPoint[i] + 1, this.colsPoint[i]] == "_" &&
                        this.matrix[this.rowsPoint[i], this.colsPoint[i] + 1] == "|")
                    {
                        if (p1.Turn)
                        {
                            p1.AddPoint();
                            this.matrix[this.rowsPoint[i], this.colsPoint[i]] = "P";
                        }
                        else
                        {
                            computerPoints++;
                            this.matrix[this.rowsPoint[i], this.colsPoint[i]] = "C";
                        }

                        this.rowsPoint.RemoveAt(i);
                        this.colsPoint.RemoveAt(i);
                        return true;
                    }
                }

                p1.SwapTurns();
                return false;
            }

            public bool EndGame()
            {
                if (p1.Points + computerPoints == 4)
                    return false;
                else
                    return true;
            }

            public string[,] GetMatrix()
            {
                return this.matrix;
            }

            private string computerSmart()
            {
                for (int i = 0; i < rowsPoint.Count; i++)
                {
                    int count = 0;
                    string coordinates = "";
                    if (this.matrix[this.rowsPoint[i] - 1, this.colsPoint[i]] == "_")
                    {
                        count++;
                    }
                    else
                    {
                        coordinates = $"{this.rowsPoint[i] - 1},{this.colsPoint[i]}";
                    }

                    if (this.matrix[this.rowsPoint[i] + 1, this.colsPoint[i]] == "_")
                    {
                        count++;
                    }
                    else
                    {
                        coordinates = $"{this.rowsPoint[i] + 1},{this.colsPoint[i]}";
                    }

                    if (this.matrix[this.rowsPoint[i], this.colsPoint[i] + 1] == "|")
                    {
                        count++;
                    }
                    else
                    {
                        coordinates = $"{this.rowsPoint[i]},{this.colsPoint[i] + 1}";
                    }

                    if (this.matrix[this.rowsPoint[i], this.colsPoint[i] - 1] == "|")
                    {
                        count++;
                    }
                    else
                    {
                        coordinates = $"{this.rowsPoint[i]},{this.colsPoint[i] - 1}";
                    }

                    if (count == 3)
                    {
                        return coordinates;
                    }
                }

                return "0";
            }

            public void computerExecuteTurn()
            {
                if (computerSmart() == "0")
                {
                    Random rnd = new Random();
                    int index = rnd.Next(computerRowMoves.Count);
                    this.executeTurn(computerRowMoves[index], computerColMoves[index]);
                }
                else
                {
                    string coordinates = computerSmart();
                    string[] rowAndCol = coordinates.Split(',');
                    int row = Convert.ToInt32(rowAndCol[0]);
                    int col = Convert.ToInt32(rowAndCol[1]);
                    this.executeTurn(row, col);
                }
            }

            private void removeAvailableTurns(int row, int col)
            {
                for (int i = 0; i < computerRowMoves.Count; i++)
                {
                    if (computerRowMoves[i] == row && computerColMoves[i] == col)
                    {
                        computerRowMoves.RemoveAt(i);
                        computerColMoves.RemoveAt(i);
                    }
                }
            }
        }

        class Program
        {
            public static void Main(string[] args)
            {
                string[,] matrix;
                Player p1 = new Player(true);
                Board board = new Board(p1);
                matrix = board.GetMatrix();
                bool gameOn = true;
                ShowMatrix(matrix);
                while (gameOn)
                {
                    int enteredRow = 0, enteredCol = 0;
                    if (p1.Turn)
                    {
                        try
                        {
                            Console.WriteLine("It's player 1's turn");
                            int ch = Convert.ToInt32(Console.ReadLine());
                            switch (ch)
                            {
                                case 1:
                                    enteredRow = 0;
                                    enteredCol = 1;
                                    break;
                                case 2:
                                    enteredRow = 0;
                                    enteredCol = 3;
                                    break;
                                case 3:
                                    enteredRow = 1;
                                    enteredCol = 0;
                                    break;
                                case 4:
                                    enteredRow = 1;
                                    enteredCol = 2;
                                    break;
                                case 5:
                                    enteredRow = 1;
                                    enteredCol = 4;
                                    break;
                                case 6:
                                    enteredRow = 2;
                                    enteredCol = 1;
                                    break;
                                case 7:
                                    enteredRow = 2;
                                    enteredCol = 3;
                                    break;
                                case 8:
                                    enteredRow = 3;
                                    enteredCol = 0;
                                    break;
                                case 9:
                                    enteredRow = 3;
                                    enteredCol = 2;
                                    break;
                                case 10:
                                    enteredRow = 3;
                                    enteredCol = 4;
                                    break;
                                case 11:
                                    enteredRow = 4;
                                    enteredCol = 1;
                                    break;
                                case 12:
                                    enteredRow = 4;
                                    enteredCol = 3;
                                    break;
                            }
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Enter a valid type of coordinate");
                            continue;
                        }
                    }

                    try
                    {
                        if (p1.Turn)
                        {
                            board.executeTurn(enteredRow, enteredCol);
                        }

                        if (!p1.Turn)
                        {
                            board.computerExecuteTurn();
                        }

                        if (board.CheckForPoint())
                        {
                            Console.WriteLine($"Player: {p1.Points} and Computer: {board.ComputerPoints}");
                        }

                        if (board.CheckForPoint())
                        {
                            Console.WriteLine($"Player: {p1.Points} and Computer: {board.ComputerPoints}");
                        }

                        if (board.CheckForPoint())
                        {
                            Console.WriteLine($"Player: {p1.Points} and Computer: {board.ComputerPoints}");
                        }

                        ShowMatrix(matrix);
                    }
                    catch (CustomBoardException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    gameOn = board.EndGame();
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();


                if (p1.Points > board.ComputerPoints)
                {
                    Console.WriteLine("Player wins!!!");
                }
                else if (board.ComputerPoints > p1.Points)
                {
                    Console.WriteLine("Computer wins!!");
                }
                else
                {
                    Console.WriteLine("DRAW!");
                }

                Console.ReadLine();
            }

            private static void ShowMatrix(string[,] matrix)
            {
                for (int row = 0; row < 5; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        Console.Write(matrix[row, col] + "  ");
                    }

                    Console.Write("\n");
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}