using AzureSBMonitor.Shell;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace AzureSBMonitor.Services
{
    public class MainWindowHandleProvider : IMainWindowHandleProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public MainWindowHandleProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IntPtr GetHandle()
        {
            var shell = _serviceProvider.GetService<ShellViewModel>();
            var view = shell.GetView() as DependencyObject;
            var win = Window.GetWindow(view);

            return new WindowInteropHelper(win).Handle;
        }
    }
}
