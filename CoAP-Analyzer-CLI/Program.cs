using System;
using System.Data;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using CoAP.Server;
using CoAP;
using CoAP.Net;
using CoAP.Stack;
using CoAP.Util;
using System.Threading;
using System.Linq;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;


namespace CoAP_Analyzer_CLI
{
    class Program
    {    
        static int _rate = 1000*30;
        static int _samples = 10;
        static void Main(string[] args)
        {
            List<Worker> workers = new List<Worker>{};
            List<Host> hosts = new List<Host>{
                new Host("[aaaa::212:4b00:60d:9abb]"),
                new Host("[aaaa::212:4b00:60d:9b57]"),
                new Host("[aaaa::212:4b00:60d:9b59]"),
                new Host("[aaaa::212:4b00:60d:9ac3]")
            };
            foreach(Host h in hosts)
            {
                workers.Add(new Worker(h, h.Temp,       _rate, 0));
                workers.Add(new Worker(h, h.Vcc3,       _rate, 0));
                workers.Add(new Worker(h, h.Ping,       _rate, 0));
                workers.Add(new Worker(h, h.Humidity,   _rate, 0));
                workers.Add(new Worker(h, h.Light,      _rate, 0));
                workers.Add(new Worker(h, h.Troughput,  _rate, 1024));    
            }
            foreach (Worker w in workers)
            {
                new Thread(w.Work).Start();
                Thread.Sleep(_rate / workers.Count);
            }
            Thread.Sleep(_rate*_samples);
            foreach(Worker w in workers)
            {
                w.Stop();
            }
            while(!workers.All(w => w._done == true));
            saveToFile(workers, "Output.xls");   
        }

        private static void saveToFile(List<Worker> workers, string filename)
        {
            DataSet ds = new DataSet("Output");
            List<DataTable> _tables = new List<DataTable>();
            List<String> _tablenames = new List<String> { "Vcc3", "Ping", "Troughput", "Hops", "Temp", "Light", "Humidity" };
            foreach (String s in _tablenames)
            {
                DataTable _table = new DataTable(s);
                _table.Columns.Add(new System.Data.DataColumn("Ip"));
                _table.Columns.Add(new System.Data.DataColumn(s, System.Type.GetType("System.Double")));
                _table.Columns.Add(new System.Data.DataColumn("Unit"));
                _table.Columns.Add(new System.Data.DataColumn("Time"));
                _tables.Add(_table);
            }
            foreach (Worker w in workers)
            {
                foreach (Measure m in w.measure)
                {
                    _tables.Find(x => x.TableName == w.methodToRun.Method.Name).Rows.Add(w.h.ip, m.value, m.unit, m.time);
                }
            }
            foreach (DataTable t in _tables)
            {
                ds.Tables.Add(t);
            }
            ExcelLibrary.DataSetHelper.CreateWorkbook(filename, ds);
        }
    }

    [DataContract]
    class Temp
    {
        [DataMember]
        public double temp { get; set; }
        [DataMember]
        public string unit { get; set; }

        public Temp(double val, string u)
        {
            temp = val;
            unit = u;
        }
    }

    [DataContract]
    class Humididy
    {
        [DataMember]
        public double humidity;
        [DataMember]
        public string unit; 
        public Humididy(double val, string u)
        {
            humidity = val;
            unit = u;
        }
    }

    [DataContract]
    class Light
    {
        [DataMember]
        public double light;
        [DataMember]
        public string unit; 
        public Light(double val, string u)
        {
            light = val;
            unit = u;
        }
    }

    [DataContract]
    class Vcc3
    {
        [DataMember]
        public double voltage;
        [DataMember]
        public string unit;
        public Vcc3(double val, string u)
        {
            voltage = val;
            unit = u;
        }

    }

    class Measure
    {
        public double value;
        public string unit;
        public DateTime time;

        public Measure(double v, string u,  DateTime t)
        {
            unit = u;
            value = v;
            time = t;
        }
    }
}