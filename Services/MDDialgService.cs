using AzureSBMonitor.ViewModels.Base;
using AzureSBMonitor.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AzureSBMonitor.Services
{
    public class MDDialgService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;
        public MDDialgService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        private (T, object) GetAndWireUpDialog<T>()
        {
            var viewModel = _serviceProvider.GetService<T>();
            var view = Caliburn.Micro.ViewLocator.LocateForModel(viewModel, null, null);
            Caliburn.Micro.ViewModelBinder.Bind(viewModel, view, null);

            return (viewModel, view);
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.IsCancelled)
                return;

            var res = (bool)eventArgs.Parameter;
            if (res)
            {
                if (eventArgs.Session.Content is FrameworkElement view)
                {
                    if (view.DataContext is BaseScreen viewModel)
                    {
                        //viewModel.ValidateOnDemand();

                        if (viewModel.HasErrors)
                            eventArgs.Cancel();
                    }
                }
            }
        }

        public Task ShowError(Exception ex)
        {
            var dlg = GetAndWireUpDialog<ErrorDialogViewModel>();
            dlg.Item1.SetError(ex);

            return DialogHost.Show(dlg.Item2, "RootDialog", ClosingEventHandler);
        }

        public Task ShowError(string message)
        {
            var dlg = GetAndWireUpDialog<ErrorDialogViewModel>();
            dlg.Item1.Message = message;

            return DialogHost.Show(dlg.Item2, "RootDialog", ClosingEventHandler);
        }

        public async Task<bool> ShowYesNo(string question)
        {
            var dlg = GetAndWireUpDialog<SimpleDialogViewModel>();
            dlg.Item1.Message = question;

            var res = await DialogHost.Show(dlg.Item2, "RootDialog", ClosingEventHandler);

            return (bool)res;
        }

        public async Task<bool> ShowViewModel(BaseScreen viewModel)
        {
            var dlg = GetAndWireUpDialog<SimpleDialogViewModel>();

            dlg.Item1.ViewModel = viewModel;

            var res = await DialogHost.Show(dlg.Item2, "RootDialog", ClosingEventHandler);

            return (bool)res;
        }


        public async Task<bool> ShowViewModelGeneric<T>(T viewModel)
        {
            var dlg = GetAndWireUpDialog<T>();

            var res = await DialogHost.Show(dlg.Item2, "RootDialog", ClosingEventHandler);

            return (bool)res;
        }
    }
}
