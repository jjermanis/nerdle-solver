using NCalc;
using System;
using System.Collections.Generic;
using System.IO;

namespace nerdle_solver
{
    public class ValidEquationFinder
    {
        private readonly char[] OPERATORS = new char[] { '+', '-', '*', '/' };
        private const string TWO_OPS_FILE_PATH = @"..\..\..\TwoOps.txt";
        private const string ONE_OPS_FILE_PATH = @"..\..\..\OneOps.txt";

        public void GenerateEquationFiles()
        {
            var twoOps = TwoOperatorEquations();
            File.WriteAllLines(TWO_OPS_FILE_PATH, twoOps);
            var oneOps = OneOperatorEquations();
            File.WriteAllLines(ONE_OPS_FILE_PATH, oneOps);
        }

        private List<string> TwoOperatorEquations()
        {
            Console.WriteLine($"Starting TwoOperatorEquations at {DateTime.Now}");
            var result = new List<string>();
            for (var v1 = 1; v1 < 100; v1++)
                foreach (var o1 in OPERATORS)
                    for (var v2 = 1; v2 < 100; v2++)
                        if (v1 < 10 || v2 < 10)
                        {
                            foreach (var o2 in OPERATORS)
                                for (var v3 = 1; v3 < 100; v3++)
                                {
                                    (var isValid, var equation) = IsValid(v1, o1, v2, o2, v3);
                                    if (isValid)
                                    {
                                        result.Add(equation);
                                        if (result.Count % 1000 == 0)
                                        {
                                            Console.WriteLine($"Found {result.Count} equations at {DateTime.Now}");
                                        }
                                    }
                                }
                        }
            Console.WriteLine($"Finished TwoOperatorEquations. Found {result.Count} equations at {DateTime.Now}");
            return result;
        }

        public List<string> OneOperatorEquations()
        {
            Console.WriteLine($"Starting OneOperatorEquations at {DateTime.Now}");
            var result = new List<string>();
            for (var v1 = 1; v1 < 1000; v1++)
                foreach (var o in OPERATORS)
                    for (var v2 = 1; v2 < 1000; v2++)
                        if (v1 < 100 || v2 < 100)
                        {
                            (var isValid, var equation) = IsValid(v1, o, v2);
                            if (isValid)
                            {
                                result.Add(equation);
                                if (result.Count % 1000 == 0)
                                {
                                    Console.WriteLine($"Found {result.Count} equations at {DateTime.Now}");
                                }
                            }
                        }
            Console.WriteLine($"Finished OneOperatorEquations. Found {result.Count} equations at {DateTime.Now}");
            return result;
        }

        public static (bool, string) IsValid(int v1, char o1, int v2)
            => IsValid($"{v1}{o1}{v2}");

        public static (bool, string) IsValid(int v1, char o1, int v2, char o2, int v3)
            => IsValid($"{v1}{o1}{v2}{o2}{v3}");

        public static (bool, string) IsValid(string lval)
        {
            if (lval.Length <= 6)
            {
                var exp = new Expression(lval);
                var eval = exp.Evaluate();
                if (eval is int)
                {
                    var rval = (int)eval;
                    if (rval >= 0 &&
                        lval.Length + eval.ToString().Length == 7)
                    {
                        return (true, $"{lval}={eval}");
                    }
                }
                else if (eval is double)
                {
                    var rawVal = (double)eval;
                    var round = Math.Round(rawVal);
                    var est = Math.Round(rawVal, 6);
                    if (round == est)
                    {
                        var rval = (int)round;
                        if (rval >= 0 &&
                            lval.Length + eval.ToString().Length == 7)
                        {
                            return (true, $"{lval}={eval}");
                        }
                    }
                }
                else
                {
                    throw new Exception("Unhandled evaluation");
                }
            }
            return (false, null);
        }
    }
}