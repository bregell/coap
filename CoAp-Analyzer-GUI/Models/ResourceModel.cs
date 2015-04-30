using CoAP_Analyzer_Client;
using CoAP_Analyzer_Client.Models;

namespace CoAP_Analyzer_GUI.Models
{
    public class ResourceModel : BaseModel
    {
        Resource _res;

        public ResourceModel()
        {

        }

        public Resource Resource
        {
            get
            {
                return _res;
            }
            set
            {
                _res = value;
                RaisePropertyChanged("Resource");
            }
        }

    }
}
