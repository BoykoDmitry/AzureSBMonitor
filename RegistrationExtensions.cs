using AzureSBMonitor.Services;
using AzureSBMonitor.Shell;
using Caliburn.Micro;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor
{
    public static class RegistrationExtensions
    {
        public static void AddCaliburn(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IWindowManager, WindowManager>();
            serviceCollection.AddSingleton<ShellViewModel>();
        }

        public static void AddLogging(this IServiceCollection serviceCollection, IConfiguration conf)
        {
            serviceCollection.AddLogging(_ => _.AddSerilog(new LoggerConfiguration()
                                                    .ReadFrom
                                                    .Configuration(conf)
                                                    .CreateLogger()));
        }

        public static void AddAppServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IStatsManager, StatsManager>();
            serviceCollection.AddSingleton<IDialogService, MDDialgService>();
            serviceCollection.AddSingleton<IMainWindowHandleProvider, MainWindowHandleProvider>();
        }
    }
}
