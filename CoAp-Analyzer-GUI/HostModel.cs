using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CoAP_Analyzer_Client;

namespace CoAp_Analyzer_GUI
{
    public class HostViewModel : INotifyPropertyChanged
    {
        #region members
        Host _host;
        int _rate;
        #endregion

        #region Properties
        public Host Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
            }
        }

        public IPAddress IP
        {
            get
            {
                return _host.IP;
            }
            set
            {
                _host.IP = value;
                RaisePropertyChanged("IP");
            }
        }
        public int Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
                RaisePropertyChanged("Rate");
            }
        }
        #endregion

        #region Construction
        public HostViewModel()
        {
            _host = new Host { IP = null};
        }

        public HostViewModel(IPAddress _ip)
        {
            _host = new Host { IP = _ip };
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        private void RaisePropertyChanged(string propertyName)
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

    public class HostListModel
    {
        #region Members
        ObservableCollection<HostViewModel> _hosts;
        #endregion

        #region Construction
        public HostListModel()
        {
            _hosts = new ObservableCollection<HostViewModel>();
        }
        #endregion

        #region Properties
        public ObservableCollection<HostViewModel> Hosts
        {
            get
            {
                return _hosts;
            }
            set
            {
                _hosts = value;
            }
        }
        #endregion
    }
}
