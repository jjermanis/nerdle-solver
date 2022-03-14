using System;
using System.IO;

namespace nerdle_solver
{
    internal class Program
    {
        private const string INTERACTIVE_ARG = "-p";
        private const string TEST_ARG = "-t";
        private const string GENERATE_EQUATIONS_ARG = "-g";

        private const string FILE_PATH = @"..\..\..\equations.txt";

        private static void Main(string[] args)
        {
            var words = File.ReadLines(FILE_PATH);

            var arg = args.Length > 0 ? args[0] : INTERACTIVE_ARG;

            switch (arg)
            {
                case TEST_ARG:
                    // TODO - implement test mode
                    throw new Exception("Not supported");

                case GENERATE_EQUATIONS_ARG:
                    new ValidEquationFinder().GenerateEquationFiles();
                    break;

                case INTERACTIVE_ARG:
                default:
                    new InteractiveGame(words).PlayGame();
                    break;
            }
        }
    }
}