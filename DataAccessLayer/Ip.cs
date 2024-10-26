using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{



    public class Connection
    {
        public string domain { get; set; }
        public string org { get; set; }
        public string isp { get; set; }

    }

    public class Timezone
    {
        public string id { get; set; }
        public string utc { get; set; }
       

    }

 

 

    public class Ip
    {
        public string ip { get; set; }
        public string type { get; set; }
        public string continent { get; set; }
        public string continent_code { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }
        public string region { get; set; }
        public string region_code { get; set; }
        public string city { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string postal { get; set; }
        public string calling_code { get; set; }
        public string capital { get; set; }
        public string borders { get; set; }
        public Connection connection { get; set; }
        public Timezone timezone { get; set; }
        
    }

}
