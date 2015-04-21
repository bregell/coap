using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoAP_Analyzer_Client.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using CoAP_Analyzer_GUI.UserControls;
using System.Windows.Input;

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
