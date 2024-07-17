using System;
using System.IO;

namespace NewAdventApp
{
    public abstract class IHelpGatherInputs
    {
        public string GetInputForFileAsString(string filename)
        {
            string filePath = GetRootDirectory() + filename;

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            throw new Exception($"Input file could not be found at {filePath}");
        }

        public StreamReader GetInputForFileAsStreamReader(string filename)
        {
            string filePath = GetRootDirectory() + filename;

            if (File.Exists(filePath))
            {
                return new StreamReader(filePath);
            }

            throw new Exception($"Input file could not be found at {filePath}");
        }

        private string GetRootDirectory()
        {
            var root = Environment.GetEnvironmentVariable("InputRootDirectory");
            if (string.IsNullOrWhiteSpace(root))
            {
                throw new Exception("Input file directory not found.");
            }
            return root;
        }
    }
}
