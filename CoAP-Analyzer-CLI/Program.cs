using System;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;
using CoAP.Log;

namespace CoAP_Analyzer_Client
{

    public class Program
    {    
        static int _rate = 1000*60;
        static void Main(string[] args)
        {
            LogManager.Level = LogLevel.Warning;
            List<Worker> _workers = new List<Worker>{ };
            List<Thread> _threads = new List<Thread> { };
            List<Host> _hosts = new List<Host>{
                new Host("[aaaa::212:4b00:60d:9abb]"),
                new Host("[aaaa::212:4b00:60d:9b57]"),
                new Host("[aaaa::212:4b00:60d:9b59]"),
                new Host("[aaaa::212:4b00:60d:9ac3]")
            };
            foreach(Host h in _hosts)
            {
                _workers.Add(new Worker(h, h.Temp,       _rate, 0));
                _workers.Add(new Worker(h, h.Vcc3,       _rate, 0));
                _workers.Add(new Worker(h, h.Ping,       _rate, 0));
                _workers.Add(new Worker(h, h.Humidity,   _rate, 0));
                _workers.Add(new Worker(h, h.Light,      _rate, 0));
                _workers.Add(new Worker(h, h.Troughput,  _rate, 1024));    
            }
            int i = 0;
            _workers.Sort(new WorkerComparer());
            foreach (Worker w in _workers)
            {   
                w._startTime = (_rate/_workers.Count)*i++;
                Thread t = new Thread(w.Work);
                _threads.Add(t);
                t.Start();
            }
            while (true)
            {
                Console.WriteLine("Enter any Key: ");
                ConsoleKeyInfo name = Console.ReadKey();
                if (name.KeyChar == 's')
                {
                    saveToFile(_workers, "Output.xls");
                }
                else if (name.KeyChar == 'c')
                {
                    break;
                }
            }
            foreach (Worker w in _workers)
            {
                
                w.Stop();
            }
            Console.WriteLine("Waiting for all _workers to finish!");
            while (!_workers.All(w => w._done == true)) ;
            saveToFile(_workers, "Output.xls");   
        }



        public static void saveToFile(List<Worker> _workers, string _filename)
        {
            DataSet _ds = new DataSet("Output");
            List<DataTable> _tables = new List<DataTable>();
            List<String> _tablenames = new List<String> { "Vcc3", "Ping", "Troughput", "Temp", "Light", "Humidity" };
            foreach (String s in _tablenames)
            {
                DataTable _table = new DataTable(s);
                _table.Columns.Add(new System.Data.DataColumn("Ip"));
                _table.Columns.Add(new System.Data.DataColumn(s, System.Type.GetType("System.Double")));
                _table.Columns.Add(new System.Data.DataColumn("Unit"));
                _table.Columns.Add(new System.Data.DataColumn("Time"));
                _tables.Add(_table);
            }
            foreach (Worker w in _workers)
            {
                foreach (Measure m in w._measure)
                {
                    _tables.Find(x => x.TableName == w._methodToRun.Method.Name).Rows.Add(w._host.IP, m.value, m.unit, m.time);
                }
            }
            foreach (DataTable t in _tables)
            {
                _ds.Tables.Add(t);
            }
            bool _saved = false;
            while (!_saved)
            {
                try
                {
                    ExcelLibrary.DataSetHelper.CreateWorkbook(_filename, _ds);
                    _saved =  true;
                }
                catch (Exception)
                {
                    Console.Write("Cannot save to that file, enter new filename!:");
                    _filename = Console.ReadLine();
                    Console.Write("\n");
                }
            }
                
            
            
        }
    }  
}