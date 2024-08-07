using System.Globalization;
using System.Net;
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

        public string Solve_05_A()
        {
            var input = GetInputForFileAsString(_filename);

            var seeds = input.Split("seeds:").Last().Split("\n").First().Trim().Split(" ").Select(s => s.Trim()).ToList();
            var locations = new List<string>();

            var seedToSoilMap = Day05_GetMapForType("seed-to-soil map:", "soil-to-fertilizer map:", input);
            var soilToFertilizerMap = Day05_GetMapForType("soil-to-fertilizer map:", "fertilizer-to-water map:", input);
            var fertilizerToWaterMap = Day05_GetMapForType("fertilizer-to-water map:", "water-to-light map:", input);
            var waterToLightMap = Day05_GetMapForType("water-to-light map:", "light-to-temperature map:", input);
            var lightToTemperatureMap = Day05_GetMapForType("light-to-temperature map:", "temperature-to-humidity map:", input);
            var temperatureToHumidityMap = Day05_GetMapForType("temperature-to-humidity map:", "humidity-to-location map:", input);
            var humidityToLocationMap = Day05_GetMapForType("humidity-to-location map:", "end-of-file", input);

            foreach (var seed in seeds)
            {
                var soil = Day05_A_UseMap(seedToSoilMap, seed);
                var fertilizer = Day05_A_UseMap(soilToFertilizerMap, soil);
                var water = Day05_A_UseMap(fertilizerToWaterMap, fertilizer);
                var light = Day05_A_UseMap(waterToLightMap, water);
                var temperature = Day05_A_UseMap(lightToTemperatureMap, light);
                var humidity = Day05_A_UseMap(temperatureToHumidityMap, temperature);
                var location = Day05_A_UseMap(humidityToLocationMap, humidity);

                locations.Add(location);
                _logger.LogWarning($"{seed} -> {soil} -> {fertilizer} -> {water} -> {light} -> {temperature} -> {humidity} -> {location}");
            }

            return locations.Min(l => double.Parse(l)).ToString();
        }

        public Dictionary<(string, string), string> Day05_GetMapForType(string mapName, string nextMapName, string input)
        {
            var mapInput = input.Split(mapName).Last().Split(nextMapName).First().Replace("\n\n", "\n");
            var output = new Dictionary<(string, string), string>();

            foreach (var line in mapInput.Split("\n"))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var destinationRangeStart = double.Parse(line.Split(" ")[0]);
                var sourceRangeStart = double.Parse(line.Split(" ")[1]);
                var rangeLength = double.Parse(line.Split(" ")[2]);

                var sourceRange = (sourceRangeStart.ToString("G0"), (sourceRangeStart + rangeLength - 1).ToString("G0"));
                var modifier = (destinationRangeStart - sourceRangeStart).ToString("G0");

                output.Add(sourceRange, modifier);
            }

            return output;
        }

        public string Day05_A_UseMap(Dictionary<(string, string), string> map, string input)
        {
            var inputValue = double.Parse(input);
            if (map.Keys.Any(k => !string.IsNullOrWhiteSpace(k.Item1) && !string.IsNullOrWhiteSpace(k.Item2) && double.Parse(k.Item1) <= inputValue && inputValue <= double.Parse(k.Item2)))
            {
                var key = map.Keys.FirstOrDefault(k => !string.IsNullOrWhiteSpace(k.Item1) && !string.IsNullOrWhiteSpace(k.Item2) && double.Parse(k.Item1) <= inputValue && inputValue <= double.Parse(k.Item2));
                var modifier = double.Parse(map[key]);
                return (inputValue += modifier).ToString("G0");
            }

            return input;
        }

        public List<(string, string)> Day05_B_UseMap(Dictionary<(string, string), string> map, (string, string) input, string mapName)
        {
            var outputs = new List<(string, string)>();
            var lowerBound = Math.Min(double.Parse(input.Item1), double.Parse(input.Item2));
            var upperBound = Math.Max(double.Parse(input.Item1), double.Parse(input.Item2));

            if (map.Keys.Any(k => !string.IsNullOrWhiteSpace(k.Item1) && !string.IsNullOrWhiteSpace(k.Item2) &&
                                ((lowerBound <= double.Parse(k.Item1) && double.Parse(k.Item1) <= upperBound) ||
                                (lowerBound <= double.Parse(k.Item2) && double.Parse(k.Item2) <= upperBound)) ||
                                (double.Parse(k.Item1) < lowerBound && upperBound < double.Parse(k.Item2)) ||
                                (double.Parse(k.Item2) < lowerBound && upperBound < double.Parse(k.Item1))))
            {
                var keys = map.Keys.Where(k => !string.IsNullOrWhiteSpace(k.Item1) && !string.IsNullOrWhiteSpace(k.Item2) &&
                                ((lowerBound <= double.Parse(k.Item1) && double.Parse(k.Item1) <= upperBound) ||
                                (lowerBound <= double.Parse(k.Item2) && double.Parse(k.Item2) <= upperBound)) ||
                                (double.Parse(k.Item1) < lowerBound && upperBound < double.Parse(k.Item2)) ||
                                (double.Parse(k.Item2) < lowerBound && upperBound < double.Parse(k.Item1)))
                                .OrderBy(k => k.Item1)
                                .ToList();

                foreach (var key in keys)
                {
                    var modifier = double.Parse(map[key]);
                    var lowerKey = Math.Min(double.Parse(key.Item1), double.Parse(key.Item2));
                    var upperKey = Math.Max(double.Parse(key.Item1), double.Parse(key.Item2));

                    var lowerValue = Math.Max(lowerKey, lowerBound);
                    var upperValue = Math.Min(upperKey, upperBound);


                    outputs.Add(((lowerValue += modifier).ToString("G0"), (upperValue += modifier).ToString("G0")));
                }

                if (lowerBound < keys.Min(k => Math.Min(double.Parse(k.Item1), double.Parse(k.Item2))))
                {
                    var value = keys.Min(k => Math.Min(double.Parse(k.Item1), double.Parse(k.Item2)));
                    outputs.Add(((lowerBound).ToString("N0"), (value - 1).ToString("N0")));
                }

                if (upperBound > keys.Max(k => Math.Max(double.Parse(k.Item1), double.Parse(k.Item2))))
                {
                    var value = keys.Max(k => Math.Max(double.Parse(k.Item1), double.Parse(k.Item2)));
                    outputs.Add(((value + 1).ToString("N0"), (upperBound).ToString("N0")));
                }

                if (keys.Count() > 1)
                {
                    for (int k = 0; k < keys.Count() - 1; k++)
                    {
                        var firstRangeTop = double.Parse(keys[k].Item2);
                        var secondRangeBottom = double.Parse(keys[k + 1].Item1);
                        if (firstRangeTop + 1 < secondRangeBottom)
                        {
                            outputs.Add(((firstRangeTop + 1).ToString("N0"), (secondRangeBottom - 1).ToString("N0")));
                        }
                    }
                }

                return outputs;
            }

            return new List<(string, string)>() { input };
        }

        public string Day05_MapToString(Dictionary<(string, string), string> map)
        {
            var output = "";
            foreach (var key in map.Keys)
            {
                output += $"{key.Item1} to {key.Item2} => modify by {map[key]}\n";
            }
            return output;
        }

        public string Solve_05_B()
        {
            var input = GetInputForFileAsString(_filename);

            var seedInfo = input.Split("seeds:").Last().Split("\n").First().Trim().Split(" ").Select(s => s.Trim()).ToList();
            var seeds = new List<(string, string)>();
            for (int i = 0; i < seedInfo.Count; i += 2)
            {
                var startSeed = double.Parse(seedInfo[i]);
                var count = double.Parse(seedInfo[i + 1]);
                seeds.Add((startSeed.ToString("N0"), (startSeed + count - 1).ToString("N0")));
            }

            var seedToSoilMap = Day05_GetMapForType("seed-to-soil map:", "soil-to-fertilizer map:", input);
            var soilToFertilizerMap = Day05_GetMapForType("soil-to-fertilizer map:", "fertilizer-to-water map:", input);
            var fertilizerToWaterMap = Day05_GetMapForType("fertilizer-to-water map:", "water-to-light map:", input);
            var waterToLightMap = Day05_GetMapForType("water-to-light map:", "light-to-temperature map:", input);
            var lightToTemperatureMap = Day05_GetMapForType("light-to-temperature map:", "temperature-to-humidity map:", input);
            var temperatureToHumidityMap = Day05_GetMapForType("temperature-to-humidity map:", "humidity-to-location map:", input);
            var humidityToLocationMap = Day05_GetMapForType("humidity-to-location map:", "end-of-file", input);

            var soils = new List<(string, string)>();
            foreach (var seed in seeds)
            {
                soils.AddRange(Day05_B_UseMap(seedToSoilMap, seed, "seedToSoilMap"));
            }

            var fertilizers = new List<(string, string)>();
            foreach (var soil in soils)
            {
                fertilizers.AddRange(Day05_B_UseMap(soilToFertilizerMap, soil, "soilToFertilizerMap"));
            }

            var waters = new List<(string, string)>();
            foreach (var fertilizer in fertilizers)
            {
                waters.AddRange(Day05_B_UseMap(fertilizerToWaterMap, fertilizer, "fertilizerToWaterMap"));
            }

            var lights = new List<(string, string)>();
            foreach (var water in waters)
            {
                lights.AddRange(Day05_B_UseMap(waterToLightMap, water, "waterToLightMap"));
            }

            var temperatures = new List<(string, string)>();
            foreach (var light in lights)
            {
                temperatures.AddRange(Day05_B_UseMap(lightToTemperatureMap, light, "lightToTemperatureMap"));
            }

            var humidities = new List<(string, string)>();
            foreach (var temperature in temperatures)
            {
                humidities.AddRange(Day05_B_UseMap(temperatureToHumidityMap, temperature, "temperatureToHumidityMap"));
            }

            var locations = new List<(string, string)>();
            foreach (var humidity in humidities)
            {
                locations.AddRange(Day05_B_UseMap(humidityToLocationMap, humidity, "humidityToLocationMap"));
            }

            return locations.Min(l => Math.Min(double.Parse(l.Item1), double.Parse(l.Item2))).ToString();
        }

        public string Solve_06_A()
        {
            var input = GetInputForFileAsString(_filename);

            var times = new List<int>();
            foreach (var time in input.Split("\n").First().Split(":").Last().Split(" "))
            {
                if (string.IsNullOrWhiteSpace(time))
                {
                    continue;
                }

                times.Add(int.Parse(time));
            }

            var distances = new List<int>();
            foreach (var distance in input.Split("\n").Last().Split(":").Last().Split(" "))
            {
                if (string.IsNullOrWhiteSpace(distance))
                {
                    continue;
                }

                distances.Add(int.Parse(distance));
            }

            var marginForError = 1;
            for (int i = 0; i < times.Count; i++)
            {
                var maxTime = times[i];
                var recordDistance = distances[i];
                var winCount = 0;

                for (int t = 0; t <= maxTime; t++)
                {
                    var myDistance = (1 * t) * (maxTime - t);
                    if (myDistance > recordDistance)
                    {
                        winCount++;
                    }
                }
                marginForError *= winCount;
            }

            return marginForError.ToString();
        }

        public string Solve_06_B()
        {
            var input = GetInputForFileAsString(_filename);

            var allowedTime = double.Parse(input.Split("\n").First().Split(":").Last().Replace(" ", "").Trim());
            var recordDistance = double.Parse(input.Split("\n").Last().Split(":").Last().Replace(" ", "").Trim());

            var minTime = 0.0;
            var maxTime = allowedTime;
            for (double t = 0; t <= allowedTime; t++)
            {
                var myDistance = (1 * t) * (allowedTime - t);
                if (myDistance > recordDistance)
                {
                    minTime = t;
                    break;
                }
            }

            for (double t = allowedTime; t >= 0; t--)
            {
                var myDistance = (1 * t) * (allowedTime - t);
                if (myDistance > recordDistance)
                {
                    maxTime = t;
                    break;
                }
            }

            return (maxTime - minTime + 1).ToString();
        }

        public string Solve_07_A()
        {
            var input = GetInputForFileAsString(_filename);

            var handBetScores = new List<(string, double, int)>();
            foreach (var line in input.Split("\n"))
            {
                var hand = line.Split(" ").First();
                var bet = double.Parse(line.Split(" ").Last());
                var score = ScoreHand(hand);
                handBetScores.Add((hand, bet, score));
            }

            handBetScores = handBetScores.OrderByDescending(h => h.Item3)
                        .ThenByDescending(h => GetCardValue(h.Item1[0]))
                        .ThenByDescending(h => GetCardValue(h.Item1[1]))
                        .ThenByDescending(h => GetCardValue(h.Item1[2]))
                        .ThenByDescending(h => GetCardValue(h.Item1[3]))
                        .ThenByDescending(h => GetCardValue(h.Item1[4]))
                        .Reverse()
                        .ToList();

            var winnings = 0.0;
            var results = "";
            for (int i = 1; i <= handBetScores.Count; i++)
            {
                var newWinnings = i * handBetScores[i - 1].Item2;
                results += $"\n{i}\t{handBetScores[i - 1].Item1}\t({handBetScores[i - 1].Item3})\t\tBet {handBetScores[i - 1].Item2} ({newWinnings})";
                winnings += newWinnings;
            }
            _logger.LogInformation(results);
            return winnings.ToString();
        }

        private int ScoreHand(string hand)
        {
            if (CheckForFiveOfKind(hand))
            {
                return 5000;
            }
            else if (CheckForFourOfKind(hand))
            {
                return 2000;
            }
            else if (CheckForFullHouse(hand))
            {
                return 1000;
            }
            else if (CheckForThreeOfKind(hand))
            {
                return 500;
            }
            else if (CheckForTwoPair(hand))
            {
                return 200;
            }
            else if (CheckForPair(hand))
            {
                return 100;
            }
            else
            {
                return GetHighCard(hand);
            }
        }

        private string GetUniqueCards(string hand)
        {
            return string.Join("", hand.Distinct().ToList());
        }

        private int GetNumberOfOccurencesOfCard(string hand, char card)
        {
            return hand.Where(h => h == card).Count();
        }

        private bool CheckForFiveOfKind(string hand)
        {
            return GetUniqueCards(hand).Length == 1;
        }

        private bool CheckForFourOfKind(string hand)
        {
            var uniqueHand = GetUniqueCards(hand);
            return uniqueHand.Length == 2 &&
                    (GetNumberOfOccurencesOfCard(hand, uniqueHand[0]) == 4 || GetNumberOfOccurencesOfCard(hand, uniqueHand[1]) == 4);
        }

        private bool CheckForFullHouse(string hand)
        {
            var uniqueHand = GetUniqueCards(hand);
            return uniqueHand.Length == 2 &&
                    ((GetNumberOfOccurencesOfCard(hand, uniqueHand[0]) == 3 && GetNumberOfOccurencesOfCard(hand, uniqueHand[1]) == 2) ||
                    (GetNumberOfOccurencesOfCard(hand, uniqueHand[0]) == 2 && GetNumberOfOccurencesOfCard(hand, uniqueHand[1]) == 3));
        }

        private bool CheckForThreeOfKind(string hand)
        {
            var uniqueHand = GetUniqueCards(hand);
            return uniqueHand.Length == 3 &&
                    (GetNumberOfOccurencesOfCard(hand, uniqueHand[0]) == 3 || GetNumberOfOccurencesOfCard(hand, uniqueHand[1]) == 3 || GetNumberOfOccurencesOfCard(hand, uniqueHand[2]) == 3);
        }

        private int GetNumberOfPairs(string hand)
        {
            var uniqueHand = GetUniqueCards(hand);
            if (uniqueHand.Length == 5)
            {
                return 0;
            }

            var numberOfPairs = 0;
            foreach (var card in uniqueHand)
            {
                if (GetNumberOfOccurencesOfCard(hand, card) == 2)
                {
                    numberOfPairs++;
                }
            }
            return numberOfPairs;
        }

        private bool CheckForTwoPair(string hand)
        {
            return GetNumberOfPairs(hand) == 2;
        }

        private bool CheckForPair(string hand)
        {
            return GetNumberOfPairs(hand) == 1;
        }

        private int GetCardValue(char card)
        {
            if (char.IsDigit(card))
            {
                return int.Parse(card.ToString());
            }

            switch (card)
            {
                case 'A':
                    return 14;
                case 'K':
                    return 13;
                case 'Q':
                    return 12;
                case 'J':
                    return 11;
                case 'T':
                    return 10;
                default:
                    return 0;
            }
        }

        private int GetHighCard(string hand)
        {
            char highestCard = hand[0];
            for (int i = 1; i < hand.Length; i++)
            {
                if (GetCardValue(highestCard) < GetCardValue(hand[i]))
                {
                    highestCard = hand[i];
                }
            }
            return GetCardValue(highestCard);
        }

        public string Solve_07_B()
        {
            var input = GetInputForFileAsString(_filename);



            return "";
        }

        public string Solve_08_A()
        {
            var input = GetInputForFileAsString(_filename);

            var instructions = input.Split("\n").First().Trim();
            var map = new Dictionary<string, (string, string)>();

            input = input.Replace(instructions, "").Trim();
            foreach (var line in input.Split("\n"))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var key = line.Split("=").First().Trim();
                var left = line.Split("(").Last().Split(",").First().Trim();
                var right = line.Split(", ").Last().Split(")").First().Trim();
                map.Add(key, (left, right));
            }

            var currentNode = "AAA";
            var targetNode = "ZZZ";
            var stepsTaken = 0;
            var currentDirection = stepsTaken % instructions.Length;
            while (currentNode != targetNode)
            {
                var nextDirection = instructions[currentDirection];
                var currentMap = map[currentNode];
                var nextNode = nextDirection == 'L' ? currentMap.Item1 : currentMap.Item2;

                currentNode = nextNode;
                stepsTaken++;
                currentDirection = stepsTaken % instructions.Length;
            }

            return stepsTaken.ToString();
        }

        public string Solve_08_B()
        {
            var input = GetInputForFileAsString(_filename);

            var instructions = input.Split("\n").First().Trim();
            var map = new Dictionary<string, (string, string)>();

            input = input.Replace(instructions, "").Trim();
            foreach (var line in input.Split("\n"))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var key = line.Split("=").First().Trim();
                var left = line.Split("(").Last().Split(",").First().Trim();
                var right = line.Split(", ").Last().Split(")").First().Trim();
                map.Add(key, (left, right));
            }

            var currentNodes = new List<string>();
            foreach (var key in map.Keys.Where(k => k.EndsWith('A')))
            {
                _logger.LogInformation($"Starting at node {key}.");
                currentNodes.Add(key);
            }

            var phaseFrequencies = new List<(int, int)>();
            foreach (var node in currentNodes)
            {
                var currentNode = node;
                var stepsTaken = 0;
                var currentDirection = stepsTaken % instructions.Length;
                while (!currentNode.EndsWith('Z'))
                {
                    var nextDirection = instructions[currentDirection];
                    var currentMap = map[currentNode];
                    var nextNode = nextDirection == 'L' ? currentMap.Item1 : currentMap.Item2;

                    currentNode = nextNode;
                    stepsTaken++;
                    currentDirection = stepsTaken % instructions.Length;
                }

                var firstSuccess = stepsTaken;

                do
                {
                    var nextDirection = instructions[currentDirection];
                    var currentMap = map[currentNode];
                    var nextNode = nextDirection == 'L' ? currentMap.Item1 : currentMap.Item2;

                    currentNode = nextNode;
                    stepsTaken++;
                    currentDirection = stepsTaken % instructions.Length;
                } while (!currentNode.EndsWith('Z'));

                var secondSuccess = stepsTaken;
                phaseFrequencies.Add((firstSuccess, secondSuccess - firstSuccess));
            }

            _logger.LogInformation($"Frequencies:\n" + string.Join("\n", phaseFrequencies.Select(p => $"{p.Item1} + t * {p.Item2}")));

            // Use Excel to get LCM based on frequencies

            return "";
        }

        public string Solve_09_A()
        {
            var input = GetInputForFileAsString(_filename);

            var sum = 0.0;
            foreach (var line in input.Split("\n"))
            {
                var numbers = new List<List<double>>
                {
                    line.Split(" ").Select(x => double.Parse(x)).ToList()
                };

                var doneFindingDifferences = false;
                var previousRow = numbers[0];
                while (!doneFindingDifferences)
                {
                    var differences = Day_09_GetDifferences(previousRow);
                    numbers.Add(differences);
                    previousRow = differences;
                    doneFindingDifferences = differences.All(d => d == 0.0);
                }

                var previousDifference = 0.0;
                for (int i = numbers.Count - 2; i >= 0; i--)
                {
                    numbers[i] = Day_09_ExtrapolateRowForward(numbers[i], previousDifference);
                    previousDifference = numbers[i].Last();
                }

                _logger.LogInformation(string.Join("\n", numbers.Select(n => string.Join(" ", n))));

                sum += previousDifference;
            }

            return sum.ToString();
        }

        public List<double> Day_09_GetDifferences(List<double> input)
        {
            var output = new List<double>();
            for (int i = 1; i < input.Count; i++)
            {
                output.Add(input[i] - input[i - 1]);
            }
            return output;
        }

        public List<double> Day_09_ExtrapolateRowForward(List<double> row, double nextDifference)
        {
            row.Add(row.Last() + nextDifference);
            return row;
        }

        public List<double> Day_09_ExtrapolateRowBackwards(List<double> row, double nextDifference)
        {
            row.Insert(0, row.First() - nextDifference);
            return row;
        }

        public string Solve_09_B()
        {
            var input = GetInputForFileAsString(_filename);

            var sum = 0.0;
            foreach (var line in input.Split("\n"))
            {
                var numbers = new List<List<double>>
                {
                    line.Split(" ").Select(x => double.Parse(x)).ToList()
                };

                var doneFindingDifferences = false;
                var previousRow = numbers[0];
                while (!doneFindingDifferences)
                {
                    var differences = Day_09_GetDifferences(previousRow);
                    numbers.Add(differences);
                    previousRow = differences;
                    doneFindingDifferences = differences.All(d => d == 0.0);
                }

                var previousDifference = 0.0;
                for (int i = numbers.Count - 2; i >= 0; i--)
                {
                    numbers[i] = Day_09_ExtrapolateRowBackwards(numbers[i], previousDifference);
                    previousDifference = numbers[i].First();
                }

                _logger.LogInformation(string.Join("\n", numbers.Select(n => string.Join(" ", n))));

                sum += previousDifference;
            }

            return sum.ToString();
        }

        private List<string> _pipeMap = new List<string>();
        private List<List<int>> _pipeMapDistances = new List<List<int>>();
        public string Solve_10_A()
        {
            var input = GetInputForFileAsString(_filename);

            _pipeMap = input.Split("\n").ToList();
            Day_10_RemoveExtraneousPipes();

            foreach (var row in _pipeMap)
            {
                var distanceRow = new List<int>();
                foreach (var column in row)
                {
                    distanceRow.Add(-1);
                }
                _pipeMapDistances.Add(distanceRow);
            }

            var startingRow = _pipeMap.FindIndex(r => r.Contains("S"));
            var startingColumn = _pipeMap[startingRow].IndexOf("S");
            _pipeMapDistances[startingRow][startingColumn] = 0;
            _logger.LogInformation($"Starting at coordinate ({startingRow}, {startingColumn})");
            // _logger.LogInformation(string.Join("\n", _pipeMap));

            if (Day_10_PipeAboveIsValid(startingRow, startingColumn))
            {
                _logger.LogInformation("Following pipe to top...");
                Day_10_FollowPipe(Day_10_Direction.Up, startingRow - 1, startingColumn, 0);
                _logger.LogInformation("Done following pipe to top.");
            }
            if (Day_10_PipeToRightIsValid(startingRow, startingColumn))
            {
                _logger.LogInformation("Following pipe to right...");
                Day_10_FollowPipe(Day_10_Direction.Right, startingRow, startingColumn + 1, 0);
                _logger.LogInformation("Done following pipe to right.");
            }
            if (Day_10_PipeBelowIsValid(startingRow, startingColumn))
            {
                _logger.LogInformation("Following pipe below...");
                Day_10_FollowPipe(Day_10_Direction.Down, startingRow + 1, startingColumn, 0);
                _logger.LogInformation("Done following pipe below.");
            }
            if (Day_10_PipeToLeftIsValid(startingRow, startingColumn))
            {
                _logger.LogInformation("Following pipe to right...");
                Day_10_FollowPipe(Day_10_Direction.Left, startingRow, startingColumn - 1, 0);
                _logger.LogInformation("Done following pipe to right.");
            }

            var maxDistance = _pipeMapDistances.SelectMany(r => r.Where(d => d < int.MaxValue && d >= 0)).Max();
            var maxDistanceRow = _pipeMapDistances.FindIndex(r => r.Contains(maxDistance));
            var maxDistanceColumn = _pipeMapDistances[maxDistanceRow].IndexOf(maxDistance);
            _logger.LogInformation($"Ending at coordinate ({maxDistanceRow}, {maxDistanceColumn})");

            return maxDistance.ToString();
        }

        private void Day_10_RemoveExtraneousPipes()
        {
            var complete = false;
            while (!complete)
            {
                complete = true;
                var modifiedMap = new List<string>();
                for (int r = 0; r < _pipeMap.Count; r++)
                {
                    var modifiedRow = "";
                    for (int c = 0; c < _pipeMap[r].Length; c++)
                    {
                        var pipe = _pipeMap[r][c];
                        var modifiedPipe = Day_10_ModifyPipeIfNeeded(pipe, r, c);
                        if (pipe != modifiedPipe)
                        {
                            // _logger.LogInformation($"Pipe {pipe} has been modified to {modifiedPipe}");
                            complete = false;
                        }
                        modifiedRow += modifiedPipe;
                    }
                    modifiedMap.Add(modifiedRow);
                }
                _pipeMap = modifiedMap;
            }

        }

        private char Day_10_ModifyPipeIfNeeded(char pipe, int row, int column)
        {
            // _logger.LogInformation($"Modifying pipe {pipe} at ({row}, {column})");
            switch (pipe)
            {
                case '|':
                    return Day_10_PipeAboveIsValid(row, column) && Day_10_PipeBelowIsValid(row, column) ? pipe : '.';
                case '-':
                    return Day_10_PipeToRightIsValid(row, column) && Day_10_PipeToLeftIsValid(row, column) ? pipe : '.';
                case 'L':
                    return Day_10_PipeAboveIsValid(row, column) && Day_10_PipeToRightIsValid(row, column) ? pipe : '.';
                case 'J':
                    return Day_10_PipeAboveIsValid(row, column) && Day_10_PipeToLeftIsValid(row, column) ? pipe : '.';
                case '7':
                    return Day_10_PipeBelowIsValid(row, column) && Day_10_PipeToLeftIsValid(row, column) ? pipe : '.';
                case 'F':
                    return Day_10_PipeBelowIsValid(row, column) && Day_10_PipeToRightIsValid(row, column) ? pipe : '.';
                default:
                    return pipe;
            }
        }

        private void Day_10_FollowPipe(Day_10_Direction wasHeadedInDirection, int row, int column, int previousDistance)
        {
            Day_10_Direction nextDirection = wasHeadedInDirection;
            int currentDistance = previousDistance + 1;

            var complete = false;
            while (!complete)
            {
                if (_pipeMap[row][column] == 'S' || (_pipeMapDistances[row][column] >= 0 && currentDistance >= _pipeMapDistances[row][column]))
                {
                    complete = true;
                    continue;
                }
                _pipeMapDistances[row][column] = currentDistance;
                wasHeadedInDirection = nextDirection;

                var pipe = _pipeMap[row][column];
                if (wasHeadedInDirection == Day_10_Direction.Up)
                {
                    switch (pipe)
                    {
                        case '|':
                            nextDirection = Day_10_Direction.Up;
                            break;
                        case 'F':
                            nextDirection = Day_10_Direction.Right;
                            break;
                        case '7':
                            nextDirection = Day_10_Direction.Left;
                            break;
                    }
                }
                else if (wasHeadedInDirection == Day_10_Direction.Right)
                {
                    switch (pipe)
                    {
                        case '-':
                            nextDirection = Day_10_Direction.Right;
                            break;
                        case '7':
                            nextDirection = Day_10_Direction.Down;
                            break;
                        case 'J':
                            nextDirection = Day_10_Direction.Up;
                            break;
                    }
                }
                else if (wasHeadedInDirection == Day_10_Direction.Down)
                {
                    switch (pipe)
                    {
                        case '|':
                            nextDirection = Day_10_Direction.Down;
                            break;
                        case 'L':
                            nextDirection = Day_10_Direction.Right;
                            break;
                        case 'J':
                            nextDirection = Day_10_Direction.Left;
                            break;
                    }
                }
                else if (wasHeadedInDirection == Day_10_Direction.Left)
                {
                    switch (pipe)
                    {
                        case '-':
                            nextDirection = Day_10_Direction.Left;
                            break;
                        case 'L':
                            nextDirection = Day_10_Direction.Up;
                            break;
                        case 'F':
                            nextDirection = Day_10_Direction.Down;
                            break;
                    }
                }

                row = row + (nextDirection == Day_10_Direction.Up ? -1 : 0) + (nextDirection == Day_10_Direction.Down ? 1 : 0);
                column = column + (nextDirection == Day_10_Direction.Left ? -1 : 0) + (nextDirection == Day_10_Direction.Right ? 1 : 0);
                currentDistance++;
            }

            _logger.LogWarning(string.Join("\n", _pipeMapDistances.Select(d => string.Join("\t", d))));
        }

        private enum Day_10_Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        private bool Day_10_PipeAboveIsValid(int row, int column)
        {
            var index = row - 1;
            if (row == 0)
            {
                return false;
            }

            var pipeAbove = _pipeMap[index][column];
            var acceptablePipesAbove = "|7FS";
            if (!acceptablePipesAbove.Contains(pipeAbove))
            {
                return false;
            }

            return true;
        }

        private bool Day_10_PipeBelowIsValid(int row, int column)
        {
            if (row == _pipeMap.Count - 1)
            {
                return false;
            }

            var pipeBelow = _pipeMap[row + 1][column];
            var acceptablePipesBelow = "|LJS";
            if (!acceptablePipesBelow.Contains(pipeBelow))
            {
                return false;
            }

            return true;
        }

        private bool Day_10_PipeToRightIsValid(int row, int column)
        {
            if (column == _pipeMap[row].Length - 1)
            {
                return false;
            }

            var pipeToRight = _pipeMap[row][column + 1];
            var acceptablePipesToRight = "-J7S";
            if (!acceptablePipesToRight.Contains(pipeToRight))
            {
                return false;
            }

            return true;
        }

        private bool Day_10_PipeToLeftIsValid(int row, int column)
        {
            if (column == 0)
            {
                return false;
            }

            var pipeToLeft = _pipeMap[row][column - 1];
            var acceptablePipesToLeft = "-LFS";
            if (!acceptablePipesToLeft.Contains(pipeToLeft))
            {
                return false;
            }

            return true;
        }

        public string Solve_10_B()
        {
            var input = GetInputForFileAsString(_filename);

            _pipeMap = input.Split("\n").Select(l => l.Trim()).ToList();
            Day_10_RemoveExtraneousPipes();

            var startingRow = _pipeMap.FindIndex(r => r.Contains("S"));
            var startingColumn = _pipeMap[startingRow].IndexOf("S");
            _logger.LogInformation($"Starting at coordinate ({startingRow}, {startingColumn})");
            _logger.LogInformation(string.Join("\n", _pipeMap));

            var completeLeftRight = false;
            var completeTopBottom = false;
            var topBound = 0;
            var bottomBound = _pipeMap.Count - 1;
            var leftBound = 0;
            var rightBound = _pipeMap[bottomBound].Length - 1;
            while (!completeLeftRight && !completeTopBottom)
            {
                for (int r = topBound; r <= bottomBound; r++)
                {
                    for (int c = leftBound; c <= rightBound; c++)
                    {
                        Day_10_ModifySpace(r, c);
                    }
                }

                if (!completeTopBottom)
                {
                    topBound++;
                    bottomBound--;
                    completeTopBottom = topBound > bottomBound;
                }

                if (!completeLeftRight)
                {
                    leftBound++;
                    rightBound--;
                    completeLeftRight = leftBound > rightBound;
                }
            }

            Thread.Sleep(2000);
            _logger.LogInformation($"Updated map...\n" + string.Join("\n", _pipeMap));

            var enclosedSpaces = _pipeMap.Sum(r => r.Count(p => p == '.'));

            return enclosedSpaces.ToString();
        }

        private bool Day_10_ModifySpace(int row, int column)
        {
            var spaceIsNotEmpty = _pipeMap[row][column] != '.';
            if (spaceIsNotEmpty)
            {
                return false;
            }

            var spaceIsOnBorder = row == 0 || column == 0 || row == _pipeMap.Count - 1 || column == _pipeMap[row].Trim().Length - 1;
            if (spaceIsOnBorder)
            {
                _pipeMap[row] = _pipeMap[row].Remove(column, 1).Insert(column, "0");
                return true;
            }

            if (Day_10_SomeSpaceAboveIsEmpty(row, column))
            {
                _pipeMap[row] = _pipeMap[row].Remove(column, 1).Insert(column, "0");
                return true;
            }

            if (Day_10_SomeSpaceToLeftIsEmpty(row, column) || Day_10_SomeSpaceToRightIsEmpty(row, column))
            {
                _pipeMap[row] = _pipeMap[row].Remove(column, 1).Insert(column, "0");
                return true;
            }

            if (Day_10_SomeSpaceBelowIsEmpty(row, column))
            {
                _pipeMap[row] = _pipeMap[row].Remove(column, 1).Insert(column, "0");
                return true;
            }

            if (Day_10_IsOutsideLoop(row, column))
            {
                _pipeMap[row] = _pipeMap[row].Remove(column, 1).Insert(column, "0");
                return true;
            }

            return false;
        }

        private bool Day_10_SomeSpaceAboveIsEmpty(int startingRow, int startingColumn)
        {
            int row = startingRow;
            int column = startingColumn;
            var oneOrMoreSpacesAboveIsEmpty = _pipeMap[row - 1][column - 1] == '0' || _pipeMap[row - 1][column] == '0' || _pipeMap[row - 1][column + 1] == '0';
            if (oneOrMoreSpacesAboveIsEmpty)
            {
                return true;
            }
            return false;
        }

        private bool Day_10_SomeSpaceBelowIsEmpty(int startingRow, int startingColumn)
        {
            int row = startingRow;
            int column = startingColumn;
            var oneOrMoreSpacesBelowIsEmpty = _pipeMap[row + 1][column - 1] == '0' || _pipeMap[row + 1][column] == '0' || _pipeMap[row + 1][column + 1] == '0';
            if (oneOrMoreSpacesBelowIsEmpty)
            {
                return true;
            }
            return false;
        }

        private bool Day_10_SomeSpaceToLeftIsEmpty(int startingRow, int startingColumn)
        {
            int row = startingRow;
            int column = startingColumn;
            var oneOrMoreSpacesToLeftIsEmpty = _pipeMap[row][column - 1] == '0';
            if (oneOrMoreSpacesToLeftIsEmpty)
            {
                return true;
            }

            return false;
        }

        private bool Day_10_SomeSpaceToRightIsEmpty(int startingRow, int startingColumn)
        {
            int row = startingRow;
            int column = startingColumn;
            var oneOrMoreSpacesToRightIsEmpty = _pipeMap[row][column + 1] == '0';
            if (oneOrMoreSpacesToRightIsEmpty)
            {
                return true;
            }
            return false;
        }

        private bool Day_10_IsOutsideLoop(int row, int column)
        {
            return Day_10_CountWallsUp(row, column) % 2 == 0 ||
                    Day_10_CountWallsDown(row, column) % 2 == 0 ||
                    Day_10_CountWallsLeft(row, column) % 2 == 0 ||
                    Day_10_CountWallsRight(row, column) % 2 == 0;
        }

        private int Day_10_CountWallsUp(int row, int column)
        {
            string path = "";
            for (int r = 0; r < row; r++)
            {
                path += _pipeMap[r][column];
            }
            path = Regex.Replace(Regex.Replace(path, @"F\|*L|7\|*J", string.Empty), @"F\|*J|7\|*L", "-");
            return path.Count(x => x == '-');
        }

        private int Day_10_CountWallsDown(int row, int column)
        {
            string path = "";
            for (int r = row + 1; r < _pipeMap.Count; r++)
            {
                path += _pipeMap[r][column];
            }
            path = Regex.Replace(Regex.Replace(path, @"F\|*L|7\|*J", string.Empty), @"F\|*J|7\|*L", "-");
            return path.Count(x => x == '-');
        }

        private int Day_10_CountWallsLeft(int row, int column)
        {
            var path = _pipeMap[row].Substring(0, column + 1);
            path = Regex.Replace(Regex.Replace(path, "F-*7|L-*J", string.Empty), "F-*J|L-*7", "|");
            return path.Count(x => x == '|');
        }

        private int Day_10_CountWallsRight(int row, int column)
        {
            var path = _pipeMap[row].Substring(column + 1);
            path = Regex.Replace(Regex.Replace(path, "F-*7|L-*J", string.Empty), "F-*J|L-*7", "|");
            return path.Count(x => x == '|');
        }

        public string Solve_11_A()
        {
            var input = GetInputForFileAsString(_filename);

            var map = input.Split("\n").ToList();
            var expandedMap = Day_11_ExpandMap(map);

            var galaxies = new List<(int, int)>();
            for (int r = 0; r < expandedMap.Count; r++)
            {
                for (int c = 0; c < expandedMap[r].Length; c++)
                {
                    if (expandedMap[r][c] == '#')
                    {
                        galaxies.Add((r, c));
                    }
                }
            }

            int sum = 0;
            List<string> logs = new List<string>();
            for (int a = 0; a < galaxies.Count; a++)
            {
                for (int b = a + 1; b < galaxies.Count; b++)
                {
                    var distance = Day_11_GetDistanceBetweenGalaxies(galaxies[a], galaxies[b]);
                    logs.Add($"({galaxies[a].Item1}, {galaxies[a].Item2}) to ({galaxies[b].Item1}, {galaxies[b].Item2}) = {distance}");
                    sum += distance;
                }
            }

            return sum.ToString();
        }

        private int Day_11_GetDistanceBetweenGalaxies((int Row, int Column) firstGalaxy, (int Row, int Column) secondGalaxy)
        {
            return Math.Abs(firstGalaxy.Row - secondGalaxy.Row) + Math.Abs(firstGalaxy.Column - secondGalaxy.Column);
        }

        private double Day_11_GetDistanceBetweenGalaxies_PartB(List<string> map, (int Row, int Column) firstGalaxy, (int Row, int Column) secondGalaxy, double factor)
        {
            var verticalExpansions = 0;
            for (int r = Math.Min(firstGalaxy.Row, secondGalaxy.Row); r < Math.Max(firstGalaxy.Row, secondGalaxy.Row); r++)
            {
                if (map[r].Trim().All(x => x == 'X'))
                {
                    verticalExpansions++;
                }
            }

            var horizontalExpansion = 0;
            for (int c = Math.Min(firstGalaxy.Column, secondGalaxy.Column); c < Math.Max(firstGalaxy.Column, secondGalaxy.Column); c++)
            {
                if (map[0][c] == 'X')
                {
                    horizontalExpansion++;
                }
            }

            return Math.Abs(firstGalaxy.Row - secondGalaxy.Row) + verticalExpansions * (factor - 1.0)
                    + Math.Abs(firstGalaxy.Column - secondGalaxy.Column) + horizontalExpansion * (factor - 1.0);
        }

        private List<string> Day_11_ExpandMap(List<string> map)
        {
            var expandedMap = map;
            for (int r = 0; r < expandedMap.Count; r++)
            {
                if (!expandedMap[r].Trim().Any(x => x != '.'))
                {
                    _logger.LogWarning($"Expanding galaxy vertically");
                    expandedMap.Insert(r, expandedMap[r]);
                    r++;
                }
            }

            for (int c = 0; c < expandedMap[0].Trim().Length; c++)
            {
                var column = expandedMap.Select(row => row[c]);
                if (!column.Any(x => x != '.'))
                {
                    _logger.LogWarning($"Expanding galaxy horizontally");
                    for (int r = 0; r < expandedMap.Count; r++)
                    {
                        expandedMap[r] = expandedMap[r].Insert(c, ".");
                    }
                    c++;
                }
            }
            return expandedMap;
        }

        private List<string> Day_11_ExpandMap_PartB(List<string> map)
        {
            var expandedMap = map;
            for (int r = 0; r < expandedMap.Count; r++)
            {
                if (!expandedMap[r].Trim().Any(x => !".X".Contains(x)))
                {
                    _logger.LogWarning($"Expanding galaxy vertically by factor of X");
                    expandedMap[r] = expandedMap[r].Replace(".", "X");
                }
            }

            for (int c = 0; c < expandedMap[0].Trim().Length; c++)
            {
                var column = expandedMap.Select(row => row[c]);
                if (!column.Any(x => !".X".Contains(x)))
                {
                    _logger.LogWarning($"Expanding galaxy horizontally by a factor of X");
                    for (int r = 0; r < expandedMap.Count; r++)
                    {
                        expandedMap[r] = expandedMap[r].Remove(c, 1).Insert(c, "X");
                    }
                    c++;
                }
            }
            return expandedMap;
        }

        public string Solve_11_B()
        {
            var input = GetInputForFileAsString(_filename);

            var map = input.Split("\n").ToList();
            var expandedMap = Day_11_ExpandMap_PartB(map);

            var galaxies = new List<(int, int)>();
            for (int r = 0; r < expandedMap.Count; r++)
            {
                for (int c = 0; c < expandedMap[r].Length; c++)
                {
                    if (expandedMap[r][c] == '#')
                    {
                        galaxies.Add((r, c));
                    }
                }
            }

            double sum = 0.0;
            List<string> logs = new List<string>();
            for (int a = 0; a < galaxies.Count; a++)
            {
                for (int b = a + 1; b < galaxies.Count; b++)
                {
                    var distance = Day_11_GetDistanceBetweenGalaxies_PartB(expandedMap, galaxies[a], galaxies[b], 1000000.0);
                    logs.Add($"({galaxies[a].Item1}, {galaxies[a].Item2}) to ({galaxies[b].Item1}, {galaxies[b].Item2}) = {distance}");
                    sum += distance;
                }
            }

            _logger.LogInformation(string.Join("\n", expandedMap));
            Thread.Sleep(2000);

            return sum.ToString();
        }

        public string Solve_13_A()
        {
            var inputs = GetInputForFileAsString(_filename).Split("\r\n\r\n");

            var sum = 0.0;
            foreach (var input in inputs)
            {
                sum += Day_13_GetScoreForMirrorInput(input);
            }

            return sum.ToString();
        }

        private double Day_13_GetScoreForMirrorInput(string input, int ignoreRow = -1, int ignoreColumn = -1)
        {
            var map = input.Split("\r\n").Select(m => m.Trim()).ToList();
            var score = 0.0;

            for (int r = 0; r + 1 < map.Count; r++)
            {
                if (r == ignoreRow)
                {
                    continue;
                }

                var rowsBelow = map.GetRange(r + 1, map.Count() - r - 1);
                var rowsAbove = map.GetRange(0, r + 1);
                rowsAbove.Reverse();

                var isMatch = true;
                for (int i = 0; i < rowsBelow.Count() && i < rowsAbove.Count(); i++)
                {
                    isMatch = isMatch && rowsBelow[i].Trim() == rowsAbove[i].Trim();
                    if (!isMatch)
                    {
                        break;
                    }
                }

                if (isMatch)
                {
                    // _logger.LogInformation($"Horizontal axis found with {rowsAbove.Count()} rows above it.");
                    score += 100.0 * rowsAbove.Count();
                    break;
                }
            }

            var numberOfColumns = map.Min(r => r.Length);
            for (int c = 0; c + 1 < numberOfColumns; c++)
            {
                if (c == ignoreColumn)
                {
                    continue;
                }

                var isMatch = true;
                for (int r = 0; r < map.Count; r++)
                {
                    if (string.IsNullOrWhiteSpace(map[r]) || c + 1 > map[r].Length - 1)
                    {
                        continue;
                    }
                    var stringToRight = map[r].Trim().Substring(c + 1).Trim();
                    var stringToLeft = map[r].Trim().Substring(0, c + 1).Trim();
                    var charsToLeft = stringToLeft.ToCharArray();
                    Array.Reverse(charsToLeft);
                    stringToLeft = new string(charsToLeft);

                    var length = Math.Min(stringToLeft.Length, stringToRight.Length);
                    stringToLeft = stringToLeft.Substring(0, length);
                    stringToRight = stringToRight.Substring(0, length);

                    isMatch = isMatch && stringToLeft == stringToRight;
                    if (!isMatch)
                    {
                        break;
                    }

                }

                if (isMatch)
                {
                    // _logger.LogInformation($"Vertical axis found with {c + 1} columns to left of it.");
                    score += c + 1;
                    break;
                }
            }

            return score;
        }

        public string Solve_13_B()
        {
            // Unsuccessful with brute force attempt
            // Advice from Reddit: Try Linq.Zip to count the pairs of rows with only 1 difference

            var inputs = GetInputForFileAsString(_filename).Split("\r\n\r\n");

            var sum = 0.0;
            var puzzleNumber = 1;
            foreach (var input in inputs.Select(i => i.Trim()))
            {
                var originalScore = Day_13_GetScoreForMirrorInput(input);
                var ignoreRow = originalScore >= 100 ? (int)(originalScore / 100) - 1 : -1;
                var ignoreColumn = originalScore < 100 ? (int)originalScore - 1 : -1;

                var newScore = originalScore;
                for (int x = 0; x < input.Length; x++)
                {
                    var newCharacter = input[x] == '#' ? "." : "#";
                    var newInput = input.Remove(x, 1).Insert(x, newCharacter);
                    newScore = Day_13_GetScoreForMirrorInput(newInput, ignoreRow, ignoreColumn);
                    if (newScore != originalScore && newScore > 0)
                    {
                        break;
                    }
                }
                _logger.LogInformation($"Puzzle {puzzleNumber}: Score changed from {originalScore} to {newScore}");
                sum += newScore;
                puzzleNumber++;
            }

            return sum.ToString();
        }

        public string Solve_14_A()
        {
            var dish = GetInputForFileAsString(_filename).Split("\r\n").ToList();
            Day_14_PrintDish(dish);

            var tiltedDish = Day_14_TiltDishNorth(dish);
            var score = Day_14_ScoreDish(tiltedDish);

            Day_14_PrintDish(tiltedDish);

            return score.ToString();
        }

        public enum Day_14_Direction
        {
            West,
            North,
            East,
            South
        }

        private void Day_14_PrintDish(List<string> dish)
        {
            _logger.LogInformation($"\n" + string.Join("\n", dish));
            Thread.Sleep(1000);
        }

        public List<string> Day_14_TiltDishNorth(List<string> dish)
        {
            var tiltedDish = dish;
            for (int r = 1; r < tiltedDish.Count; r++)
            {
                for (int c = 0; c < tiltedDish[r].Length; c++)
                {
                    if (tiltedDish[r][c] != 'O')
                    {
                        continue;
                    }

                    if (tiltedDish[r - 1][c] == '.')
                    {
                        int row = GetHighestEmptyRowIndex(tiltedDish, r - 1, c);
                        tiltedDish[row] = tiltedDish[row].Remove(c, 1).Insert(c, "O");
                        tiltedDish[r] = tiltedDish[r].Remove(c, 1).Insert(c, ".");
                    }
                }
            }
            return tiltedDish;
        }

        private int GetHighestEmptyRowIndex(List<string> dish, int startingRow, int column)
        {
            if (startingRow == 0)
            {
                return startingRow;
            }
            for (int r = startingRow - 1; r >= 0; r--)
            {
                if (dish[r][column] == '#' || dish[r][column] == 'O')
                {
                    return r + 1;
                }
                if (r == 0)
                {
                    return r;
                }
            }
            return startingRow;
        }

        private int GetLowestEmptyRowIndex(List<string> dish, int startingRow, int column)
        {
            if (startingRow == dish.Count - 1)
            {
                return startingRow;
            }
            for (int r = startingRow + 1; r <= dish.Count - 1; r++)
            {
                if (dish[r][column] == '#' || dish[r][column] == 'O')
                {
                    return r - 1;
                }
                if (r == dish.Count - 1)
                {
                    return r;
                }
            }
            return startingRow;
        }

        public List<string> Day_14_TiltDishSouth(List<string> dish)
        {
            var tiltedDish = dish;
            for (int r = tiltedDish.Count - 2; r >= 0; r--)
            {
                for (int c = 0; c < tiltedDish[r].Length; c++)
                {
                    if (tiltedDish[r][c] != 'O')
                    {
                        continue;
                    }

                    if (tiltedDish[r + 1][c] == '.')
                    {
                        int row = GetLowestEmptyRowIndex(tiltedDish, r + 1, c);
                        tiltedDish[row] = tiltedDish[row].Remove(c, 1).Insert(c, "O");
                        tiltedDish[r] = tiltedDish[r].Remove(c, 1).Insert(c, ".");
                    }
                }
            }
            return tiltedDish;
        }

        public List<string> Day_14_TiltDishWest(List<string> dish)
        {
            var tiltedDish = dish;
            for (int r = 0; r < tiltedDish.Count; r++)
            {
                var modifiedRow = tiltedDish[r].Trim();
                while (modifiedRow.Contains(".O"))
                {
                    modifiedRow = modifiedRow.Replace(".O", "O.");
                }
                tiltedDish[r] = modifiedRow;
            }
            return tiltedDish;
        }

        public List<string> Day_14_TiltDishEast(List<string> dish)
        {
            var tiltedDish = dish;
            for (int r = 0; r < tiltedDish.Count; r++)
            {
                var modifiedRow = tiltedDish[r].Trim();
                while (modifiedRow.Contains("O."))
                {
                    modifiedRow = modifiedRow.Replace("O.", ".O");
                }
                tiltedDish[r] = modifiedRow;
            }
            return tiltedDish;
        }

        public List<string> Day_14_SpinDish(List<string> dish)
        {
            var spunDish = dish;
            spunDish = Day_14_TiltDishNorth(spunDish);
            spunDish = Day_14_TiltDishWest(spunDish);
            spunDish = Day_14_TiltDishSouth(spunDish);
            spunDish = Day_14_TiltDishEast(spunDish);

            return spunDish;
        }

        public double Day_14_ScoreDish(List<string> dish)
        {
            var score = 0.0;
            for (int r = 0; r < dish.Count; r++)
            {
                score += (dish.Count - r) * dish[r].Count(c => c == 'O');
            }
            return score;
        }

        private Dictionary<string, double> Day_14_B_DishCache = new Dictionary<string, double>();
        public string Solve_14_B()
        {
            var dish = GetInputForFileAsString(_filename).Split("\r\n").ToList();
            var spunDish = dish;

            Day_14_B_DishCache.Add(string.Join("", spunDish.SelectMany(x => x)), 0.0);
            var i = 1.0;
            var limit = 1000000000.0;
            while (i <= limit)
            {
                spunDish = Day_14_SpinDish(spunDish);
                var stringRepresentation = string.Join("", spunDish.SelectMany(x => x));

                if (Day_14_B_DishCache.ContainsKey(stringRepresentation))
                {
                    var oldConfigurationIndex = Day_14_B_DishCache[stringRepresentation];
                    _logger.LogWarning($"This dish configuration already existed at index {oldConfigurationIndex} (currently on index {i})");

                    var frequency = i - oldConfigurationIndex;
                    var startingPoint = i;
                    var offset = (Math.Round((limit - startingPoint) / frequency, 0) - 1) * frequency;
                    offset = offset > 0 ? offset : 0;
                    _logger.LogInformation($"Skipping {offset} iterations (index {i} -> {i + offset})");
                    i += offset;
                }
                else
                {
                    Day_14_B_DishCache.Add(stringRepresentation, i);
                }
                i++;
            }

            var score = Day_14_ScoreDish(spunDish);

            return score.ToString();
        }

        public string Solve_15_A()
        {
            var input = GetInputForFileAsString(_filename).Replace("\r\n", ",").Split(",").ToList();
            var sum = input.Sum(i => Day_15_GetAsciiScore(i));
            return sum.ToString();
        }

        private int Day_15_GetAsciiScore(string input)
        {
            var score = 0;
            foreach (var character in input.Trim())
            {
                score += (int)character;
                score *= 17;
                score %= 256;
            }
            return score;
        }

        public string Solve_15_B()
        {
            var input = GetInputForFileAsString(_filename).Replace("\r\n", ",").Split(",").ToList();
            var boxes = new Dictionary<int, List<string>>();
            for (int i = 0; i <= 255; i++)
            {
                boxes.Add(i, new List<string>());
            }

            foreach (var instruction in input)
            {
                var label = instruction.Split("-").First().Split("=").First().Trim();
                var boxNumber = Day_15_GetAsciiScore(label);
                var shouldRemove = instruction.Contains('-');
                var formattedLens = instruction.Trim().Replace("=", " ");

                if (shouldRemove && boxes[boxNumber].Any(v => v.Contains(label)))
                {
                    var index = boxes[boxNumber].FindIndex(v => v.Contains(label));
                    boxes[boxNumber].RemoveAt(index);
                }

                if (!shouldRemove)
                {
                    if (boxes[boxNumber].Any(v => v.Contains(label)))
                    {
                        var index = boxes[boxNumber].FindIndex(v => v.Contains(label));
                        boxes[boxNumber].RemoveAt(index);
                        boxes[boxNumber].Insert(index, formattedLens);
                    }
                    else
                    {
                        boxes[boxNumber].Add(formattedLens);
                    }
                }
            }

            var score = boxes.Where(b => b.Value.Any()).Sum(b => Day_15_ScoreBox(b));

            _logger.LogInformation($"Boxes:\n" + string.Join("\n", boxes.Where(b => b.Value.Any()).Select(b => $"Box {b.Key}: " + string.Join(",", b.Value))));
            Thread.Sleep(1000);
            return score.ToString();
        }

        private double Day_15_ScoreBox(KeyValuePair<int, List<string>> input)
        {
            var score = 0.0;
            for (int i = 0; i < input.Value.Count(); i++)
            {
                score += (input.Key + 1) * (i + 1) * int.Parse(input.Value[i].Last().ToString());
            }
            return score;
        }

        public enum Day16_Direction
        {
            Right,
            Down,
            Left,
            Up
        }
        private List<((int, int), Day16_Direction)> Day_16_LightCache = new List<((int, int), Day16_Direction)>();
        private List<(int, int)> Day_16_EnergizedMap = new List<(int, int)>();
        private List<string> Day_16_InputMap = new List<string>();
        public string Solve_16_A()
        {
            Day_16_InputMap = GetInputForFileAsString(_filename).Split("\r\n").ToList();

            var energizedSum = GetScoreForStart(((0, 0), Day16_Direction.Right));

            return energizedSum.ToString();
        }

        private double GetScoreForStart(((int, int), Day16_Direction) start)
        {
            Day_16_EnergizedMap.Clear();
            Day_16_LightCache.Clear();
            var nextSteps = new List<((int, int), Day16_Direction)>()
            {
                start
            };

            while (nextSteps.Any())
            {
                var nextStep = nextSteps.First();
                nextSteps.RemoveAt(0);
                nextSteps.AddRange(GetNextSteps(nextStep.Item1.Item1, nextStep.Item1.Item2, nextStep.Item2));
            }

            return Day_16_EnergizedMap.Count();
        }

        private List<((int, int), Day16_Direction)> GetNextSteps(int row, int column, Day16_Direction currentDirection)
        {
            var nextSteps = new List<((int, int), Day16_Direction)>();

            if (row < 0 || row >= Day_16_InputMap.Count() || column < 0 || column >= Day_16_InputMap.First().Length)
            {
                return nextSteps;
            }

            if (Day_16_LightCache.Contains(((row, column), currentDirection)))
            {
                return nextSteps;
            }
            else
            {
                Day_16_LightCache.Add(((row, column), currentDirection));
            }

            if (!Day_16_EnergizedMap.Any(e => e.Item1 == row && e.Item2 == column))
            {
                Day_16_EnergizedMap.Add((row, column));
            }

            var currentCharacter = Day_16_InputMap[row][column];
            if (currentCharacter == '|' && (currentDirection == Day16_Direction.Down || currentDirection == Day16_Direction.Up))
            {
                currentCharacter = '.';
            }
            else if (currentCharacter == '-' && (currentDirection == Day16_Direction.Right || currentDirection == Day16_Direction.Left))
            {
                currentCharacter = '.';
            }

            var nextDirection = currentDirection;
            switch (currentCharacter)
            {
                case '.':
                    nextSteps.Add((GetNextCoordinate(row, column, currentDirection), currentDirection));
                    break;
                case '\\':
                    nextDirection = GetNextDirectionForSlash(currentDirection, true);
                    nextSteps.Add((GetNextCoordinate(row, column, nextDirection), nextDirection));
                    break;
                case '/':
                    nextDirection = GetNextDirectionForSlash(currentDirection, false);
                    nextSteps.Add((GetNextCoordinate(row, column, nextDirection), nextDirection));
                    break;
                case '|':
                    nextSteps.Add((GetNextCoordinate(row, column, Day16_Direction.Up), Day16_Direction.Up));
                    nextSteps.Add((GetNextCoordinate(row, column, Day16_Direction.Down), Day16_Direction.Down));
                    break;
                case '-':
                    nextSteps.Add((GetNextCoordinate(row, column, Day16_Direction.Right), Day16_Direction.Right));
                    nextSteps.Add((GetNextCoordinate(row, column, Day16_Direction.Left), Day16_Direction.Left));
                    break;
                default:
                    break;
            }

            return nextSteps;
        }

        private (int, int) GetNextCoordinate(int row, int column, Day16_Direction currentDirection)
        {
            int nextRow = row;
            int nextColumn = column;
            switch (currentDirection)
            {
                case Day16_Direction.Right:
                    nextColumn++;
                    break;
                case Day16_Direction.Down:
                    nextRow++;
                    break;
                case Day16_Direction.Left:
                    nextColumn--;
                    break;
                case Day16_Direction.Up:
                    nextRow--;
                    break;
                default:
                    break;
            }

            return (nextRow, nextColumn);
        }

        private Day16_Direction GetNextDirectionForSlash(Day16_Direction currentDirection, bool isBackSlash = false)
        {
            if (isBackSlash)
            {
                // \
                switch (currentDirection)
                {
                    case Day16_Direction.Right:
                        return Day16_Direction.Down;
                    case Day16_Direction.Down:
                        return Day16_Direction.Right;
                    case Day16_Direction.Left:
                        return Day16_Direction.Up;
                    case Day16_Direction.Up:
                        return Day16_Direction.Left;
                    default:
                        return currentDirection;
                }
            }
            else
            {
                // /
                switch (currentDirection)
                {
                    case Day16_Direction.Right:
                        return Day16_Direction.Up;
                    case Day16_Direction.Down:
                        return Day16_Direction.Left;
                    case Day16_Direction.Left:
                        return Day16_Direction.Down;
                    case Day16_Direction.Up:
                        return Day16_Direction.Right;
                    default:
                        return currentDirection;
                }
            }
        }

        public string Solve_16_B()
        {
            Day_16_InputMap = GetInputForFileAsString(_filename).Split("\r\n").ToList();
            var energizedSum = 0.0;

            var startingSteps = new List<((int, int), Day16_Direction)>();
            for (int r = 0; r < Day_16_InputMap.Count(); r++)
            {
                startingSteps.Add(((r, 0), Day16_Direction.Right));
                startingSteps.Add(((r, Day_16_InputMap.First().Length - 1), Day16_Direction.Left));
            }
            for (int c = 0; c < Day_16_InputMap.First().Length; c++)
            {
                startingSteps.Add(((0, c), Day16_Direction.Down));
                startingSteps.Add(((Day_16_InputMap.Count() - 1, c), Day16_Direction.Up));
            }

            foreach (var start in startingSteps.OrderBy(s => s.Item1.Item1).ThenBy(s => s.Item1.Item2))
            {
                var thisEnergizedSum = GetScoreForStart(start);

                if (thisEnergizedSum > energizedSum)
                {
                    _logger.LogWarning($"Higher energy sum found at ({start.Item1.Item1}, {start.Item1.Item2}) with direction {start.Item2}. Sum = {thisEnergizedSum}");
                    energizedSum = thisEnergizedSum;
                    Thread.Sleep(250);
                }
            }

            return energizedSum.ToString();
        }

        private List<((int, int) To, (int, int) From)> Day17_CameFromLocation = new List<((int, int), (int, int))>();
        private Dictionary<(int, int), double> Day17_CostSoFar = new Dictionary<(int, int), double>();
        private Dictionary<(int, int), int> Day17_HeuristicMap = new Dictionary<(int, int), int>();
        private int Day17_MapHeight = 0;
        private int Day17_MapWidth = 0;
        private (int Row, int Column) _target = (0, 0);
        public string Solve_17_A()
        {
            var input = GetInputForFileAsString(_filename).Split("\r\n").ToList();
            Day17_MapHeight = input.Count();
            Day17_MapWidth = input.First().Length;
            _target = (Day17_MapHeight - 1, Day17_MapWidth - 1);

            for (int r = 0; r < Day17_MapHeight; r++)
            {
                for (int c = 0; c < Day17_MapWidth; c++)
                {
                    Day17_HeuristicMap.Add((r, c), int.Parse(input[r][c].ToString()));
                }
            }

            try
            {
                FillOutCostOfPoint((0, 0), (-100, -100), 0.0, 0);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.StackTrace);
            }

            // _logger.LogInformation(string.Join($"\n", Day17_CostSoFar.OrderBy(x => x.Key.Item1)
            //                                                         .ThenBy(x => x.Key.Item2)
            //                                                         .Select(x => $"{x.Key}\t{x.Value}")));
            PrintCosts();
            Thread.Sleep(1000);

            if (!Day17_CostSoFar.Any(x => x.Key == _target))
            {
                _logger.LogError($"Target not reached.");
                return "Target not reached.";
            }

            return Day17_CostSoFar[_target].ToString();
        }

        private void FillOutCostOfPoint((int Row, int Column) point, (int Row, int Column) previousPoint, double costSoFar, int numberOfStraightsInARow = 0)
        {
            if (Day17_CameFromLocation.Any(x => x.To == point && x.From == previousPoint))
            {
                return;
            }
            Day17_CameFromLocation.Add((point, previousPoint));

            var originalCost = costSoFar;
            if (Day17_CostSoFar.Any(c => c.Key == point))
            {
                originalCost = Day17_CostSoFar[point];
                if (originalCost < costSoFar)
                {
                    return;
                }
                Day17_CostSoFar[point] = costSoFar;
            }
            else
            {
                Day17_CostSoFar.Add(point, costSoFar);
            }

            if (point.Row == _target.Row && point.Column == _target.Column)
            {
                _logger.LogInformation($"Target found!");
                return;
            }

            var neighbors = Day17_GetValidNeighbors(point.Row, point.Column).OrderBy(p => GetRelativeDistanceBetweenPoints(point, _target));
            foreach (var neighbor in neighbors)
            {
                var nextCost = GetHeuristicCostOfPoint(neighbor.Row, neighbor.Column);
                var continuesToGoStraight = (point.Row == neighbor.Row && point.Row == previousPoint.Row) || (point.Column == neighbor.Column && point.Column == previousPoint.Column);
                var newNumberOfStraightInRow = continuesToGoStraight ? numberOfStraightsInARow + 1 : 0;
                if (newNumberOfStraightInRow >= 3)
                {
                    continue;
                }
                FillOutCostOfPoint(neighbor, point, costSoFar + nextCost, newNumberOfStraightInRow);
                Thread.Sleep(100);
            }
        }

        private List<(int Row, int Column)> Day17_GetValidNeighbors(int row, int column)
        {
            var neighbors = new List<(int, int)>();
            if (row > 0 && !Day17_CameFromLocation.Any(x => x.To == (row - 1, column) && x.From == (row, column)))
            {
                neighbors.Add((row - 1, column));
            }
            if (row < Day17_MapHeight - 1 && !Day17_CameFromLocation.Any(x => x.To == (row + 1, column) && x.From == (row, column)))
            {
                neighbors.Add((row + 1, column));
            }
            if (column > 0 && !Day17_CameFromLocation.Any(x => x.To == (row, column - 1) && x.From == (row, column)))
            {
                neighbors.Add((row, column - 1));
            }
            if (column < Day17_MapWidth - 1 && !Day17_CameFromLocation.Any(x => x.To == (row, column + 1) && x.From == (row, column)))
            {
                neighbors.Add((row, column + 1));
            }
            return neighbors;
        }

        private int GetRelativeDistanceBetweenPoints((int Row, int Column) firstPoint, (int Row, int Column) secondPoint)
        {
            return Math.Abs(firstPoint.Row - secondPoint.Row) + Math.Abs(firstPoint.Column - secondPoint.Column);
        }

        private int GetHeuristicCostOfPoint(int row, int column)
        {
            return Day17_HeuristicMap[(row, column)];
        }

        private void PrintCosts()
        {
            var lines = new List<string>();
            for (int r = 0; r < Day17_MapHeight; r++)
            {
                var values = new List<double>();
                for (int c = 0; c < Day17_MapWidth; c++)
                {
                    values.Add(Day17_CostSoFar[(r, c)]);
                }
                lines.Add(string.Join("\t", values.Select(v => v.ToString())));
            }
            _logger.LogInformation(string.Join("\n", lines));
        }

        public string Solve_17_B()
        {
            var input = GetInputForFileAsString(_filename).Split("\r\n").ToList();

            return "";
        }

        List<(int X, int Y)> PerimeterPoints = new() { (0, 0) };
        public class Day18_Instruction
        {
            public Day16_Direction Direction;
            public int NumberOfSpaces;
            public string Color;

            public Day18_Instruction(string input)
            {
                var components = input.Trim().Split(" ");

                NumberOfSpaces = int.Parse(components[1].Trim());
                Color = components.Last().Trim().Trim(')').Trim('(').Trim('#');

                switch (components.First().Trim())
                {
                    case "R":
                        Direction = Day16_Direction.Right;
                        break;
                    case "L":
                        Direction = Day16_Direction.Left;
                        break;
                    case "U":
                        Direction = Day16_Direction.Up;
                        break;
                    case "D":
                        Direction = Day16_Direction.Down;
                        break;
                    default:
                        break;
                }
            }

            public void AdjustForPartB()
            {
                NumberOfSpaces = int.Parse(Color.Substring(0, 5), NumberStyles.HexNumber);
                Direction = (Day16_Direction)int.Parse(Color.Last().ToString());
            }
        }
        private int Day18_CurrentRow = 0;
        private int Day18_CurrentColumn = 0;
        public string Solve_18_A()
        {
            var input = GetInputForFileAsString(_filename).Split("\r\n").ToList();
            var instructions = new List<Day18_Instruction>();
            foreach (var line in input)
            {
                instructions.Add(new Day18_Instruction(line));
            }

            var perimeterArea = 0.0;
            foreach (var instruction in instructions)
            {
                perimeterArea += instruction.NumberOfSpaces;
                PerimeterPoints.Add(Day18_ExecuteDirection(instruction.NumberOfSpaces, instruction.Direction));
            }

            var enclosedArea = Day18_CalculateAreaUsingTriangleFormula(PerimeterPoints);
            var totalArea = (perimeterArea + enclosedArea) / 2 + 1;

            _logger.LogInformation($"Perimeter: {perimeterArea}\nEnclosed: {enclosedArea}\nTotal: {totalArea}");

            return totalArea.ToString();
        }

        private (int X, int Y) Day18_ExecuteDirection(int numberOfSpaces, Day16_Direction direction)
        {
            switch (direction)
            {
                case Day16_Direction.Right:
                    return Day18_GoRight(numberOfSpaces);
                case Day16_Direction.Down:
                    return Day18_GoDown(numberOfSpaces);
                case Day16_Direction.Left:
                    return Day18_GoLeft(numberOfSpaces);
                case Day16_Direction.Up:
                default:
                    return Day18_GoUp(numberOfSpaces);
            }
        }

        private (int X, int Y) Day18_GoUp(int numberOfSpaces)
        {
            Day18_CurrentRow -= numberOfSpaces;
            return (Day18_CurrentRow, Day18_CurrentColumn);
        }

        private (int X, int Y) Day18_GoDown(int numberOfSpaces)
        {
            Day18_CurrentRow += numberOfSpaces;
            return (Day18_CurrentRow, Day18_CurrentColumn);
        }

        private (int X, int Y) Day18_GoLeft(int numberOfSpaces)
        {
            Day18_CurrentColumn -= numberOfSpaces;
            return (Day18_CurrentRow, Day18_CurrentColumn);
        }

        private (int X, int Y) Day18_GoRight(int numberOfSpaces)
        {
            Day18_CurrentColumn += numberOfSpaces;
            return (Day18_CurrentRow, Day18_CurrentColumn);
        }

        private double Day18_CalculateAreaUsingTriangleFormula(List<(int X, int Y)> perimeterPoints)
        {
            var area = 0.0;
            for (int i = 1; i < perimeterPoints.Count(); i++)
            {
                area += Day18_GetDeterminantOfPoints(perimeterPoints[i - 1], perimeterPoints[i]);
            }
            area += Day18_GetDeterminantOfPoints(perimeterPoints.Last(), perimeterPoints.First());

            return Math.Abs(area);
        }

        private double Day18_GetDeterminantOfPoints((double X, double Y) firstPoint, (double X, double Y) secondPoint)
        {
            return (firstPoint.Y + secondPoint.Y) * (firstPoint.X - secondPoint.X);
        }

        public string Solve_18_B()
        {
            var input = GetInputForFileAsString(_filename).Split("\r\n").ToList();
            var instructions = new List<Day18_Instruction>();
            foreach (var line in input)
            {
                var instruction = new Day18_Instruction(line);
                instruction.AdjustForPartB();
                instructions.Add(instruction);
            }

            var perimeterArea = 0.0;
            foreach (var instruction in instructions)
            {
                perimeterArea += instruction.NumberOfSpaces;
                var destination = Day18_ExecuteDirection(instruction.NumberOfSpaces, instruction.Direction);
                _logger.LogInformation($"Going {instruction.NumberOfSpaces} meters {instruction.Direction}\t\t({destination.X}, {destination.Y})");
                Thread.Sleep(100);
                PerimeterPoints.Add(destination);
            }

            var enclosedArea = Day18_CalculateAreaUsingTriangleFormula(PerimeterPoints);
            var totalArea = (perimeterArea + enclosedArea) / 2 + 1;

            _logger.LogInformation($"Perimeter: {perimeterArea}\nEnclosed: {enclosedArea}\nTotal: {totalArea}");

            return totalArea.ToString();
        }

        public enum Day19_Category
        {
            X,
            M,
            A,
            S
        }

        public class Day19_WorkflowRule
        {
            public Day19_Category Category;
            public bool OperatorIsLessThan;
            public int Limit;
            public string NextWorkflow;

            public Day19_WorkflowRule()
            {
            }

            public Day19_WorkflowRule(string input)
            {
                switch (input.First())
                {
                    case 'x':
                        Category = Day19_Category.X;
                        break;
                    case 'm':
                        Category = Day19_Category.M;
                        break;
                    case 'a':
                        Category = Day19_Category.A;
                        break;
                    case 's':
                    default:
                        Category = Day19_Category.S;
                        break;
                }

                OperatorIsLessThan = input.Contains("<");

                Regex regexToGetLimit = new Regex(@"(?<=[<>])(.*)(?=[:])");
                Limit = int.Parse(regexToGetLimit.Match(input).Value);
                NextWorkflow = input.Split(":").Last();
            }

            public string GetStringRepresentation()
            {
                var categoryString = "";
                switch (Category)
                {
                    case Day19_Category.X:
                        categoryString = "x";
                        break;
                    case Day19_Category.M:
                        categoryString = "m";
                        break;
                    case Day19_Category.A:
                        categoryString = "a";
                        break;
                    case Day19_Category.S:
                        categoryString = "s";
                        break;
                    default:
                        break;
                }

                var operatorSymbol = OperatorIsLessThan ? "<" : ">";
                return $"{categoryString}{operatorSymbol}{Limit}";
            }
        }

        public class Day19_Workflow
        {
            public string Name;
            public List<Day19_WorkflowRule> Rules = new();
            public string DefaultWorkflow;

            public Day19_Workflow() { }

            public Day19_Workflow(string input)
            {
                Name = input.Split("{").First().Trim();
                DefaultWorkflow = input.Split($",").Last().Split("}").First().Trim();
                foreach (var rule in input.Split("{").Last().Split("}").First().Split(",").ToList().Where(r => r.Contains(":")))
                {
                    Rules.Add(new Day19_WorkflowRule(rule));
                }
            }

            public string GetStringRepresentation()
            {
                return $"{Name}: " + string.Join($", ", Rules.Select(r => r.GetStringRepresentation()));
            }
        }

        public class Day19_Part
        {
            public int X;
            public int M;
            public int A;
            public int S;

            public Day19_Part() { }

            public Day19_Part(int x, int m, int a, int s)
            {
                X = x;
                M = m;
                A = a;
                S = s;
            }

            public Day19_Part(string inputs)
            {
                foreach (var input in inputs.Split("{").Last().Split("}").First().Split(",").ToList())
                {
                    var value = int.Parse(input.Split("=").Last());
                    switch (input.First())
                    {
                        case 'x':
                            X = value;
                            break;
                        case 'm':
                            M = value;
                            break;
                        case 'a':
                            A = value;
                            break;
                        case 's':
                        default:
                            S = value;
                            break;
                    }
                }
            }

            public int GetScore()
            {
                return X + M + A + S;
            }

            public int GetScoreForCategory(Day19_Category category)
            {
                switch (category)
                {
                    case Day19_Category.X:
                        return X;
                    case Day19_Category.M:
                        return M;
                    case Day19_Category.A:
                        return A;
                    case Day19_Category.S:
                        return S;
                    default:
                        return 0;
                }
            }

            public string GetStringRepresentation()
            {
                return $"x={X},m={M},a={A},s={S}";
            }
        }

        public class Day19_Range
        {
            public int Minimum;
            public int Maximum;

            public Day19_Range() { }
            public Day19_Range(int min, int max)
            {
                Minimum = min;
                Maximum = max;
            }

            public bool ContainsNumberInclusive(int number)
            {
                return Minimum <= number && number <= Maximum;
            }

            public int GetCount()
            {
                return Maximum - Minimum + 1;
            }

            public string GetStringValue()
            {
                return $"[{Minimum}, {Maximum}]";
            }

            public int GetRandomValue()
            {
                var random = new Random();
                return random.Next(Minimum, Maximum);
            }

            public List<Day19_Range> SplitUpToAndIncluding(int limit)
            {
                var result = new List<Day19_Range>();
                if (limit >= Minimum)
                {
                    result.Add(new Day19_Range(Minimum, Math.Min(Maximum, limit)));
                }
                if (limit < Maximum)
                {
                    result.Add(new Day19_Range(Math.Max(limit + 1, Minimum), Maximum));
                }
                return result;
            }
        }

        public class Day19_RangePart
        {
            public Day19_Range X;
            public Day19_Range M;
            public Day19_Range A;
            public Day19_Range S;

            public Day19_RangePart() { }
            public Day19_RangePart(Day19_Range x, Day19_Range m, Day19_Range a, Day19_Range s)
            {
                X = x;
                M = m;
                A = a;
                S = s;
            }

            public Day19_RangePart Clone()
            {
                return new Day19_RangePart(X, M, A, S);
            }

            public string GetStringValue()
            {
                return $"X {X.GetStringValue()}\tM {M.GetStringValue()}\tA {A.GetStringValue()}\tS {S.GetStringValue()}";
            }

            public Day19_Part GetRandomPart()
            {
                return new Day19_Part(X.GetRandomValue(), M.GetRandomValue(), A.GetRandomValue(), S.GetRandomValue());
            }

            public bool ContainsPoint(int x, int m, int a, int s)
            {
                return X.ContainsNumberInclusive(x) &&
                        M.ContainsNumberInclusive(m) &&
                        A.ContainsNumberInclusive(a) &&
                        S.ContainsNumberInclusive(s);
            }

            public double GetNumberOfCominbations()
            {
                return 1.0 * X.GetCount() * M.GetCount() * A.GetCount() * S.GetCount();
            }

            public Day19_Range GetRangeForCategory(Day19_Category category)
            {
                switch (category)
                {
                    case Day19_Category.X:
                        return X;
                    case Day19_Category.M:
                        return M;
                    case Day19_Category.A:
                        return A;
                    case Day19_Category.S:
                    default:
                        return S;
                }
            }

            public void SetRangeForCategory(Day19_Category category, Day19_Range range)
            {
                switch (category)
                {
                    case Day19_Category.X:
                        X = range;
                        break;
                    case Day19_Category.M:
                        M = range;
                        break;
                    case Day19_Category.A:
                        A = range;
                        break;
                    case Day19_Category.S:
                    default:
                        S = range;
                        break;
                }
            }
        }

        private List<Day19_Workflow> Day19_Workflows = new List<Day19_Workflow>();
        public string Solve_19_A()
        {
            var workflowInputs = GetInputForFileAsString(_filename).Split("\r\n\r\n").First().Split("\r\n").ToList();
            var partInputs = GetInputForFileAsString(_filename).Split("\r\n\r\n").Last().Split("\r\n").ToList();

            foreach (var workflowInput in workflowInputs)
            {
                Day19_Workflows.Add(new Day19_Workflow(workflowInput));
            }

            var parts = new List<Day19_Part>();
            foreach (var partInput in partInputs)
            {
                parts.Add(new Day19_Part(partInput));
            }

            var acceptedParts = new List<Day19_Part>();
            var totalScore = 0;
            Day19_Workflow firstWorkflow = Day19_Workflows.FirstOrDefault(w => w.Name == "in");
            foreach (var part in parts)
            {
                var result = Day19_GetDestinationForPart(part, firstWorkflow);
                if (result == "A")
                {
                    acceptedParts.Add(part);
                    totalScore += part.GetScore();
                }
            }
            _logger.LogInformation($"\n{acceptedParts.Count()} Accepted Parts:\n" + string.Join("\n", acceptedParts.Select(p => p.GetStringRepresentation())));
            Thread.Sleep(1000);

            return totalScore.ToString();
        }

        public string Day19_GetDestinationForPart(Day19_Part part, Day19_Workflow workflow)
        {
            foreach (var rule in workflow.Rules)
            {
                if ((rule.OperatorIsLessThan && part.GetScoreForCategory(rule.Category) < rule.Limit) ||
                    (!rule.OperatorIsLessThan && part.GetScoreForCategory(rule.Category) > rule.Limit))
                {
                    var destination = rule.NextWorkflow.Trim();
                    if (destination == "R" || destination == "A")
                    {
                        return destination;
                    }

                    Day19_Workflow nextWorkflow = Day19_Workflows.FirstOrDefault(w => w.Name == destination);
                    return Day19_GetDestinationForPart(part, nextWorkflow);
                }
            }

            if (workflow.DefaultWorkflow == "R" || workflow.DefaultWorkflow == "A")
            {
                return workflow.DefaultWorkflow;
            }

            Day19_Workflow defaultWorkflow = Day19_Workflows.FirstOrDefault(w => w.Name == workflow.DefaultWorkflow);
            return Day19_GetDestinationForPart(part, defaultWorkflow);
        }

        List<Day19_RangePart> Day19_PartB_AcceptedPartRanges = new List<Day19_RangePart>();
        public string Solve_19_B()
        {
            var workflowInputs = GetInputForFileAsString(_filename).Split("\r\n\r\n").First().Split("\r\n").ToList();
            foreach (var workflowInput in workflowInputs)
            {
                Day19_Workflows.Add(new Day19_Workflow(workflowInput));
            }

            var acceptedPartRanges = new List<Day19_RangePart>();
            var pendingPartRanges = new List<(Day19_RangePart Range, string NextWorkflowName)>()
            {
                (new Day19_RangePart(new Day19_Range(1,4000),new Day19_Range(1,4000),new Day19_Range(1,4000),new Day19_Range(1,4000)), "in")
            };

            while (pendingPartRanges.Any())
            {
                (var currentRange, var nextWorkflowName) = pendingPartRanges.First();
                pendingPartRanges.Remove((currentRange, nextWorkflowName));
                pendingPartRanges.AddRange(Day19_SplitPartRangeBasedOnWorkflow(currentRange, nextWorkflowName));
            }

            _logger.LogInformation($"\nResults:\n" + string.Join("\n", Day19_PartB_AcceptedPartRanges.OrderBy(a => a.X.Minimum)
                                                                                                    .ThenBy(a => a.X.Maximum)
                                                                                                    .ThenBy(a => a.M.Minimum)
                                                                                                    .ThenBy(a => a.M.Maximum)
                                                                                                    .ThenBy(a => a.A.Minimum)
                                                                                                    .ThenBy(a => a.A.Maximum)
                                                                                                    .ThenBy(a => a.S.Minimum)
                                                                                                    .ThenBy(a => a.S.Maximum)
                                                                                                    .Select(a => a.GetStringValue())));
            Thread.Sleep(1000);

            var parts = new List<Day19_Part>();
            foreach (var acceptedPart in Day19_PartB_AcceptedPartRanges.OrderBy(a => a.X.Minimum)
                                                                        .ThenBy(a => a.X.Maximum)
                                                                        .ThenBy(a => a.M.Minimum)
                                                                        .ThenBy(a => a.M.Maximum)
                                                                        .ThenBy(a => a.A.Minimum)
                                                                        .ThenBy(a => a.A.Maximum)
                                                                        .ThenBy(a => a.S.Minimum)
                                                                        .ThenBy(a => a.S.Maximum))
            {
                parts.Add(acceptedPart.GetRandomPart());
            }

            var acceptedParts = new List<Day19_Part>();
            Day19_Workflow firstWorkflow = Day19_Workflows.FirstOrDefault(w => w.Name == "in");
            foreach (var part in parts)
            {
                var result = Day19_GetDestinationForPart(part, firstWorkflow);
            }

            var totalScore = Day19_PartB_AcceptedPartRanges.Sum(a => a.GetNumberOfCominbations());
            return totalScore.ToString();
        }

        public List<(Day19_RangePart PartRange, string NextWorkflowName)> Day19_SplitPartRangeBasedOnWorkflow(Day19_RangePart incomingPartRange, string workflowName)
        {
            if (workflowName == "A")
            {
                Day19_PartB_AcceptedPartRanges.Add(incomingPartRange);
                return new();
            }
            if (workflowName == "R")
            {
                return new();
            }

            List<(Day19_RangePart PartRange, string NextWorkflowName)> results = new();
            var currentPartRange = incomingPartRange;
            Day19_Workflow workflow = Day19_Workflows.First(w => w.Name == workflowName);
            foreach (var rule in workflow.Rules)
            {
                var currentRange = currentPartRange.GetRangeForCategory(rule.Category);
                if (rule.OperatorIsLessThan && currentRange.Minimum < rule.Limit)
                {
                    var newRanges = currentRange.SplitUpToAndIncluding(rule.Limit - 1);
                    var newPartRange = currentPartRange.Clone();
                    newPartRange.SetRangeForCategory(rule.Category, newRanges.First());
                    results.Add((newPartRange, rule.NextWorkflow));
                    currentPartRange.SetRangeForCategory(rule.Category, newRanges.Last());
                }
                else if (!rule.OperatorIsLessThan && currentRange.Maximum > rule.Limit)
                {
                    var newRanges = currentRange.SplitUpToAndIncluding(rule.Limit);
                    var newPartRange = currentPartRange.Clone();
                    newPartRange.SetRangeForCategory(rule.Category, newRanges.Last());
                    results.Add((newPartRange, rule.NextWorkflow));
                    currentPartRange.SetRangeForCategory(rule.Category, newRanges.First());
                }
            }

            results.Add((currentPartRange, workflow.DefaultWorkflow));
            return results;
        }
    }
}
