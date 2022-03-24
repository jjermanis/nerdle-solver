using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace nerdle_solver
{
    public class PossibleEquations
    {
        private const string FILE_PATH = @"..\..\..\equations.txt";
        private const int EQUATION_LEN = 8;
        private static readonly HashSet<char> VALID_RESULT_CHARS = new HashSet<char> { 'G', 'P', 'B' };

        private static readonly LetterDistribution _startDist;
        private static readonly IList<string> _startEquations;

        private LetterDistribution _dist;
        private IList<string> _options;

        static PossibleEquations()
        {
            var equations = File.ReadLines(FILE_PATH);
            _startDist = new LetterDistribution(equations);
            _startEquations = equations.OrderByDescending(w => Score(_startDist, w)).ToList();
        }

        public PossibleEquations()
        {
            _dist = _startDist;
            _options = _startEquations;
        }

        public static IList<string> GetAllEquations()
            => _startEquations;

        public string BestGuess()
            => _options.FirstOrDefault();

        public int RemainingOptions()
            => _options.Count;

        public static bool IsValidResult(string result)
        {
            if (result.Length != EQUATION_LEN)
                return false;
            foreach (var c in result)
                if (!VALID_RESULT_CHARS.Contains(c))
                    return false;
            return true;
        }

        private static long Score(LetterDistribution dist, string equation)
            => dist.Score(equation);

        public void UpdateGuess(string guess, string result)
        {
            var newList = new List<string>();
            foreach (var equation in _options)
                if (IsEquationValid(guess, result, equation))
                    newList.Add(equation);

            _options = newList;
        }

        public static bool IsEquationValid(string guess, string result, string equation)
        {
            // Optimization: do a simple check first. For each char, if the result is G, they
            // need to match, and if the result isn't G, they need to NOT match.
            for (int x = 0; x < 8; x++)
                if ((result[x] == 'G') == (guess[x] != equation[x]))
                    return false;

            // Equation is valid if it would generate the same result as the most recent guess
            return result.Equals(CalcResult(guess, equation));
        }

        public static string CalcResult(string guess, string equation)
        {
            var result = Enumerable.Repeat('B', EQUATION_LEN).ToArray();
            var remainingLetters = new List<char>();

            // Check for greens first
            for (var x = 0; x < EQUATION_LEN; x++)
            {
                if (guess[x] == equation[x])
                    result[x] = 'G';
                else
                    remainingLetters.Add(equation[x]);
            }

            // Now check for yellows
            for (var x = 0; x < EQUATION_LEN; x++)
            {
                if (result[x] != 'G' && remainingLetters.Contains(guess[x]))
                {
                    result[x] = 'P';
                    remainingLetters.Remove(guess[x]);
                }
            }

            return new string(result);
        }
    }
}