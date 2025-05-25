using AzureSBMonitor.Services;
using Caliburn.Micro;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.ViewModels.Base
{
    public class BaseScreen : Screen, INotifyDataErrorInfo
    {
        private readonly PropertyInfo[] _props;

        private IList<(string propName, Func<string> validatorFunc)> _validators;

        private readonly IDialogService _dialogService;
        private readonly ILogger _logger;
        protected BaseScreen(ILogger logger, IDialogService dialogService)
        {
            _validators = new List<(string, Func<string>)>();
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            PropertyChanged += BaseScreen_PropertyChanged;
        }

        private void BaseScreen_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnErrorsChanged(this, new DataErrorsChangedEventArgs(e.PropertyName));
        }

        protected ILogger Logger
        {
            get => _logger;
        }

        protected IDialogService DialogService
        {
            get => _dialogService;
        }

        private bool _isBusy = false;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }

        protected string getPropName<TProperty>(Expression<Func<TProperty>> property)
        {
            return property.GetMemberInfo().Name;
        }

        protected void AddValidator<TProperty>(Expression<Func<TProperty>> property, Func<string> func)
        {
            MemberExpression member = property.Body as MemberExpression;
            PropertyInfo propInfo = member.Member as PropertyInfo;

            if (propInfo == null)
                throw new ArgumentNullException($"{property} is not property!");

            AddValidator(propInfo.Name, func);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        protected void AddValidator(string propertyName, Func<string> func)
        {
            _validators.Add((propertyName, func));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            var errors = new List<string>();

            var validators = string.IsNullOrEmpty(propertyName) ? _validators.ToList() : _validators.Where(_ => _.propName == propertyName).ToList();

            foreach (var v in validators)
            {
                var e = v.validatorFunc.Invoke();
                if (!string.IsNullOrEmpty(e))
                    errors.Add(e);
            }

            return errors;
        }

        public bool HasErrors => GetErrors(null).OfType<object>().Any();

        protected virtual void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }

}
