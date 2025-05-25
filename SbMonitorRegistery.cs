using AzureSBMonitor.Shell;
using AzureSBMonitor.ViewModels.Base;
using Lamar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor
{
    public class SbMonitorRegistery : ServiceRegistry
    {
        public SbMonitorRegistery()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.Exclude(_ => _ == typeof(ShellViewModel));
                x.AddAllTypesOf<BaseScreen>().NameBy(n => n.Name.Replace("ViewModel", ""));

            });
        }
    }
}
