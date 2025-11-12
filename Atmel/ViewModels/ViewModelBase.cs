using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;

namespace Atmel.ViewModels
{
    /// <summary>
    /// Base ViewModel implementing INotifyPropertyChanged
    /// Following MVVM pattern and Prism principles
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Execute action on UI thread
        /// </summary>
        protected async Task RunOnUiThreadAsync(Action action)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () => action());
        }

        /// <summary>
        /// Called when view is navigated to
        /// </summary>
        public virtual void OnNavigatedTo(object parameter)
        {
        }

        /// <summary>
        /// Called when view is navigated from
        /// </summary>
        public virtual void OnNavigatedFrom()
        {
        }
    }
}
