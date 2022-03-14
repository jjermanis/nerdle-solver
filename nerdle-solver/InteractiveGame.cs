using System;
using System.Collections.Generic;

namespace nerdle_solver
{
    internal class InteractiveGame
    {
        private readonly IEnumerable<string> _equations;

        public InteractiveGame(IEnumerable<string> equations)
        {
            _equations = equations;
        }

        public void PlayGame()
        {
            var options = new PossibleEquations(_equations);
            Console.WriteLine("Welcome to nerdle-solver. This program will help you solve the daily Nerdle puzzle.");
            Console.WriteLine("To use, guess what this program suggests. Then, let this program know the result.");
            Console.WriteLine("Enter the eight colors from the result. G for green, P for purple, and B for black.");

            for (int i = 0; i < 6; i++)
            {
                var currGuess = options.BestGuess();
                if (i > 0)
                {
                    var count = options.RemainingOptions();
                    if (count == 1)
                        Console.Write("OK - there's only one valid option. ");
                    else
                        Console.Write($"{count} valid options remaining. ");
                }
                Console.WriteLine($"Please guess: {currGuess}");
                var result = PromptResult();
                if (result == "GGGGGGGG")
                {
                    Console.WriteLine("Congrats!");
                    return;
                }
                options.AddClue(currGuess, result);
            }
            Console.WriteLine("Looks like you ran out of guesses. My fault.");
        }

        private static string PromptResult()
        {
            while (true)
            {
                Console.Write("Result? ");
                var result = Console.ReadLine().ToUpper();
                if (!PossibleEquations.IsValidResult(result))
                    Console.WriteLine(
                        "Incorrect format. Results should be eight letters long. G for green, P for purple, and B for black.");
                else
                    return result;
            }
        }
    }
}