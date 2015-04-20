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
using CoAP_Analyzer_GUI.UserControls;

namespace CoAP_Analyzer_GUI
{

    public static class SharedData
    {
        public static MainWindowModel _mwm = new MainWindowModel();
        public static HostListModel _hostList = new HostListModel(){ Name = "_Hosts", Command = new RelayCommand(param => _command(SharedData._hostList), param => true) };
        public static WorkerListModel _workerList = new WorkerListModel() { Name="_Workers", Command = new RelayCommand(param => _command(SharedData._workerList), param => true) };
        public static MeasureListModel _measureList = new MeasureListModel() { Name="_Measures", Command = new RelayCommand(param => _command(SharedData._measureList), param => true) }; 
        public static ChartListModel _chartList = new ChartListModel() { Name="_Create Chart", Command = new RelayCommand(param => _command(SharedData._chartList), param => true) };
        public static Action<object> _command;
        public static List<Worker> _removedWorkers = new List<Worker>();
        public static List<Thread> _threads = new List<Thread>();
        public static bool _running = false;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowModel _mwm = SharedData._mwm;
        public MainWindow()
        {
            SharedData._command = ChangeView;
            _mwm.addNavigation(SharedData._hostList); // Workers = SharedData._workerList.Workers 
            _mwm.addNavigation(SharedData._workerList); //Hosts = SharedData._hostList.Hosts
            _mwm.addNavigation(SharedData._measureList); //Measures = SharedData._measureList.Measures
            _mwm.addNavigation(SharedData._chartList);
            this.DataContext = _mwm;
            InitializeComponent();
            
        }

        private void ChangeView(object sender)
        {
            GridContent.Content = sender;
        }

        private bool CanExe
        {
            get { return true; }   
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!SharedData._running){
                foreach (WorkerModel w in SharedData._workerList.Workers)
                {
                    w.Worker.Run();
                    w.Worker.Host.Running = true;
                }
                SharedData._running = true;
                ((Button)sender).Content = "Stop";
            }
            else
            {
                foreach (WorkerModel w in SharedData._workerList.Workers)
                {

                    w.Worker.Pause();
                    w.Worker.Host.Running = false;
                }
                SharedData._running = false;
                ((Button)sender).Content = "_Start";
            } 
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e){
            List<Worker> _swl = SharedData._removedWorkers;
            SharedData._workerList.Workers.ToList().ForEach(x => _swl.Add(x.Worker));
            Program.saveToFile(_swl, "Output.xls");
        }
    }
}
