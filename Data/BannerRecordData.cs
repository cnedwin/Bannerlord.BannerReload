using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BannerReload.Data
{
    class BannerRecordData
    {

        public string SaveName { get; set; }

        public string DateString { get; set; }

        public string BannerCode { get; set; }

        public BannerRecordData(string name, string bannerRecordString)
        {
            SaveName = name;
            BannerCode = bannerRecordString;
            DateString = DateTime.Now.ToString(); 
        }
    }
}
