using CoAP_Analyzer_Client.Models;
using System.Collections.ObjectModel;

namespace CoAP_Analyzer_GUI.Models
{
    public class MainWindowModel : BaseModel
    {
        public MainWindowModel()
        {
            Navigation = new ObservableCollection<BaseModel>();
        }
    }
}
