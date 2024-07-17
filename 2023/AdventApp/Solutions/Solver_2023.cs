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

        public string Solve_03_A()
        {
            var input = GetInputForFileAsString(_filename);
            var lines = input.Split("\n");

            var allowableCharacters = new List<char>() { '.', ' ', '\n' };
            var locationsOfSpecialCharacters = new List<(int, int)>();

            for (int row = 0; row < lines.Count(); row++)
            {
                for (int col = 0; col < lines[row].Trim().Length; col++)
                {
                    if (!char.IsLetterOrDigit(lines[row][col]) && !allowableCharacters.Contains(lines[row][col]))
                    {
                        locationsOfSpecialCharacters.Add((row, col));
                    }
                }
            }

            var sum = 0;
            for (int row = 0; row < lines.Count(); row++)
            {
                int startOfNumber = -1;
                int endOfNumber = -1;
                int number = 0;
                bool isSpecial = false;

                for (int col = 0; col < lines[row].Trim().Length; col++)
                {
                    if (char.IsDigit(lines[row][col]))
                    {
                        if (startOfNumber < 0)
                        {
                            startOfNumber = col;
                        }
                        endOfNumber = col;

                        number = number * 10 + int.Parse(lines[row][col].ToString());

                        if (!isSpecial)
                        {
                            var adjacentLocations = new List<(int, int)>()
                            {
                                (row-1,col-1),
                                (row-1,col),
                                (row-1,col+1),
                                (row,col-1),
                                (row,col+1),
                                (row+1,col-1),
                                (row+1,col),
                                (row+1,col+1)
                            };
                            isSpecial = locationsOfSpecialCharacters.Intersect(adjacentLocations).Any();
                        }

                        if (isSpecial && endOfNumber == lines[row].Trim().Length - 1)
                        {
                            sum += number;
                            _logger.LogInformation($"Adding {number}");
                        }
                    }
                    else
                    {
                        if (isSpecial)
                        {
                            sum += number;
                            _logger.LogInformation($"Adding {number}");
                        }

                        number = 0;
                        startOfNumber = -1;
                        endOfNumber = -1;
                        isSpecial = false;
                    }
                }
            }

            return sum.ToString();
        }

        public string Solve_03_B()
        {
            var input = GetInputForFileAsString(_filename);
            var lines = input.Split("\n");

            var locationsOfGears = new List<(int, int)>();

            for (int row = 0; row < lines.Count(); row++)
            {
                for (int col = 0; col < lines[row].Trim().Length; col++)
                {
                    if (lines[row][col] == '*')
                    {
                        locationsOfGears.Add((row, col));
                    }
                }
            }

            var sum = 0;
            Regex regexForContinuousNumbers = new Regex(@"\d{1,}");
            foreach (var gear in locationsOfGears)
            {
                var adjacentNumbers = new List<int>();
                var row = gear.Item1;
                var col = gear.Item2;

                var numberToTopLeft = (row > 0) && (col > 0) && char.IsDigit(lines[row - 1][col - 1]);
                var numberToTop = (row > 0) && char.IsDigit(lines[row - 1][col]);
                var numberToTopRight = (row > 0) && (col < lines[row].Trim().Length - 1) && char.IsDigit(lines[row - 1][col + 1]);
                var numberToLeft = (col > 0) && char.IsDigit(lines[row][col - 1]);
                var numberToRight = (col < lines[row].Trim().Length - 1) && char.IsDigit(lines[row][col + 1]);
                var numberToBottomLeft = (row < lines.Count() - 1) && (col > 0) && char.IsDigit(lines[row + 1][col - 1]);
                var numberToBottom = (row < lines.Count() - 1) && char.IsDigit(lines[row + 1][col]);
                var numberToBottomRight = (row < lines.Count() - 1) && (col < lines[row].Trim().Length - 1) && char.IsDigit(lines[row + 1][col + 1]);

                var numbersAbove = 0;
                if (!numberToTop)
                {
                    numbersAbove = (numberToTopLeft ? 1 : 0) + (numberToTopRight ? 1 : 0);
                }
                else
                {
                    numbersAbove = 1;
                }

                var numbersBelow = 0;
                if (!numberToBottom)
                {
                    numbersBelow = (numberToBottomLeft ? 1 : 0) + (numberToBottomRight ? 1 : 0);
                }
                else
                {
                    numbersBelow = 1;
                }

                var adjacentNumberCount = (numberToLeft ? 1 : 0) +
                                          (numberToRight ? 1 : 0) +
                                          numbersAbove +
                                          numbersBelow;

                if (adjacentNumberCount != 2)
                {
                    continue;
                }

                if (numberToLeft)
                {
                    var currentNumber = "";
                    var c = col - 1;
                    while (c >= 0 && char.IsDigit(lines[row][c]))
                    {
                        currentNumber = lines[row][c] + currentNumber;
                        c--;
                    }
                    adjacentNumbers.Add(int.Parse(currentNumber));
                }

                if (numberToRight)
                {
                    var currentNumber = "";
                    var c = col + 1;
                    while (c < lines[row].Trim().Length && char.IsDigit(lines[row][c]))
                    {
                        currentNumber = currentNumber + lines[row][c];
                        c++;
                    }
                    adjacentNumbers.Add(int.Parse(currentNumber));
                }

                if (numbersAbove == 1)
                {
                    var indexToRight = numberToTopRight ? col + 1 : col;
                    var startingIndexOfNumber = GetBeginningIndexOfNumberToLeft(lines[row - 1], indexToRight);
                    var substring = lines[row - 1].Substring(startingIndexOfNumber);
                    Match match = regexForContinuousNumbers.Match(substring);
                    adjacentNumbers.Add(int.Parse(match.Value));
                }

                if (numbersAbove == 2)
                {
                    var startingIndexOfLeftNumber = GetBeginningIndexOfNumberToLeft(lines[row - 1], col - 1);
                    var leftSubstring = lines[row - 1].Substring(startingIndexOfLeftNumber);
                    Match leftMatch = regexForContinuousNumbers.Match(leftSubstring);
                    adjacentNumbers.Add(int.Parse(leftMatch.Value));

                    var startingIndexOfRightNumber = col + 1;
                    var rightSubstring = lines[row - 1].Substring(startingIndexOfRightNumber);
                    Match rightMatch = regexForContinuousNumbers.Match(rightSubstring);
                    adjacentNumbers.Add(int.Parse(rightMatch.Value));
                }

                if (numbersBelow == 1)
                {
                    var indexToRight = numberToBottomRight ? col + 1 : col;
                    var startingIndexOfNumber = GetBeginningIndexOfNumberToLeft(lines[row + 1], indexToRight);
                    var substring = lines[row + 1].Substring(startingIndexOfNumber);
                    _logger.LogInformation($"Substring: {substring}");
                    Match match = regexForContinuousNumbers.Match(substring);
                    adjacentNumbers.Add(int.Parse(match.Value));
                }

                if (numbersBelow == 2)
                {
                    var startingIndexOfLeftNumber = GetBeginningIndexOfNumberToLeft(lines[row + 1], col - 1);
                    var leftSubstring = lines[row + 1].Substring(startingIndexOfLeftNumber);
                    Match leftMatch = regexForContinuousNumbers.Match(leftSubstring);
                    adjacentNumbers.Add(int.Parse(leftMatch.Value));

                    var startingIndexOfRightNumber = col + 1;
                    var rightSubstring = lines[row + 1].Substring(startingIndexOfRightNumber);
                    Match rightMatch = regexForContinuousNumbers.Match(rightSubstring);
                    adjacentNumbers.Add(int.Parse(rightMatch.Value));
                }

                _logger.LogInformation($"Gear ({row}, {col}) is adjacent to {adjacentNumbers[0]} and {adjacentNumbers[1]}.");

                sum += adjacentNumbers[0] * adjacentNumbers[1];
            }

            return sum.ToString();
        }

        private int GetBeginningIndexOfNumberToLeft(string input, int startingColumn)
        {
            var numberFound = false;
            var col = startingColumn;
            while (col >= 0)
            {
                if (char.IsDigit(input[col]))
                {
                    numberFound = true;
                    col--;
                }
                else
                {
                    if (!numberFound)
                    {
                        col--;
                    }
                    else
                    {
                        _logger.LogInformation($"Returning {col}");
                        return col;
                    }
                }
            }

            if (numberFound)
            {
                return 0;
            }
            return -1;
        }

        public string Solve_04_A()
        {
            var input = GetInputForFileAsString(_filename);
            var sum = 0;
            foreach (var line in input.Split("\n"))
            {
                var formattedLine = line.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
                List<int> winningNumbers = formattedLine.Split(":").Last().Split("|").First().Trim().Split(" ").Select(n => int.Parse(n)).ToList();
                List<int> myNumbers = formattedLine.Split("|").Last().Trim().Split(" ").Select(n => int.Parse(n)).ToList();
                int winners = winningNumbers.Intersect(myNumbers).Count();
                sum += (int)Math.Pow(2, winners - 1);

                _logger.LogInformation(line + "  -----> " + (int)Math.Pow(2, winners - 1));
            }

            return sum.ToString();
        }

        public string Solve_04_B()
        {
            var input = GetInputForFileAsString(_filename);
            var lastCardNumber = input.Split("\n").Count();

            var results = new Dictionary<int, int>();
            for (int i = 1; i <= lastCardNumber; i++)
            {
                results.Add(i, 1);
            }

            var sum = 0;
            for (int i = 1; i <= lastCardNumber; i++)
            {
                var line = input.Split("\n")[i - 1];
                var formattedLine = line.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
                List<int> winningNumbers = formattedLine.Split(":").Last().Split("|").First().Trim().Split(" ").Select(n => int.Parse(n)).ToList();
                List<int> myNumbers = formattedLine.Split("|").Last().Trim().Split(" ").Select(n => int.Parse(n)).ToList();
                int winners = winningNumbers.Intersect(myNumbers).Count();
                _logger.LogInformation($"Card {i}: {winners} winners");

                if (winners != 0)
                {
                    _logger.LogInformation($"Card {i}: {results[i]}");
                    for (int j = i + 1; j <= lastCardNumber && j <= i + winners; j++)
                    {
                        results[j] = results[j] + results[i];
                    }
                }

                sum += results[i];
            }

            return sum.ToString();
        }
    }
}
