﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using CoAP.Server;
using CoAP;
using CoAP.Net;
using CoAP.Stack;
using CoAP.Util;
using System.Threading;
using ExcelLibrary;

namespace CoAP_Analyzer_CLI
{
    class Host
    {
        public IPAddress ip { get; private set; }

        private CoapConfig _conf;
        private IEndPoint _endpoint;

        public Host(string ip)
        {
            this.ip = IPAddress.Parse(ip);

            _conf = new CoapConfig();
            _conf.DefaultBlockSize = 64;
            _conf.MaxMessageSize = 64;
            _endpoint = new CoAPEndPoint(_conf);
            _endpoint.Start();
        }

        public Measure Temp(int timeout)
        {
            timeout = (timeout == 0) ? System.Threading.Timeout.Infinite : timeout;
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "sensors/temp").Uri;
            req.URI = uri;

            //Send Package
            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null && req.Response.PayloadSize != 0)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Temp));
                try {
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    Temp t = (Temp)ser.ReadObject(stream1);
                    return new Measure(t.temp, t.unit, DateTime.Now);
                }
                catch (Exception)
                {
                    return new Measure(-1, "Serialization Error", DateTime.Now);
                }
            }
                return new Measure(-1, "Timeout", DateTime.Now);
        }

        public Measure Light(int timeout)
        {
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "sensors/light").Uri;
            req.URI = uri;

            //Send Package
            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null && req.Response.PayloadSize != 0)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Light));
                try {    
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    Light l = (Light)ser.ReadObject(stream1);
                    return new Measure(l.light, l.unit, DateTime.Now);
                }
                catch (Exception)
                {
                    return new Measure(-1, "Serialization Error", DateTime.Now);
                }
            }
            return new Measure(-1, "Timeout", DateTime.Now);
        }

        public Measure Humidity(int timeout)
        {
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "sensors/humidity").Uri;
            req.URI = uri;

            //Send Package
            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null && req.Response.PayloadSize != 0)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Humididy));
                try {
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    Humididy h = (Humididy)ser.ReadObject(stream1);
                    return new Measure(h.humidity, h.unit, DateTime.Now);
                }
                catch (Exception)
                {
                    return new Measure(-1, "Serialization Error", DateTime.Now);
                }
            }
            return new Measure(-1, "Timeout", DateTime.Now);
        }

        public Measure Vcc3(int timeout)
        {
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            req.URI = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "sensors/vdd3").Uri;

            //Send Package
            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null && req.Response.PayloadSize != 0)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Vcc3));
                try
                {
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    Vcc3 v = (Vcc3)ser.ReadObject(stream1);
                    return new Measure(v.voltage, v.unit, DateTime.Now);
                }
                catch (Exception)
                {
                    return new Measure(-1, "Serialization Error", DateTime.Now);
                }
                        
            }
            return new Measure(-1, "Timeout", DateTime.Now);
        }

        public Measure Hops(int timeout)
        {
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "info/hops").Uri;
            req.URI = uri;

            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null  && req.Response.PayloadSize != 0)
            {
                return new Measure(Convert.ToInt32(req.Response.Payload.ToString()), "Hops", DateTime.Now);
            }
            return new Measure(-1, "Timeout", DateTime.Now);
        }

        public Measure Ping(int timeout)
        {
            Request req = new Request(Method.GET);
            req.URI = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort).Uri;

            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null)
            {
                    return new Measure(req.Response.RTT, "ms", DateTime.Now);
            }
            else
            {
                return new Measure(-1, "Timeout", DateTime.Now);
            }
        }

        public Measure Troughput(int bytes)
        {
            //Prepare the package
            String payload = new String('X', bytes);
            Request req = new Request(Method.PUT);
            req.SetPayload(payload, MediaType.TextPlain);
            req.URI = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "data/buffer").Uri;

            //Send Package
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            req.Send(_endpoint);
            req.Response = req.WaitForResponse(100000);
            stopWatch.Stop();
            if (req.Response != null){
                return new Measure((double)bytes / (double)stopWatch.ElapsedMilliseconds, "bytes/ms", DateTime.Now);
            }
            else
            {
                return new Measure(-1, "Timeout", DateTime.Now);
            }
        }
    }
}
