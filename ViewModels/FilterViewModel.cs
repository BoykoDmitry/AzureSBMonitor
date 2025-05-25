using AzureSBMonitor.Models;
using AzureSBMonitor.Services;
using AzureSBMonitor.ViewModels.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.ViewModels
{
    public class FilterViewModel : BaseScreen
    {
        private readonly IStatsManager _statsManager;
        public FilterViewModel(ILogger<FilterViewModel> logger, IDialogService dialogService, IStatsManager statsManager) : base(logger, dialogService)
        {

            _subject = new Subject<IEnumerable<StatEntity>>();
            _statsManager = statsManager;   
        }

        private readonly ISubject<IEnumerable<StatEntity>> _subject;
        public ISubject<IEnumerable<StatEntity>> Subject => _subject;

        private string _connectionString;
        public string ConnectionString {
            get => _connectionString;
            set
            {
                _connectionString = value;
                NotifyOfPropertyChange();

                CanSearch = !string.IsNullOrEmpty(_connectionString);
            }
        }

        private bool _canSearch = false;
        public bool CanSearch
        {
            get => _canSearch;
            set
            {
                _canSearch = value;
                NotifyOfPropertyChange();
            }
        }

        public void CancelSearch()
        {
            CancellationTokenSource.Cancel();
        }

        private CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();

        public async Task Search()
        {
            if (string.IsNullOrEmpty(_connectionString)) return;

            if (HasErrors) return;

            try
            {
                CancellationTokenSource = new CancellationTokenSource();

                SearchVisible = false;
                IsBusy = true;

                var stats = await _statsManager.GetStats(ConnectionString, CancellationTokenSource.Token);

                _subject.OnNext(stats);
            }
            catch(OperationCanceledException)
            {
                Logger.LogInformation("search was canceled");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex,ex.Message);

                await DialogService.ShowError(ex);
            }
            finally
            {
                IsBusy = false;
                SearchVisible = true;
            }
        }

        private bool _searchVisible = true;
        public bool SearchVisible
        {
            get => _searchVisible;
            set
            {
                _searchVisible = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
