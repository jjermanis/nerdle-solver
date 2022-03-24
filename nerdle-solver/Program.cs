namespace nerdle_solver
{
    internal class Program
    {
        private const string INTERACTIVE_ARG = "-p";
        private const string TEST_ARG = "-t";
        private const string GENERATE_EQUATIONS_ARG = "-g";

        private static void Main(string[] args)
        {
            var arg = args.Length > 0 ? args[0] : INTERACTIVE_ARG;
 
            switch (arg)
            {
                case TEST_ARG:
                    new TestGame().RunTest();
                    break;

                case GENERATE_EQUATIONS_ARG:
                    new ValidEquationFinder().GenerateEquationFiles();
                    break;

                case INTERACTIVE_ARG:
                default:
                    new InteractiveGame().PlayGame();
                    break;
            }
        }
    }
}