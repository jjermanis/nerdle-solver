using System.Collections.Generic;
using System.Linq;

namespace nerdle_solver
{
    public class PossibleEquations
    {
        private const int EQUATION_LEN = 8;
        private static readonly HashSet<char> VALID_RESULT_CHARS = new HashSet<char> { 'G', 'P', 'B' };
        private readonly LetterDistribution _dist;
        private List<string> _options;

        public PossibleEquations(IEnumerable<string> equations)
        {
            _dist = new LetterDistribution(equations);
            _options = equations.OrderByDescending(w => Score(w)).ToList();
        }

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

        public long Score(string equation)
            => _dist.Score(equation);

        public void UpdateGuess(string guess, string result)
        {
            var newList = new List<string>();
            foreach (var equation in _options)
                if (IsEquationValid(guess, result, equation))
                    newList.Add(equation);

            _options = newList;
        }

        public static bool IsEquationValid(string guess, string result, string equation)
            // Equation is valid if it would generate the same result as the most recent guess
            => result.Equals(CalcResult(guess, equation));

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