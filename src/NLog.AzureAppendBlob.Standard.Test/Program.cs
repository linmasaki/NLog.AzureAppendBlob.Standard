using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;


namespace NLog.AzureAppendBlob.Standard.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesProvider = BuildDi();
            var runner = servicesProvider.GetRequiredService<Runner>();
            runner.DoAction("Run Action");

            #if NETCOREAPP1_0 || NETCOREAPP1_1
                Console.WriteLine("* Net Core App 1.x * Press ANY key to exit");
            #else
                Console.WriteLine("* Net Core App 2.x * Press ANY key to exit");
            #endif

            Console.WriteLine("Press ANY key to exit");
            Console.ReadKey();
        }

        private static IServiceProvider BuildDi()
        {
            var services = new ServiceCollection();

            //Runner is the custom class
            services.AddTransient<Runner>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            
            #if NETCOREAPP1_0 || NETCOREAPP1_1
                services.AddLogging();
            #else
                services.AddLogging(builder => builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace));
            #endif

            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            
            //configure NLog
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            LogManager.LoadConfiguration("nlog.config");

            return serviceProvider;
        }

        public class Runner
        {
            private readonly ILogger<Runner> _logger;

            public Runner(ILogger<Runner> logger)
            {
                this._logger = logger;
            }

            public void DoAction(string name)
            {
                #if NETCOREAPP1_0 || NETCOREAPP1_1
                    _logger.LogInformation("/*** NETCOREAPP1_x ***/");
                #else
                    _logger.LogInformation("/*** NETCOREAPP2_x ***/");
                #endif

                _logger.LogTrace(name + " Trace!");
                _logger.LogDebug(name + " Debug!");
                _logger.LogInformation(name + " Information!");
                _logger.LogWarning(name + " Warning!");
                _logger.LogError(name + " Error!");
                _logger.LogCritical(name + " Critical!");

                try
                {
                    throw new NotSupportedException();
                }
                catch (Exception ex)
                {
                    _logger.LogError("This is an expected exception.", ex);
                }
            }
        }

    }
}
