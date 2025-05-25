using AzureSBMonitor.Models;
using AzureSBMonitor.Services;
using AzureSBMonitor.ViewModels.Base;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.ViewModels
{
    public class MainGridViewModel : BaseScreen
    {
        public MainGridViewModel(ILogger<MainGridViewModel> logger, IDialogService dialogService) : base(logger, dialogService)
        {
        }

        private IEnumerable<StatEntity> _entities = new List<StatEntity>();
        public IEnumerable<StatEntity> Entities 
        { 
            get 
            {
                return _entities; 
            } 

            set
            {
                _entities = value;
                Filtered = value;
                Filter = string.Empty;

                NotifyOfPropertyChange(); 
            }
        }

        private IEnumerable<StatEntity> _filtered = new List<StatEntity>();
        public IEnumerable<StatEntity> Filtered
        {
            get
            {
                return _filtered;
            }

            set
            {
                _filtered = value;
                NotifyOfPropertyChange();
            }
        }

        private string _filter = string.Empty;
        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                NotifyOfPropertyChange();                
            }
        }

        public void DoFilter()
        {
            FilterEntities();
        }

        private void FilterEntities()
        {
            Filtered = Entities;

            if (!string.IsNullOrEmpty(Filter))
                Filtered = Entities.Where(_ => _.Title.ToLower().Contains(Filter.ToLower()));
        }
    }
}
