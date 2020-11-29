using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace BannerReload.Data
{
    class GlobalDataProvider
    {

        private static GlobalDataProvider _instance;

        private List<BannerRecordData> _bannerRecordData;

        public static GlobalDataProvider Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new GlobalDataProvider();
                }

                return _instance;
            }
        }

        public List<BannerRecordData> BannerRecordData()
        {
            if(null == _bannerRecordData)
            {
                _bannerRecordData = new List<BannerRecordData>();
            }

            return this._bannerRecordData;
        }

        private GlobalDataProvider()
        {
            this.LoadBannerData();
        }

        public void LoadBannerData()
        {
            string path = System.IO.Path.Combine(Utilities.GetConfigsPath(), "BannerReload", "BannerReload.json");
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                try
                {
                    using (StreamReader streamReader = fileInfo.OpenText())
                    {
                        string json = streamReader.ReadToEnd();
                        if (json != null)
                        {
                            this._bannerRecordData = (List<BannerRecordData>)JsonConvert.DeserializeObject(json, typeof(List<BannerRecordData>));
                        }

                    }
                }
                catch (JsonException e)
                {
                    InformationManager.DisplayMessage(new InformationMessage("BannerReload load BannerRecordData failed" + e.Message));
                }
            }
        }

        public void SaveBannerData()
        {
            try
            {
                string dic = System.IO.Path.Combine(Utilities.GetConfigsPath(), "BannerReload");
                string path = System.IO.Path.Combine(dic, "BannerReload.json");

                System.IO.Directory.CreateDirectory(dic);

                string json = JsonConvert.SerializeObject(this._bannerRecordData, Formatting.Indented);
                File.WriteAllText(path, json);
               // StreamWriter streamWriter = new StreamWriter(path, false);
               // streamWriter.Write(json);
               // streamWriter.Flush();// 清空缓存
               // streamWriter.Close();
            }
            catch (JsonException e)
            {
                InformationManager.DisplayMessage(new InformationMessage("BannerReload save BannerRecordData failed" + e.Message));
            }
        }
    }
}
