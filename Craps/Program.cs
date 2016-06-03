/*****************************************************************************************************************
* A console app to simulate a number of games of craps.  The user enters how many games they want played.        *
* When finished, it tells the user:                                                                              *
* - the average number of rolls per game                                                                         *
* - the highest number of rolls                                                                                  *
* - the lowest number of rolls                                                                                   *
* - the most common rolls                                                                                        *
* - the average winning percentage                                                                               *
* - the number of wins                                                                                           *
* - the number of losses                                                                                         *
*                                                                                                                *
* The rules of craps: each game the 'shooter' rolls two dice.   If the numbers on the dice add up to 2, 3, or 12 *
* the shooter loses. If the numbers on the dice add up to 7 or 11 the shooter wins.  If the numbers on the dice  *
* add up to 4, 5, 6, 8, 9 or 10 that sets the 'points'then the shooter wins,  shooter continues to roll until    *
* the numbers match the points then the shooter wins, or if a 7 is rolled the shooter loses.                     *
*                                                                                                                *
* Author: A Thomas   Created: 14 May 2016     Updated: 03 June 2016                                              *                      
*****************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Craps
{
    class Program
    {
        //class variable to ensure random numbers appear when finding random dice values
        static Random randomNumbers = new Random();

        //declaring class variables for end game statistics
        static float averageNoOfRollsPerGame = 0;
        static List<int> noOfRollsPerGame = new List<int>();
        static int highestNoOfRolls = 1;
        static int lowestNoOfRolls = 0;
        static int mostCommonRoll = 0;
        static List<int> sumOfEachRoll = new List<int>();
        static float averageWinningPercentage = 0;
        static int noOfWins = 0;
        static int noOfLosses = 0;

        static void Main(string[] args)
        {
            DisplayIntro();
        }

        private static void DisplayIntro()
        {
            Console.WriteLine("\nThe Game Craps\n");

            Console.WriteLine("How to play Craps:\n");

            Console.WriteLine("You enter the number of games you want to play.");
            Console.WriteLine("In each game, the shooter rolls two dice.");
            Console.WriteLine("If the numbers on the dice add up to 2, 3 or 12, the shooter looses.");
            Console.WriteLine("If the numbers on the dice add up to 7 or 11, the shooter wins.");
            Console.WriteLine("If the numbers on the dice add up to 4, 5, 6, 8, 9 or 10, that sets the points.");
            Console.WriteLine("In the latter case, the shooter continues to roll until the numbers match the points, then the shooter wins.");
            Console.WriteLine("Or, if a 7 is rolled, the shooter looses.\n");

            //call on method to commence asking the number of games a player would like to play
            ProcessNumberofGames();
        }

        private static void ProcessNumberofGames()
        {
            int numberOfGames = 0;
            string continueGame = "";
            Console.WriteLine("How many games of Craps do you want to play?");

            try
            {
                numberOfGames = Int32.Parse(Console.ReadLine());

                if (numberOfGames <= 0)
                {
                    NumberofGamesEntryError();
                }
                else
                {
                    PlayGame(numberOfGames);
                    Console.WriteLine();
                    Console.WriteLine("Press 'y' to continue playing");
                    continueGame = Console.ReadLine();
                    if (continueGame == "y" || continueGame == "Y")
                    {
                        ClearVariableValuesForNewGame();
                        DisplayIntro();
                    }
                    else
                    {
                        Console.WriteLine("Exiting.");
                        Console.ReadKey();
                    }
                }
            }
            catch
            {
                NumberofGamesEntryError();
            }
        }

        private static void NumberofGamesEntryError()
        {
            string agreeToContinue = "";
            Console.WriteLine("That entry was not recognized. Press 'y' if you wish to continue, or press any other key to exit.");
            agreeToContinue = Console.ReadLine();
            if (agreeToContinue == "y" || agreeToContinue == "Y")
                DisplayIntro();
            else
            {
                Console.WriteLine("Exiting");
                Console.ReadKey();
            }
        }

        //commence the actual game
        private static void PlayGame(int numberOfGames)
        {
            int dice1 = 0;
            int dice2 = 0;
            int sum = 0;

            for (int i = 0; i < numberOfGames; i++)
            {
                //a blank line for display purposes
                Console.WriteLine();

                dice1 = RollDice();
                dice2 = RollDice();

                Console.WriteLine("For round " + (i + 1) + ", the first dice has the value: " + dice1 + ", and the second dice has the value: " + dice2 + ".");
                sum = dice1 + dice2;
                Console.WriteLine("This gives the sum of: " + sum);

                //deal with statistical data for the end of the game
                StatisticalSumAnalysis(sum);
                MostCommonRoll(sum);

                //process the dice results
                ProcessDiceResults(sum);

                //display purposes
                Console.WriteLine();
            }
            DisplayEndGameStatistics();
        }

        //process dice results by calling on relevant method using a switch
        private static void ProcessDiceResults(int sum)
        {
            switch (sum)
            {
                case 2: case 3: case 12:
                    ShooterLoses();
                    break;
                case 7: case 11:
                    ShooterWins();
                    break;
                case 4: case 5: case 6: case 8: case 9: case 10:
                    PointsRound(sum);
                    break;
                default:
                    break;
            }
        }

        private static void ShooterLoses()
        {
            Console.WriteLine("Sorry, shooter looses this round.");
            //calculation for end of game statistic
            NoOfLosses();
        }
        private static void ShooterWins()
        {
            Console.WriteLine("Shooter wins this round!");
            //calculation for end of game statistic
            NoOfWins();
        }

        private static void PointsRound(int sum)
        {
            int pointSum = 0;
            int dice1 = 0;
            int dice2 = 0;
            int roundsCounter = 1;
            Boolean matchfound = false;

            Console.WriteLine("Commencing points round: Rolling again as the Shooter wins the round if the dice sum matches the points value " + sum + " before a 7 is rolled. Otherwise, the shooter loses.");
            Console.WriteLine("");

            while (matchfound == false)
            {
                Console.WriteLine();
                dice1 = RollDice();
                dice2 = RollDice();
                Console.WriteLine("Points round: The first dice has the value: " + dice1 + " and the second dice has the value: " + dice2 + ".");

                pointSum = dice1 + dice2;
                if (pointSum == 7)
                {
                    Console.WriteLine("This gives the sum of: " + pointSum);
                    ShooterLoses();
                    matchfound = true;
                }
                else if (pointSum == sum)
                {
                    Console.WriteLine("This gives the sum of: " + pointSum);
                    ShooterWins();
                    matchfound = true;
                }
                else
                    Console.WriteLine("This gives the sum of: " + pointSum + ". This does not match the points value " + sum + " or a 7.  Rolling again.");
                Console.ReadLine();

                //collect and manipulate data for end of game statistics
                roundsCounter++;
            }

            //manipulate data for end of game statistics
            StatisticalPointsAnalysis(roundsCounter);
            MostCommonRoll(pointSum);
        }
        public static void ClearVariableValuesForNewGame()
        {
            averageNoOfRollsPerGame = 0;
            List<int> noOfRollsPerGame = new List<int>();
            highestNoOfRolls = 1;
            lowestNoOfRolls = 0;
            mostCommonRoll = 0;
            List<int> sumOfEachRoll = new List<int>();
            averageWinningPercentage = 0;
            noOfWins = 0;
            noOfLosses = 0;

        }
        static int RollDice()
        {
            int dice = 0;
            dice = randomNumbers.Next(1, 7);
            return dice;
        }

        /***********statistics section methods**************/
        static void StatisticalSumAnalysis(int Sum)
        {
            LowestNoOfRolls(1);
            if ((Sum < 4 & Sum > 10) || Sum == 7)
            {
                AverageNumberOfRollsPerGame(1);
            }
        }
        static void StatisticalPointsAnalysis(int RoundsCounter)
        {
            HighestNoOfRolls(RoundsCounter);
            LowestNoOfRolls(RoundsCounter);
            AverageNumberOfRollsPerGame(RoundsCounter);
        }
        static void AverageNumberOfRollsPerGame(int Rounds)
        {
            noOfRollsPerGame.Add(Rounds);
            averageNoOfRollsPerGame = (float)noOfRollsPerGame.Average();
        }
        static void HighestNoOfRolls(int roundsCounter)
        {
            if (highestNoOfRolls < roundsCounter)
                highestNoOfRolls = roundsCounter;
        }
        static void LowestNoOfRolls(int Rounds)
        {
            //If rounds == 1, then it is not a points round, and will be the lowest no of rolls
            //else, it is a points round, so set lowestNoOfRounds if appropriate
            if (Rounds == 1)
            {
                lowestNoOfRolls = 1;
            }
            else if (lowestNoOfRolls > Rounds)
            {
                lowestNoOfRolls = Rounds;
            }
            else if (lowestNoOfRolls == 0)
            {
                lowestNoOfRolls = Rounds;
            }

        }
        static void MostCommonRoll(int Sum)
        {
            //this methods finds the lowest (common) number in the list ie. if you have {2, 2, 3, 3}, or {2, 3, 4, 5} it will only display '2'.
            sumOfEachRoll.Add(Sum);
            mostCommonRoll = (from i in sumOfEachRoll
                        group i by i into grp
                        orderby grp.Count() descending
                        select grp.Key).First();

        }
        static void AverageWinningPercentage()
        {
            averageWinningPercentage = (((float)noOfWins /((float)noOfWins + (float)noOfLosses)) * (float)100);     
        }

        static void NoOfWins()
        {
            noOfWins = noOfWins + 1;
        }

        static void NoOfLosses()
        {
            noOfLosses = noOfLosses + 1;
        }
       
        static void DisplayEndGameStatistics()
        {
            AverageWinningPercentage();
            Console.WriteLine("The average number of rolls per game was: " + averageNoOfRollsPerGame);
            Console.WriteLine("The highest number of rolls was: " + highestNoOfRolls);
            Console.WriteLine("The lowest number of rolls was: " + lowestNoOfRolls);
            Console.WriteLine("The most common roll was: " + mostCommonRoll);
            Console.WriteLine("The average winning percentage was: " + averageWinningPercentage + "%");
            Console.WriteLine("Total games you won: " + noOfWins);
            Console.WriteLine("Total games you lost: " + noOfLosses);
        }
    }
}



   
