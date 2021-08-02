using System;
using System.Collections.Generic;

namespace Bulls_And_Cows
{
    public class Computer
    {
        public string Number { get; }

        private readonly List<String> _possibleNumbers;
        private readonly Random _random;


        public Computer()
        {
            _random = new Random();
            _possibleNumbers = AllPossibleCombinations();
            Number = GenerateNumber();
        }

        public bool Play()
        {
            int initialIndex = _random.Next(_possibleNumbers.Count);

            String initialNumber = _possibleNumbers[initialIndex];

            Console.WriteLine($"Computer's guess is {initialNumber}");

            Console.WriteLine("Enter the number of bulls:");

            int bullsCount = int.Parse(Console.ReadLine());

            if (bullsCount == 4)
            {
                Console.WriteLine("Computer wins!");
                return true;
            }

            Console.WriteLine("Enter the number of cows:");

            int cowsCount = int.Parse(Console.ReadLine());

            _possibleNumbers.RemoveAt(initialIndex);
            NumbersPruning(initialNumber, bullsCount, cowsCount);

            if (_possibleNumbers.Count < 1 && bullsCount != 4)
            {
                throw new Exception();
            }

            Console.WriteLine("------------------------------------");

            return false;
        }


        private void NumbersPruning(string initialNumber, int bullsCount, int cowsCount)
        {
            for (int i = 0; i < _possibleNumbers.Count; i++)
            {
                string currentNumber = _possibleNumbers[i];

                int currentBullsCount = 0;
                int currentCowsCount = 0;

                for (int j = 0; j < initialNumber.Length; j++)
                {
                    if (currentNumber.Contains(initialNumber[j]))
                    {
                        currentCowsCount++;

                        if (currentNumber[j] == initialNumber[j])
                        {
                            currentBullsCount++;
                            currentCowsCount--;
                        }
                    }
                }

                if (currentBullsCount != bullsCount || currentCowsCount != cowsCount)
                {
                    _possibleNumbers.RemoveAt(i);
                    i--;
                }
            }
        }

        private string GenerateNumber()
        {
            int[] digits = new int[4];
            string number = "";

            number += digits[0] = _random.Next(1, 9);
            number += digits[1] = GenerateUniqueDigits(digits[0], -1, -1);
            number += digits[2] = GenerateUniqueDigits(digits[0], digits[1], -1);
            number += digits[3] = GenerateUniqueDigits(digits[0], digits[1], digits[2]);
            return number;
        }

        private int GenerateUniqueDigits(int firstDigit, int secondDigit, int thirdDigit)
        {
            int number = -1;

            do
            {
                number = _random.Next(9);
            } while (number == firstDigit || number == secondDigit || number == thirdDigit);

            return number;
        }


        private List<string> AllPossibleCombinations()
        {
            List<string> result = new List<string>();

            for (int firstDigit = 1; firstDigit <= 9; firstDigit++)
            {
                for (int secondDigit = 0; secondDigit <= 9; secondDigit++)
                {
                    if (firstDigit != secondDigit)
                    {
                        for (int thirdDigit = 0; thirdDigit <= 9; thirdDigit++)
                        {
                            if (thirdDigit != firstDigit && 
                                thirdDigit != secondDigit)
                            {
                                for (int fourthDigit = 0; fourthDigit <= 9; fourthDigit++)
                                {
                                    if (fourthDigit != firstDigit && 
                                        fourthDigit != secondDigit &&
                                        fourthDigit != thirdDigit)
                                    {
                                        string number = firstDigit + "" + secondDigit + "" + thirdDigit + "" +
                                                        fourthDigit + "";
                                        result.Add(number);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}