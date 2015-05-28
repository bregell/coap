using System;
using System.Runtime.Serialization;

namespace CoAP_Analyzer_Client
{
    [DataContract]
    class TempC : Measure
    {
        [DataMember]
        public double Temp 
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        public TempC(double val, string u)
        {
            Temp = val;
            Unit = u;
        }
    }

    [DataContract]
    class HumididyC : Measure
    {
        [DataMember]
        public double Humidity
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        public HumididyC(double val, string u)
        {
            Humidity = val;
            Unit = u;
        }
    }

    [DataContract]
    class LightC : Measure
    {
        [DataMember]
        public double Light
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        public LightC(double val, string u)
        {
            Light = val;
            Unit = u;
        }
    }

    [DataContract]
    class VoltageC : Measure
    {
        [DataMember]
        public double Voltage
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        public VoltageC(double val, string u)
        {
            Voltage = val;
            Unit = u;
        }

    }

    [DataContract]
    public class Measure
    {
        double _value;
        string _unit;
        DateTime _time;

        public Measure()
        {

        }

        public Measure(double _v, string _s)
        {
            Value = _v;
            Unit = _s;
            Time = DateTime.Now;
        }

        public Measure(double _v, string _s, DateTime _dt)
        {
            Value = _v;
            Unit = _s;
            Time = _dt;
        }

        //TODO Change to value 
        [DataMember(Name="value")]
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        [DataMember(Name="unit")]
        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }
    }
}
