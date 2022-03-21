using System;
using System.Collections.Generic;
using System.Linq;

namespace nerdle_solver
{
    public class TestGame
    {
        private const int GUESS_COUNT = 6;
        private readonly IList<string> _equations;

        public TestGame(IEnumerable<string> equations)
        {
            _equations = equations.ToList();
        }

        public void RunTest()
        {
            int start = Environment.TickCount;
            Console.WriteLine($"Running test: {_equations.Count} equations");
            var results = new ResultDistribution(GUESS_COUNT);
            foreach (var equation in _equations)
            {
                var score = PlayGame(equation);
                if (score.HasValue)
                    results.ScoreCount[score.Value]++;
                else
                    results.Misses++;
            }
            Console.Write(results);
            Console.WriteLine($"Time: {Environment.TickCount - start} ms");
        }

        public int? PlayGame(string target)
        {
            var options = new PossibleEquations(_equations);

            for (int i = 0; i < GUESS_COUNT; i++)
            {
                var currGuess = options.BestGuess();
                var result = GetResult(currGuess, target);
                if (result == "GGGGGGGG")
                {
                    return i + 1;
                }
                options.UpdateGuess(currGuess, result);
            }
            return null;
        }

        private string GetResult(string guess, string target)
            => PossibleEquations.CalcResult(guess, target);
    }
}