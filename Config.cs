using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 简易桌面整理
{
    static class Config
    {
        static string ConfigFileFullName = "config.json";
        static JObject _config { get; set; }
        static Config()
        {
            if (File.Exists(ConfigFileFullName))
            {
                _config = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(ConfigFileFullName));
            }
            else
            {
                InitConfig();
                string _str = JsonConvert.SerializeObject(_config);
#if DEBUG
#else
                File.WriteAllText(ConfigFileFullName, _str);
#endif
            }
        }
        public static T Get<T>(string key)
        {
            if (_config.ContainsKey(key))
            {
                return _config[key].ToObject<T>();
            }
            return default(T);
        }
        static void InitConfig()
        {
            Dictionary<string, string> defaultRules = new Dictionary<string, string>();
            defaultRules.Add("文件夹", "^#dir#");
            defaultRules.Add("应用程序", "\\.lnk|\\.exe");
            defaultRules.Add("图片文件", "\\.jpg$|\\.png$|\\.jpeg$|\\.bmp$|\\.gif$|\\.tif$|\\.webp$|\\.svg$");
            defaultRules.Add("文档", "\\.txt$|\\.doc$|\\.docx$|\\.pdf$|\\.html$|\\.htm$|\\.xls|\\.xlsx");
            defaultRules.Add("音频文件", "\\.wav|\\.mp3|\\.wma|\\.ogg|\\.midi|\\.ape|\\.flac|\\.aac");
            defaultRules.Add("视频文件", "\\.wmv|\\.mp4|\\.rmvb|\\.avi|\\.mpeg|\\.mpg|\\.rm|\\.flv|\\.mov|\\.mkv|\\.f4v|\\.m4v|\\.webm|\\.3gp|\\.qt");
            defaultRules.Add("其他文件", ".*");
            _config = JObject.FromObject(new
            {
                rules = defaultRules,
                interval = 1000,
                enable_on_start=true,
                show_on_start=true
            });
        }
    }
}
