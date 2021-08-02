using System;
using System.Text;

namespace Bulls_And_Cows
{
    public class Player
    {
        private readonly Random _random;
        private readonly StringBuilder _scoreboard;
        private bool _firstRound;
        private readonly string _computerNumber;

        public Player(string computerNumber)
        {
            _random = new Random();
            _scoreboard = new StringBuilder()
                .Append("Previous guesses: Number Bulls Cows\n");
            _firstRound = true;
            _computerNumber = computerNumber;
        }

        public bool Play()
        {
            if (!_firstRound)
            {
                Console.WriteLine(_scoreboard.ToString());
            }
            else
            {
                _firstRound = false;
            }

            int bullsCount = 0;
            int cowsCount = 0;

            string playerGuess = ValidateInput();

            for (int i = 0; i < _computerNumber.Length; i++)
            {
                if (_computerNumber.Contains(playerGuess[i]))
                {
                    cowsCount++;
                    if (_computerNumber[i] == playerGuess[i])
                    {
                        bullsCount++;
                        cowsCount--;
                    }
                }
            }

            Console.WriteLine($"Bulls: {bullsCount} Cows: {cowsCount} \n");
            _scoreboard.Append($"                  {playerGuess}   {bullsCount}     {cowsCount} \n");

            if (bullsCount == 4)
            {
                Console.WriteLine("Player wins");
                return true;
            }

            Console.WriteLine("------------------------------------");
            return false;
        }

        private string ValidateInput()
        {
            string result = "";
            bool flag = false;
            do
            {
                result = Console.ReadLine();

                if (result.Length != 4 ||
                    result[0] == result[1] ||
                    result[0] == result[2] ||
                    result[0] == result[3] ||
                    result[1] == result[2] ||
                    result[1] == result[3] ||
                    result[2] == result[3])
                {
                    Console.WriteLine("Invalid input");
                }
                else
                {
                    flag = true;
                }
            } while (!flag);

            return result;
        }
    }
}