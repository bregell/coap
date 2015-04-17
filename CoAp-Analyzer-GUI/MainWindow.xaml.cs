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
using System.ComponentModel;
using System.Diagnostics;
using CoAP_Analyzer_GUI.Models;

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
        public static ChartModel _cm = new ChartModel();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowModel _mwm;
        public MainWindow()
        {
            InitializeComponent();
            _mwm = new MainWindowModel();
            _mwm.addNavigation(new HostListModel() { Name = "Hosts" }); // Workers = SharedData._workerList.Workers 
            _mwm.addNavigation(new WorkerListModel() { Name = "Workers" }); //Hosts = SharedData._hostList.Hosts
            _mwm.addNavigation(new MeasureListModel() { Name = "Measures" }); //Measures = SharedData._measureList.Measures
            _mwm.addNavigation(new ChartModel() { Name = "Charts" });
            this.DataContext = _mwm;     
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StartBtn.Text.ToString() == "Start")
            {
                foreach (WorkerModel w in SharedData._workerList.Workers)
                {
                    w.Worker.Run();
                }
                SharedData._running = true;
                StartBtn.Text = "Pause";
            }
            else if (StartBtn.Text.ToString() == "Pause")
            {
                foreach (WorkerModel w in SharedData._workerList.Workers)
                {

                    w.Worker.Pause();
                }
                SharedData._running = false;
                StartBtn.Text = "Start";
            } 
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e){
            List<Worker> _swl = SharedData._removedWorkers;
            SharedData._workerList.Workers.ToList().ForEach(x => _swl.Add(x.Worker));
            Program.saveToFile(_swl, "Output.xls");
        }
    }
}
