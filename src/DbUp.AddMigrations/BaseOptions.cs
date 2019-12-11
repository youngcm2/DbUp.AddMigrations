using CommandLine;

namespace DbUp.AddMigrations
{
    internal class BaseOptions
    {
        [Option('v', "verbosity", Required = false, HelpText = "Name for the script", Default = LogLevel.Error)]
        public LogLevel LogLevel { get; set; }
    }
}