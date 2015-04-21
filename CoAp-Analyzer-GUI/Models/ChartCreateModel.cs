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
    public class ChartCreateModel : BaseModel
    {

        ObservableCollection<WorkerModel> _workers;
        ObservableCollection<ChartModel> _charts;
        ChartModel _current;

        public ChartCreateModel(){
            Navigation = new ObservableCollection<BaseModel>();
            Charts = new ObservableCollection<ChartModel>();
            Chart = new ChartModel();
            Chart.Name = "New Chart";
            Chart.Command = new RelayCommand(param => SharedData._command(Chart), param => true);
            Workers = SharedData._workerList.Workers;
        }

        public ChartModel Chart
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
                RaisePropertyChanged("Chart");
            }
        }

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

        public ObservableCollection<ChartModel> Charts
        {
            get
            {
                return _charts;
            }
            set
            {
                _charts = value;
                RaisePropertyChanged("Charts");
            }
        }
    }
}
