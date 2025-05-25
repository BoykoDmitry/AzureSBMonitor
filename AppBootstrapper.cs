using AzureSBMonitor.Shell;
using Caliburn.Micro;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using Lamar;
using Microsoft.Extensions.DependencyInjection;
using AzureSBMonitor.Services;

namespace AzureSBMonitor
{
    public class AppBootstrapper : BootstrapperBase
    {
        static Serilog.ILogger Logger => new LoggerConfiguration().WriteTo.File("appstart.log", Serilog.Events.LogEventLevel.Error).CreateLogger();

        IContainer serviceProvider;
        IConfigurationRoot _configuration;
        public AppBootstrapper()
        {
            BuildConfiguration();
            Initialize();

            Coroutine.Completed += Coroutine_Completed;
        }

        private void BuildConfiguration()
        {
            try
            {
                var cnf = new ConfigurationBuilder();
                cnf.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                _configuration = cnf.Build();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);

                throw;
            }
        }

        private async void Coroutine_Completed(object sender, ResultCompletionEventArgs e)
        {
            if (e.Error != null)
            {
                var dlg = serviceProvider.GetService<IDialogService>();
                await dlg.ShowError(e.Error);
            }
        }

        protected override async void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var dlg = serviceProvider.GetService<IDialogService>();
            await dlg.ShowError(e.Exception);

            base.OnUnhandledException(sender, e);
        }

        protected override void Configure()
        {
            try
            {
                serviceProvider = new Container(_ =>
                {
                    _.IncludeRegistry<SbMonitorRegistery>();

                    _.AddSingleton<IConfiguration>(_configuration);

                    _.AddLogging(_configuration);
                    _.AddAppServices();
                    _.AddCaliburn();

                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);

                throw;
            }

            //LogManager.GetLog = t => new SerilogLog(serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(AppBootstrapper)));
        }

        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrEmpty(key))
                return serviceProvider.GetInstance(service);
            else
                return serviceProvider.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return serviceProvider.GetAllInstances(service).OfType<object>();
        }

        protected override void BuildUp(object instance)
        {
            (serviceProvider as Container).BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {

            dynamic set = new ExpandoObject();
            set.ResizeMode = ResizeMode.CanResize;
            set.SizeToContent = SizeToContent.Manual;
            set.Title = "Azure service bus queue and topics";

            DisplayRootViewForAsync<ShellViewModel>(set);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { Assembly.GetExecutingAssembly() };
        }
    }
}

