using System;
using System.Runtime.Serialization;

namespace CoAP_Analyzer_Client
{
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

    public class Measure
    {
        public double value;
        public string unit;
        public DateTime time;

        public Measure(double v, string u, DateTime t)
        {
            unit = u;
            value = v;
            time = t;
        }
    }
}
