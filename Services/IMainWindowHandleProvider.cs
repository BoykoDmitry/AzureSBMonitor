using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.Services
{
    public interface IMainWindowHandleProvider
    {
        IntPtr GetHandle();
    }
}
