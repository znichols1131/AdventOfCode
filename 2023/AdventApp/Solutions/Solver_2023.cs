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
                    var differences = GetDifferences(previousRow);
                    numbers.Add(differences);
                    previousRow = differences;
                    doneFindingDifferences = differences.All(d => d == 0.0);
                }

                var previousDifference = 0.0;
                for (int i = numbers.Count - 2; i >= 0; i--)
                {
                    numbers[i] = ExtrapolateRowForward(numbers[i], previousDifference);
                    previousDifference = numbers[i].Last();
                }

                _logger.LogInformation(string.Join("\n", numbers.Select(n => string.Join(" ", n))));

                sum += previousDifference;
            }

            return sum.ToString();
        }

        public List<double> GetDifferences(List<double> input)
        {
            var output = new List<double>();
            for (int i = 1; i < input.Count; i++)
            {
                output.Add(input[i] - input[i - 1]);
            }
            return output;
        }

        public List<double> ExtrapolateRowForward(List<double> row, double nextDifference)
        {
            row.Add(row.Last() + nextDifference);
            return row;
        }

        public List<double> ExtrapolateRowBackwards(List<double> row, double nextDifference)
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
                    var differences = GetDifferences(previousRow);
                    numbers.Add(differences);
                    previousRow = differences;
                    doneFindingDifferences = differences.All(d => d == 0.0);
                }

                var previousDifference = 0.0;
                for (int i = numbers.Count - 2; i >= 0; i--)
                {
                    numbers[i] = ExtrapolateRowBackwards(numbers[i], previousDifference);
                    previousDifference = numbers[i].First();
                }

                _logger.LogInformation(string.Join("\n", numbers.Select(n => string.Join(" ", n))));

                sum += previousDifference;
            }

            return sum.ToString();
        }
    }
}
