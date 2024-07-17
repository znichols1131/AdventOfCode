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
                default:
                    throw new Exception("Invalid day/part.");
            }
        }
    }
}
