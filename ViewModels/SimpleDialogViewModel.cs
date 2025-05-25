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
    public class SimpleDialogViewModel : BaseScreen
    {
        public SimpleDialogViewModel(ILogger<SimpleDialogViewModel> log, IDialogService dialogService) : base(log, dialogService)
        {
        }

        private BaseScreen _viewModel;
        public BaseScreen ViewModel
        {
            get { return _viewModel; }
            set { Set(ref _viewModel, value); }
        }

        private string _message = null;
        public string Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
        }
    }
}
