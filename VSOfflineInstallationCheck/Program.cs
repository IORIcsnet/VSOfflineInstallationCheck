using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSOfflineInstallationCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> LSjson = new List<string>();
            List<string> LSfile = new List<string>();
            JObject JO;
            using (TextReader reader = File.OpenText(@"E:\vs2017\Catalog.json"))
            {
                JO = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            }
            JArray JA = (JArray)JO["packages"];
            foreach (JObject jo in JA)
            {
                if (jo["payloads"] != null)
                {
                    string id = (string)jo["id"];
                    string version = (string)jo["version"];
                    string chip = "";
                    if (jo["chip"] != null)
                    {
                        chip = (string)jo["chip"];
                    }
                    string language = "";
                    if (jo["language"] != null)
                    {
                        language = (string)jo["language"];
                    }
                    if (language != "" && language != "zh-CN" && language != "zh-cn" && language != "neutral")
                    {
                        continue;
                    }
                    string s = id + "," + "version=" + version;
                    if (chip != "")
                    {
                        s += "," + "chip=" + chip;
                    }
                    if (language != "")
                    {
                        s += "," + "language=" + language;
                    }
                    LSjson.Add(s);
                }
            }
            LSjson.Add("Archive");
            LSjson.Add("certificates");
            string[] files = Directory.GetDirectories(@"E:\vs2017");

            foreach (string s in files)
            {
                LSfile.Add(Path.GetFileName(s));
            }
            foreach (string s in LSfile)
            {
                if (!LSjson.Contains(s))
                {
                    Directory.Delete(@"E:\vs2017\" + s, true);
                    Console.WriteLine(s);
                }
            }

            Console.ReadKey();

            Console.ReadKey();
        }
    }
}
