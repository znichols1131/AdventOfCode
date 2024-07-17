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
            return input;
        }
    }
}
