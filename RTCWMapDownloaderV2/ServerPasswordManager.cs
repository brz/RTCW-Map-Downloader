using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RTCWMapDownloader
{
    public static class ServerPasswordManager
    {
        public static string UserDataPath
        {
            get
            {
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appDataFolder, "Breez", "RTCW Map Downloader");
            }
        }

        public static string ServerPasswordManagerFile
        {
            get
            {
                return Path.Combine(UserDataPath, "serverpasswordmanager.json");
            }
        }

        public static List<ServerPasswordItem> GetCurrentItems()
        {
            if (!File.Exists(ServerPasswordManagerFile)) return new List<ServerPasswordItem>();
            return JsonConvert.DeserializeObject<List<ServerPasswordItem>>(File.ReadAllText(ServerPasswordManagerFile));
        }

        public static void AddItem(ServerPasswordItem serverPasswordItem)
        {
            var currentServerPasswordItems = GetCurrentItems();
            currentServerPasswordItems.Add(serverPasswordItem);

            if (!Directory.Exists(UserDataPath))
            {
                Directory.CreateDirectory(UserDataPath);
            }
            File.WriteAllText(ServerPasswordManagerFile, JsonConvert.SerializeObject(currentServerPasswordItems));
        }

        public static void SaveItems(List<ServerPasswordItem> serverPasswordItems)
        {
            File.WriteAllText(ServerPasswordManagerFile, JsonConvert.SerializeObject(serverPasswordItems));
        }

        public static string GetPasswordForServer(string ipAndPort)
        {
            var splitted = ipAndPort.Split(':');
            var ip = splitted[0];
            var port = splitted[1];

            var allItems = GetCurrentItems();
            return allItems.FirstOrDefault(i => i.Ip == ip && i.Port == port)?.Password;
        }
    }
}
