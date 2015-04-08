using System;
using System.Text;
using CoAP.Server.Resources;
using CoAP;

namespace CoAP_Analyzer_Server.Resources
{
    class Temp : Resource
    {
        Random rnd = new Random();
        public Temp(String name)
            : base(name)
        {
            Attributes.Title = "Temperature";
        }

        protected override void DoGet(CoapExchange exchange)
        {
            Response res = new Response(StatusCode.Content);
            int val = rnd.Next(20, 30);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"light\":");
            sb.Append(val.ToString());
            sb.Append("}");
            res.Payload = Encoding.UTF8.GetBytes(sb.ToString());
            res.ContentType = CoAP.MediaType.ApplicationJson;
            exchange.Respond(res);
        }
    }
}
