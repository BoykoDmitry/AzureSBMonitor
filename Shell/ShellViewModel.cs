using AzureSBMonitor.Services;
using AzureSBMonitor.ViewModels;
using AzureSBMonitor.ViewModels.Base;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.Shell
{
    public class ShellViewModel : BaseScreen
    {
        public ShellViewModel(Microsoft.Extensions.Logging.ILogger<ShellViewModel> logger, 
            IDialogService dialogService,
            FilterViewModel filterViewModel,
            MainGridViewModel mainGridViewModel
            ) : base(logger, dialogService)
        {
            _filterViewModel = filterViewModel;
            _mainGridViewModel = mainGridViewModel;

            _filterViewModel.Subject.Subscribe(_ =>
            {
                MainGridViewModel.Entities = _;
            });
        }

        private readonly FilterViewModel _filterViewModel;
        public FilterViewModel FilterViewModel => _filterViewModel;

        private readonly MainGridViewModel _mainGridViewModel;
        public MainGridViewModel MainGridViewModel => _mainGridViewModel;
    }
}
