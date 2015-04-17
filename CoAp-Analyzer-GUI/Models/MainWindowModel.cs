using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoAP_Analyzer_Client.Models;
using System.Collections.ObjectModel;

namespace CoAP_Analyzer_GUI.Models
{
    class MainWindowModel : BaseModel
    {
        #region Members
        ObservableCollection<BaseModel> _navigation;
        #endregion

        public MainWindowModel()
        {
            Navigation = new ObservableCollection<BaseModel>();
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
    }
}
