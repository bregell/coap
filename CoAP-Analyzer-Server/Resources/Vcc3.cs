using System;
using System.Text;
using CoAP.Server.Resources;
using CoAP;

namespace CoAP_Analyzer_Server.Resources
{
    class Vcc3 : Resource
    {
        Random rnd;
        public Vcc3(String name)
            : base(name)
        {
            Attributes.Title = "Voltage";
            rnd = new Random();
        }

        protected override void DoGet(CoapExchange exchange)
        {
            Response res = new Response(StatusCode.Content);
            int val = rnd.Next(1900, 2250);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"vcc3\":");
            sb.Append(val.ToString());
            sb.Append("}");
            res.Payload = Encoding.UTF8.GetBytes(sb.ToString());
            res.ContentType = CoAP.MediaType.ApplicationJson;
            exchange.Respond(res);
        }
    }
}
