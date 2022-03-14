using System.Collections.Generic;

namespace nerdle_solver
{
    // Yes, these aren't letters, but the docs for Nerdle specifically refer to its
    // characters as "letters"
    internal class LetterDistribution
    {
        private const int EQ_LEN = 8;
        private IDictionary<char, int> Frequency { get; set; }
        private IDictionary<int, int> EqualsLocFreq { get; set; }

        public LetterDistribution(IEnumerable<string> equations)
        {
            InitMap(equations);
        }

        private void InitMap(IEnumerable<string> equations)
        {
            Frequency = new Dictionary<char, int>();
            for (var c = '0'; c <= '9'; c++)
            {
                Frequency[c] = 0;
            }
            Frequency['+'] = 0;
            Frequency['-'] = 0;
            Frequency['*'] = 0;
            Frequency['/'] = 0;
            EqualsLocFreq = new Dictionary<int, int>();
            for (var x = 4; x < 7; x++)
                EqualsLocFreq[x] = 0;

            foreach (var equation in equations)
                for (var x = 0; x < EQ_LEN; x++)
                {
                    var letter = equation[x];
                    if (letter != '=')
                        Frequency[letter]++;
                    else
                        EqualsLocFreq[x]++;
                }
        }

        public long Score(string equation)
        {
            var alreadySeen = new HashSet<char>();
            var result = 1L;

            for (var x = 0; x < EQ_LEN; x++)
            {
                var letter = equation[x];
                if (alreadySeen.Contains(letter))
                    continue;
                alreadySeen.Add(letter);
                if (letter != '=')
                    // Division here to prevent overflow
                    result *= (Frequency[letter] / 128);
                else
                    result *= EqualsLocFreq[x];
            }
            return result;
        }
    }
}