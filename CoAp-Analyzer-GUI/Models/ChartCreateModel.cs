using CoAP_Analyzer_Client.Models;
using CoAP_Analyzer_GUI.UserControls;
using System.Collections.ObjectModel;

namespace CoAP_Analyzer_GUI.Models
{
    public class ChartCreateModel : BaseModel
    {

        ObservableCollection<ChartModel> _charts;
        ChartModel _current = new ChartModel();

        public ChartCreateModel(){
            Navigation = new ObservableCollection<BaseModel>();
            Charts = new ObservableCollection<ChartModel>();
            Chart.Name = "New Chart";
            Chart.Command = new RelayCommand(param => SharedData._command(Chart), param => true);
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
                return SharedData._workerList.Workers;
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
