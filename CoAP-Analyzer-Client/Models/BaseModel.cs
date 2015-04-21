using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace CoAP_Analyzer_Client.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        #region Members
        string _name;
        ICommand _command;
        ObservableCollection<BaseModel> _navigation;
        #endregion

        #region Properties
        public string Name 
        { 
            get 
            { 
                return _name; 
            } 
            set 
            { 
                _name = value; 
                RaisePropertyChanged("Name"); 
            } 
        }

        public ICommand Command
        {
            get 
            {
                return _command;
            }
            set 
            { 
                _command = value; 
                RaisePropertyChanged("Command");
            }
        }

        public BaseModel Self 
        { 
            get 
            { 
                return this;
            } 
        }

        public ObservableCollection<BaseModel> Navigation
        {
            get
            {
                return _navigation;
            }
            set 
            { 
                _navigation = value;
                RaisePropertyChanged("Navigation"); 
            }
        }

        public void addNavigation(BaseModel _mb)
        {
            Navigation.Add(_mb);
            RaisePropertyChanged("Navigation");
        } 
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

    public class RelayCommand : ICommand
    {
        #region Fields
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion

        #region Constructors
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region Members
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion
    }

    public class ListEnum<T> : IEnumerator
    {
        ObservableCollection<T> _collection;
        int position = -1;

        public ListEnum(ObservableCollection<T> _m)
        {
            _collection = _m;
        }

        object IEnumerator.Current
        {
            get { return _collection[position]; }
        }

        bool IEnumerator.MoveNext()
        {
            position++;
            return (position < _collection.Count);
        }

        void IEnumerator.Reset()
        {
            position = -1;
        }
    }
}
