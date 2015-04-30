using System.Collections;
using System.Collections.ObjectModel;

namespace CoAP_Analyzer_Client.Models
{
    public class WorkerListModel : BaseModel, IEnumerable
    {
        #region Members
        ObservableCollection<WorkerModel> _workers = new ObservableCollection<WorkerModel>();
        ObservableCollection<Resource> _resources = new ObservableCollection<Resource>();
        bool _enabled = true;
        #endregion

        #region Construction
        public WorkerListModel()
        {

        }
        #endregion

        #region Methods
        public void addWorker(WorkerModel _wm)
        {
            Workers.Add(_wm);
            RaisePropertyChanged("Workers");
        }
        public void removeWorker(WorkerModel _wm)
        {
            Workers.Remove(_wm);
            RaisePropertyChanged("Workers");
        }
        #endregion

        #region Properties
        public bool Enabled { get { return _enabled; } set { _enabled = value; RaisePropertyChanged("Enabled"); } }
        public ObservableCollection<WorkerModel> Workers
        {
            get
            {
                return _workers;
            }
            set
            {
                _workers = value;
                RaisePropertyChanged("Workers");
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
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return new ListEnum<WorkerModel>(_workers);
        }
        #endregion
    }
}
