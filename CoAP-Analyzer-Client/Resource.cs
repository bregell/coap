
namespace CoAP_Analyzer_Client
{
    public class Resource
    {
        string _name;
        int _timeout;
        int _param;
        int _rate;
        string _path;
        //Type _type;

        public Resource(int _tt, int _pm, string _ph, string _n, int _rate)
        {
            Timeout = _tt;
            Param = _pm;
            Path = _ph;
            Name = _n;
            Rate = _rate;
        }

        //public Type Type
        //{
        //    get
        //    {
        //        return _type;
        //    }
        //    set
        //    {
        //        _type = value;
        //    }
        //}
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public int Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
            }
        }
        public int Param
        {
            get
            {
                return _param;
            }
            set
            {
                _param = value;
            }
        }

        public int Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }
    }
}
