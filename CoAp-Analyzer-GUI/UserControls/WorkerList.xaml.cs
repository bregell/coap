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
using System.Collections.Specialized;
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
            try
            {
                SharedData._measureList.Measures = w.Worker.Measures;

            }
            catch (Exception)
            {
                
            }  
        }
    }
}
