using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Wsashi.Entities;

namespace Wsashi.Core
{
    public static class DataStorage
    {
        public static void SaveUserAccounts(IEnumerable<Entities.GlobalUserAccount> accounts, string filePath)
        {
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static IEnumerable<GlobalUserAccount> LoadUserAccounts(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GlobalUserAccount>>(json);
        }

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}