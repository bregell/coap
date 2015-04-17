using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CoAP_Analyzer_Client.Models;

namespace CoAP_Analyzer_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for WorkerList.xaml
    /// </summary>
    public partial class WorkerList : UserControl
    {
        public WorkerList()
        {
            InitializeComponent();
            this.DataContext = SharedData._workerList;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WorkerModel w = (WorkerModel)(workerlist.SelectedItem);
            SharedData._measureList.Measures.CollectionChanged -= Measures_CollectionChanged;
            SharedData._cm.ClearModel();
            try
            {
                SharedData._measureList.Measures = w.Worker.Measures;
                SharedData._cm.UpdateModel(SharedData._cm.LineSeries[0], SharedData._measureList.Measures);
                SharedData._measureList.Measures.CollectionChanged += Measures_CollectionChanged;
            }
            catch (Exception)
            {
                
            }  
        }

        void Measures_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ObservableCollection<MeasureModel> _mm = new ObservableCollection<MeasureModel>();
                foreach(object _m in e.NewItems){
                    _mm.Add((MeasureModel)_m);  
                }
                SharedData._cm.UpdateModel(SharedData._cm.LineSeries[0] ,_mm);
                SharedData._cm.Model.InvalidatePlot(true);
            });
        }
    }
}
