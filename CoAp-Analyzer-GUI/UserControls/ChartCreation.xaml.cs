using CoAP_Analyzer_Client.Models;
using CoAP_Analyzer_GUI.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoAP_Analyzer_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for ChartCreation.xaml
    /// </summary>
    public partial class ChartCreation : UserControl
    {
        public ChartCreation()
        {  
            InitializeComponent();
            SharedData._chartTab.CreateModel.PropertyChanged += _clm_PropertyChanged;
        }

        void _clm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Chart")
            {
                selectItems();
            }
        }

        private void selectItems()
        {
            foreach(WorkerModel _wm in SharedData._chartTab.CreateModel.Chart.Workers){
                workerlist.SelectedItems.Add(_wm);
            }
        }

        private void workerlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Add the newly selected items
            //ObservableCollection<WorkerModel> _wm = ((ObservableCollection<WorkerModel>)sender);
            //if (!SharedData._chartTab.CreateModel.Chart.Series.Any(x => x.Item3.Equals(_wm.Measure))){
            foreach (object _obj in e.AddedItems)
            {
                WorkerModel _wm = ((WorkerModel)_obj);
                if (!SharedData._chartTab.CreateModel.Chart.Series.Any(x => x.Item3.Equals(_wm.Measure)))
                {
                    SharedData._chartTab.CreateModel.Chart.AddSeries(_wm);
                }  
            }
            //Remove the unselected items
            foreach (object _obj in e.RemovedItems)
            {
                WorkerModel _wm = ((WorkerModel)_obj);
                if (SharedData._chartTab.CreateModel.Chart.Series.Any(x => x.Item3.Equals(_wm.Measure)))
                {
                    SharedData._chartTab.CreateModel.Chart.RemoveSeries(_wm);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SharedData._chartTab.CreateModel.Chart.Name = SharedData._chartTab.CreateModel.Chart.Model.Title;
            if (!SharedData._chartTab.CreateModel.Charts.Contains(SharedData._chartTab.CreateModel.Chart))
            {
                SharedData._chartTab.CreateModel.Charts.Add(SharedData._chartTab.CreateModel.Chart);
                SharedData._chartTab.CreateModel.addNavigation(SharedData._chartTab.CreateModel.Chart);
            }
            SharedData._chartTab.CreateModel.Chart.Controls = Visibility.Visible;
            SharedData._chartTab.CreateModel.Chart = new ChartModel();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            SharedData._chartTab.CreateModel.Chart.Controls = System.Windows.Visibility.Visible;
            SharedData._chartTab.CreateModel.Chart = new ChartModel();
        }
    }
}
