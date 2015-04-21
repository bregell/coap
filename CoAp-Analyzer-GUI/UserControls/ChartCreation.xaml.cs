using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CoAP_Analyzer_GUI.Models;
using CoAP_Analyzer_Client.Models;

namespace CoAP_Analyzer_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for ChartCreation.xaml
    /// </summary>
    public partial class ChartCreation : UserControl
    {
        static ChartCreateModel _clm;
        static ChartModel _cm;

        public ChartCreation()
        {  
            InitializeComponent();
            _clm = SharedData._chartTab.CreateModel;
            _cm = _clm.Chart;
        }

        private void workerlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Add the newly selected items
            //ObservableCollection<WorkerModel> _wm = ((ObservableCollection<WorkerModel>)sender);
            //if (!_cm.Series.Any(x => x.Item3.Equals(_wm.Measure))){
            foreach (object _obj in e.AddedItems)
            {
                WorkerModel _wm = ((WorkerModel)_obj);
                if (!_cm.Series.Any(x => x.Item3.Equals(_wm.Measure)))
                {
                    _clm.Chart.AddSeries(_wm);
                }  
            }
            //Remove the unselected items
            foreach (object _obj in e.RemovedItems)
            {
                WorkerModel _wm = ((WorkerModel)_obj);
                if (_cm.Series.Any(x => x.Item3.Equals(_wm.Measure)))
                {
                    _clm.Chart.RemoveSeries(_wm);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _clm.Chart.Name = _clm.Chart.Model.Title;
            _clm.Charts.Add(_clm.Chart);
            _clm.addNavigation(_clm.Chart);
            _clm.Chart.Controls = Visibility.Visible;
            _clm.Chart = new ChartModel();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _clm.Chart = new ChartModel();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            _clm.Chart.RefreshPlot();
        }
    }
}
