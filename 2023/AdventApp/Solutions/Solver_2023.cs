using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace NewAdventApp
{
    public class Solver_2023 : IHelpGatherInputs
    {
        private readonly ILogger<IHelpGatherInputs> _logger;
        private string _filename;

        public Solver_2023(ILogger<IHelpGatherInputs> logger)
        {
            _logger = logger;
        }

        public void SetFilename(string filename)
        {
            _filename = filename;
        }

        public string Solve_01_A()
        {
            var input = GetInputForFileAsString(_filename);

            var sum = 0;
            foreach (var line in input.Split("\n"))
            {
                var numbers = line.Where(c => Char.IsDigit(c));
                sum += int.Parse(numbers.FirstOrDefault().ToString()) * 10 + int.Parse(numbers.LastOrDefault().ToString());
            }

            return sum.ToString();
        }

        public string Solve_01_B()
        {
            var input = GetInputForFileAsString(_filename).ToLowerInvariant()
                                                        .Replace("one", "o1e")
                                                        .Replace("two", "t2o")
                                                        .Replace("three", "t3e")
                                                        .Replace("four", "f4r")
                                                        .Replace("five", "f5e")
                                                        .Replace("six", "s6x")
                                                        .Replace("seven", "s7n")
                                                        .Replace("eight", "e8t")
                                                        .Replace("nine", "n9e");

            var sum = 0;
            foreach (var line in input.Split("\n"))
            {
                var numbers = line.Where(c => Char.IsDigit(c));
                sum += int.Parse(numbers.FirstOrDefault().ToString()) * 10 + int.Parse(numbers.LastOrDefault().ToString());
            }

            return sum.ToString();
        }

        public string Solve_02_A()
        {
            var input = GetInputForFileAsString(_filename);

            var maxRedCubes = 12;
            var maxGreenCubes = 13;
            var maxBlueCubes = 14;

            var sum = 0;
            foreach (var line in input.Split("\n"))
            {
                var gameNumber = int.Parse(line.Split(":").First().Split(" ").Last().Trim());
                var isPossible = true;
                foreach (var hand in line.Split(":").Last().Split(";"))
                {
                    foreach (var color in hand.Split(","))
                    {
                        var number = int.Parse(color.Trim().Split(" ").First());
                        var isRed = color.ToLowerInvariant().Contains("red");
                        var isGreen = color.ToLowerInvariant().Contains("green");
                        var isBlue = color.ToLowerInvariant().Contains("blue");

                        if (isPossible && isRed && number > maxRedCubes)
                        {
                            isPossible = false;
                        }

                        if (isPossible && isGreen && number > maxGreenCubes)
                        {
                            isPossible = false;
                        }

                        if (isPossible && isBlue && number > maxBlueCubes)
                        {
                            isPossible = false;
                        }
                    }
                }

                if (isPossible)
                {
                    sum += gameNumber;
                }
            }

            return sum.ToString();
        }

        public string Solve_02_B()
        {
            var input = GetInputForFileAsString(_filename);



            var sum = 0;
            foreach (var line in input.Split("\n"))
            {
                var minRedCubes = 0;
                var minGreenCubes = 0;
                var minBlueCubes = 0;
                var gameNumber = int.Parse(line.Split(":").First().Split(" ").Last().Trim());

                foreach (var hand in line.Split(":").Last().Split(";"))
                {
                    foreach (var color in hand.Split(","))
                    {
                        var number = int.Parse(color.Trim().Split(" ").First());
                        var isRed = color.ToLowerInvariant().Contains("red");
                        var isGreen = color.ToLowerInvariant().Contains("green");
                        var isBlue = color.ToLowerInvariant().Contains("blue");

                        if (isRed && number > minRedCubes)
                        {
                            minRedCubes = number;
                        }

                        if (isGreen && number > minGreenCubes)
                        {
                            minGreenCubes = number;
                        }

                        if (isBlue && number > minBlueCubes)
                        {
                            minBlueCubes = number;
                        }
                    }
                }

                var power = minRedCubes * minGreenCubes * minBlueCubes;
                sum += power;
            }

            return sum.ToString();
        }
    }
}
