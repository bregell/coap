using CoAP_Analyzer_Client;
using CoAP_Analyzer_Client.Models;
using System.Collections.ObjectModel;

namespace CoAP_Analyzer_GUI.Models
{
    public class HostCreationModel : BaseModel
    {
        HostListModel _hlm = new HostListModel();
        HostModel _host;
        WorkerListModel _wlm = new WorkerListModel();
        ObservableCollection<Resource> _resources = new ObservableCollection<Resource>();

        public HostCreationModel()
        {
            Hosts = SharedData._hostList;
        }
        public HostModel Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
                RaisePropertyChanged("Host");
            }
        }

        public ObservableCollection<Resource> Resources
        {
            get
            {
                return _resources;
            }
            set
            {
                _resources = value;
                RaisePropertyChanged("Resources");
            }
        }

        public HostListModel Hosts
        {
            get
            {
                return _hlm;
            }
            set
            {
                _hlm = value;
                RaisePropertyChanged("Hosts");
            }
        }

        public WorkerListModel Workers
        {
            get
            {
                return _wlm;
            }
            set
            {
                _wlm = value;
                _wlm.Resources = Resources;
                RaisePropertyChanged("Workers");
            }

        }
    }
}
