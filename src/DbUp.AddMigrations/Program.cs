using System;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DbUp.AddMigrations
{
    class Program
    {
        static int Main(string[] args)
        {
            var parser = new Parser(settings =>
            {
                settings.CaseInsensitiveEnumValues = true;
                settings.HelpWriter = Console.Error;
            });
            var parserResult = parser.ParseArguments<FileProviderOptions>(args);
            
            return 
                parserResult
                .MapResult(
                    ConfigureAndRun,
                    errs => DisplayHelp(parserResult));
        }

        private static int DisplayHelp<T>(ParserResult<T> parserResult)
        {
            var helpText = HelpText.AutoBuild(parserResult, h =>
            {
                h.AddEnumValuesToHelpText = true;
                return HelpText.DefaultParsingErrorsHandler(parserResult, h);
            }, e => e);
            Console.WriteLine(helpText);
            return 1;
        }
        private static void ConfigureServices(IServiceCollection services, BaseOptions options)
        {
            var loglevel = (Microsoft.Extensions.Logging.LogLevel)options.LogLevel;
            if (loglevel == Microsoft.Extensions.Logging.LogLevel.None)
            {
                loglevel = Microsoft.Extensions.Logging.LogLevel.Error;
            }
            
            services.AddLogging(configure => configure.SetMinimumLevel(loglevel).AddConsole())
                .AddTransient<IProviderMigrations<FileProviderOptions>, FileProviderMigrations>();
        }

        private static int ConfigureAndRun<TOptions>(TOptions options) where TOptions : BaseOptions
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection, options);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider.GetService<IProviderMigrations<TOptions>>().Add(options);
        }
    }
}