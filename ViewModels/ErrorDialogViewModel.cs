using AzureSBMonitor.Services;
using AzureSBMonitor.ViewModels.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.ViewModels
{
    public class ErrorDialogViewModel : BaseScreen
    {
        public ErrorDialogViewModel(ILogger<ErrorDialogViewModel> log, IDialogService dialogService) : base(log, dialogService)
        {
        }

        public void SetError(Exception ex)
        {
            var e = ex;

            if (ex is AggregateException aggregateException)
            {
                e = aggregateException.Flatten().InnerException;
            }

            Message = e?.Message;
            Stack = e?.StackTrace;
        }

        private string _stack = null;
        public string Stack
        {
            get { return _stack; }
            set { Set(ref _stack, value); }
        }

        private string _message = null;
        public string Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
        }
    }
}
