using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExtensionMethods;

namespace NewAdventApp
{
    public class Execute
    {
        private readonly ILogger<Execute> _logger;
        private readonly ILogger<IHelpGatherInputs> _loggerForSolvers;

        public Execute(ILogger<Execute> logger, ILogger<IHelpGatherInputs> loggerForSolvers)
        {
            _logger = logger;
            _loggerForSolvers = loggerForSolvers;
        }

        [Function("Execute")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest request)
        {
            _logger.LogInformation($"Executing Advent of Code...");

            var year = request.GetStringForHeader("year") ?? "2023";
            var day = request.GetStringForHeader("day") ?? "1";
            var part = request.GetStringForHeader("part") ?? "A";
            var isTesting = request.GetBooleanForHeader("isTesting", true);
            _logger.LogInformation($"Year {year}, Day {day}" + (isTesting ? " (**TESTING MODE**)" : ""));

            var result = CallAppropriateFunction(year, day, part, isTesting);

            return new OkObjectResult(result);
        }

        private string CallAppropriateFunction(string year, string day, string part, bool isTesting)
        {
            var filename = $"Input_{year}_{day}_{part}" + (isTesting ? "_Test" : "") + ".txt";
            switch (year)
            {
                case "2023":
                    return CallAppropriateFunction_2023(year, day, part, isTesting, filename);
                default:
                    throw new Exception("Invalid year.");
            }
        }

        private string CallAppropriateFunction_2023(string year, string day, string part, bool isTesting, string filename)
        {
            var solver = new Solver_2023(_loggerForSolvers);
            solver.SetFilename(filename);

            string dayPart = day + "-" + part;
            switch (dayPart)
            {
                case "1-A":
                    return solver.Solve_01_A();
                case "1-B":
                    return solver.Solve_01_B();
                case "2-A":
                    return solver.Solve_02_A();
                case "2-B":
                    return solver.Solve_02_B();
                case "3-A":
                    return solver.Solve_03_A();
                case "3-B":
                    return solver.Solve_03_B();
                case "4-A":
                    return solver.Solve_04_A();
                case "4-B":
                    return solver.Solve_04_B();
                case "5-A":
                    return solver.Solve_05_A();
                case "5-B":
                    return solver.Solve_05_B();
                case "6-A":
                    return solver.Solve_06_A();
                case "6-B":
                    return solver.Solve_06_B();
                case "7-A":
                    return solver.Solve_07_A();
                case "7-B":
                    return solver.Solve_07_B();
                case "8-A":
                    return solver.Solve_08_A();
                case "8-B":
                    return solver.Solve_08_B();
                case "9-A":
                    return solver.Solve_09_A();
                case "9-B":
                    return solver.Solve_09_B();
                case "10-A":
                    return solver.Solve_10_A();
                case "10-B":
                    return solver.Solve_10_B();
                case "11-A":
                    return solver.Solve_11_A();
                case "11-B":
                    return solver.Solve_11_B();
                case "12-A":
                    return solver.Solve_12_A();
                case "12-B":
                    return solver.Solve_12_B();
                default:
                    throw new Exception("Invalid day/part.");
            }
        }
    }
}
