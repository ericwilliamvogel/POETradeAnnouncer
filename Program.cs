using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.IO;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Console = Colorful.Console;
using System.Drawing;

namespace POETradeAnnouncer
{
    internal class Program
    {
        static SpeechSynthesizer _speechSynth = new SpeechSynthesizer();
        static Stack<string> _queue = new Stack<string>(10);
        static LocalCommunicator fileIO = new LocalCommunicator();
        static string path = string.Empty;
        static async Task Main(string[] args)
        {
            Console.WriteLine("POETradeAnnouncer v1.0", Color.Gold);
            Console.WriteLine("Greetings exiles, welcome to the POETradeAnnouncer. Everytime you get a whisper, you will receive a friendly prompt from your local Microsoft TTS representative!", Color.White);
            Console.WriteLine("This app was made by the lazy, for the lazy.", Color.Green);
            Console.WriteLine();

            string warning = "WARNING:";
            string warningMessage = "{0} POETradeAnnouncer operates from your POE logs folder! If you care about your logs make sure to back them up!";

            Console.WriteLineFormatted(warningMessage, Color.Purple, Color.LightGoldenrodYellow, warning);
            Console.WriteLine("You can find your logs folder in your steamapps directory -> common -> PathOfExile -> logs.", Color.LightGoldenrodYellow);

            if(!fileIO.ConfigExists())
            {
                Console.WriteLine();
                Console.WriteLine("!!!");
                Console.WriteLine("Your config file doesn't exist yet! Let's set that up.", Color.Green);
                Console.WriteLine("!!!");
                Console.WriteLine();
                Console.WriteLine();


                Console.WriteLine("After you backed up your Client.txt file in your PathOfExile logs folder, enter the full path of the Client.txt file on your machine below!", Color.Pink);
                Console.Write("Path: ", Color.Yellow);

                string newPath = Console.ReadLine();
                if(newPath==null)
                {
                    Console.WriteLine("Oops, you entered null! Restart the application to try again!", Color.Red);
                    return;
                }
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("What volume do you want the app to speak at? You can always change this volume in the config file or in your Volume Mixer. Enter a number value from 1 to 100", Color.Pink);
                Console.Write("Volume: ", Color.Yellow);

                string newVolume = Console.ReadLine();
                if (newVolume == null)
                {
                    Console.WriteLine("Oops, you entered null! Restart the application to try again!", Color.Red);
                    return;
                }

                if(!Int32.TryParse(newVolume, out int parsedVolume)) {
                    Console.WriteLine("Oops, you entered a non-integer! Restart the application to try again!", Color.Red);
                    return;
                }

                fileIO.RewriteFile("config", new ConfigModel() { FilePath = newPath, Volume = parsedVolume });
                Console.WriteLine();
                Console.WriteLine("All done!", Color.Green);
                Console.WriteLine("You can always reset these values by going into your config.json file in the directory where you keep this app!", Color.Yellow);
            }

            ConfigModel model = fileIO.GetConfig();
            if(model == null)
            {
                Console.WriteLine("Oops, your config file is either missing or using an unrecognized json schema! Fix the schema to only include fields 'Volume' and 'FilePath' then restart the application to try again!", Color.Red);
            }

            path = model.FilePath;

            _speechSynth.Volume = model.Volume;
            await UpdateLoop(5000);
        }

        static async Task UpdateLoop(int ms)
        {
            await Task.Delay(ms);
            var fileInfo = fileIO.GetLastRowsOfFile(path);

            foreach (var line in fileInfo)
            {
                if (line.Contains("divine") && line.Contains("From") && !_queue.Contains(line))
                {
                    _queue.Push(line);
                    Console.WriteLine("Divine message");
                    int index = line.IndexOf("for ") + 4;
                    char next = '1';
                    string number = "";
                    int iterator = index;
                    while (next != ' ')
                    {
                        number += line[iterator];
                        iterator++;

                        next = line[iterator];
                    }

                    _speechSynth.SelectVoice("Microsoft Zira Desktop");
                    _speechSynth.SpeakAsync($"A trade for {number} divines is waiting.");

                }
                else if (line.Contains("chaos") && line.Contains("From") && !_queue.Contains(line))
                {
                    _queue.Push(line);
                    Console.WriteLine("Chaos message");
                    int index = line.IndexOf("for ") + 4;
                    char next = '1';
                    string number = "";
                    int iterator = index;

                    while (next != ' ')
                    {
                        number += line[iterator];
                        iterator++;

                        next = line[iterator];
                    }

                    _speechSynth.SelectVoice("Microsoft David Desktop");
                    _speechSynth.SpeakAsync($"A trade for {number} chaos orbs is waiting.");

                }

            }

            await UpdateLoop(ms);

        }
    }


}
