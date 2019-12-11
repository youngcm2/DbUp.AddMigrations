using CommandLine;

namespace DbUp.AddMigrations
{
    [Verb("file")]
    internal class FileProviderOptions : BaseOptions
    {
        [Option('s', "scripts-directory", HelpText = "Scripts directory used by dbup", Default = "scripts")]
        public string ScriptsFolder { get; set; } 

        [Option('n', "name", Required = true, HelpText = "Name for the script")]
        public string Name { get; set; }
    }
}