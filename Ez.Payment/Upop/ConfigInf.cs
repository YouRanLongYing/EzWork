using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Payment.Contract;

namespace Ez.Payment.Upop
{
    public class ConfigInf
    {
        public string signMethod;
        public string securityKey;
        public string frontPayURL;
        public string backPayURL;
        public string queryURL;
        public string notify_url;
        public string return_url;
        public string SSLCertPolicy;
        public string SSLCertStorePath;
        public bool PostExpect100Continue;
        public StrDictSerializable payParamsPredef;
        public string[] payParams;
        public string[] payParamsNotEmpty;
        public string[] queryParams;
        public string[] merReservedParams;
    }
}
