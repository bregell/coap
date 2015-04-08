using System;
using System.Collections.Generic;
using System.Text;
using CoAP.Server.Resources;
using CoAP;


namespace CoAP_Analyzer_Server.Resources
{
    class Storage : Resource
    {
        private String _content;

        public Storage(String name)
            : base(name)
        {
            Attributes.Title = "Buffer";
        }

        protected override void DoGet(CoapExchange exchange)
        {
            if (_content != null)
            {
                
                exchange.Respond(_content);
            }
            else
            {
                String subtree = LinkFormat.Serialize(this, null);
                exchange.Respond(StatusCode.Content, subtree, MediaType.ApplicationLinkFormat);
            }
        }

        protected override void DoPut(CoapExchange exchange)
        {
            _content = exchange.Request.PayloadString;
            exchange.Respond(StatusCode.Changed);
        }
    }
}
