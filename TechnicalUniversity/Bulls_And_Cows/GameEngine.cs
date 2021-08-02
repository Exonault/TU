using System;

namespace Bulls_And_Cows
{
    public static class GameEngine
    {
        public static void StartGame()
        {
            Computer computer = new Computer();
            Player player = new Player(computer.Number);

            string nextToPlay = "player";
            bool isWinner = false;

            while (!isWinner)
            {
                switch (nextToPlay)
                {
                    case "player":
                        Console.WriteLine("Player's turn (try to guess the number of the computer)");
                        isWinner = player.Play();
                        nextToPlay = "computer";
                        break;
                    case "computer":
                        try
                        {
                            isWinner = computer.Play();
                            nextToPlay = "player";
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("You have lied");
                            isWinner = true;
                        }

                        break;
                }
            }
        }
    }
}