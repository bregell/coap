using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Net;
using CoAP_Analyzer_Client;
using CoAP_Analyzer_Client.Models;
using CoAP.Log;

namespace CoAP_Analyzer_GUI
{

    public static class SharedData
    {
        public static HostListModel _hostList = new HostListModel();
        public static WorkerListModel _workerList = new WorkerListModel();
        public static MeasureListModel _measureList = new MeasureListModel();
        public static List<Worker> _removedWorkers = new List<Worker>();
        public static List<Thread> _threads = new List<Thread>();
        public static bool _running = false;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {

        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StartBtn.Content.ToString() == "Start")
            {
                foreach (WorkerModel w in SharedData._workerList.Workers)
                {
                    w.Worker.Run();
                }
                SharedData._running = true;
                StartBtn.Content = "Pause";
            }
            else if (StartBtn.Content.ToString() == "Pause")
            {
                foreach (WorkerModel w in SharedData._workerList.Workers)
                {

                    w.Worker.Pause();
                }
                SharedData._running = false;
                StartBtn.Content = "Start";
            } 
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e){
            List<Worker> _swl = SharedData._removedWorkers;
            SharedData._workerList.Workers.ToList().ForEach(x => _swl.Add(x.Worker));
            Program.saveToFile(_swl, "Output.xls");
        }
    }
}
