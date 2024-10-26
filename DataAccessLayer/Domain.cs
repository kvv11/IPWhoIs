using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Domain_registrar
    {
        public string registrar_name { get; set; }
        public string website_url { get; set; }
  
    }

    public class Domain
    {
        public string domain_registered { get; set; }
        public string whois_server { get; set; }
        public DateTime create_date { get; set; }
        public DateTime update_date { get; set; }
        public DateTime expiry_date { get; set; }
        public Domain_registrar domain_registrar { get; set; }
    }

}
