using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace POETradeAnnouncer
{
    internal class LocalCommunicator
    {
        public JObject GetFile(string fileName)
        {
            JObject file = JsonConvert.DeserializeObject<JObject>(GetFileRaw(fileName));

            if (file != null)
            {
                return (JObject)file;
            }

            return new JObject();
        }

        public bool ConfigExists()
        {
            if(File.Exists("config.json"))
            {
                return true;
            }
            return false;
        }
        public ConfigModel GetConfig()
        {
            ConfigModel model = JsonConvert.DeserializeObject<ConfigModel>(GetFileRaw("config"));

            if(model != null)
            {
                Console.WriteLine("Config file loaded!", Color.Green);
                Console.WriteLine($"FilePath:{model.FilePath}", Color.LightGoldenrodYellow);
                Console.WriteLine($"Volume:{model.Volume}", Color.LightGoldenrodYellow);
                return model;
            }


            Console.WriteLine("No config file found!", Color.Green);
            return null;
        }

        public void RewriteFile(string fileName, ConfigModel model)
        {
            string file = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText(fileName + ".json", file);
        }
        private string GetFileRaw(string fileName)
        {
            string jsonContent = string.Empty;
            using (FileStream fileStream = File.OpenRead(fileName + ".json"))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
                {

                    jsonContent = streamReader.ReadToEnd();

                }
            }

            return jsonContent;
        }

        public List<string> GetLastRowsOfFile(string fileName)
        {
            string jsonContent = string.Empty;

            string path2 = fileName.Substring(0, fileName.Length - 4) + "_1.txt";
            File.Delete(path2);
            File.Copy(fileName, path2);

            using (FileStream fileStream = File.OpenRead(path2))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
                {

                    jsonContent = streamReader.ReadToEnd();

                }
            }

            var list = File.ReadLines(path2).ToList();
            list.Reverse();

            List<string> listOfStrings = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                listOfStrings.Add(list[i]);
            }
            return listOfStrings;
        }


    }
}
