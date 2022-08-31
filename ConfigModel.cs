using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POETradeAnnouncer
{
    internal class ConfigModel
    {
        [JsonProperty("volume")]
        public int Volume { get; set; }

        [JsonProperty("file_path")]
        public string FilePath { get; set; }
    }


}
