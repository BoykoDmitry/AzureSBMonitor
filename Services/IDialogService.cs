using AzureSBMonitor.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.Services
{
    public interface IDialogService
    {
        Task<bool> ShowYesNo(string question);

        Task ShowError(string message);
        Task ShowError(Exception ex);
        Task<bool> ShowViewModel(BaseScreen viewModel);
        Task<bool> ShowViewModelGeneric<T>(T viewModel);
    }
}
