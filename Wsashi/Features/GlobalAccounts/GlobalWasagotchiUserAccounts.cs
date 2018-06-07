using Wsashi.Configuration;
using System;
using Wsashi.Entities;
using Discord;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Wsashi.Features.GlobalAccounts
{
    internal static class GlobalWasagotchiUserAccounts
    {
        private static readonly ConcurrentDictionary<ulong, GlobalWasagotchiUserAccount> wgAccounts = new ConcurrentDictionary<ulong, GlobalWasagotchiUserAccount>();

        static GlobalWasagotchiUserAccounts()
        {
            var info = System.IO.Directory.CreateDirectory(Path.Combine(Constants.ResourceFolder, Constants.WasagotchiAccountsFolder));
            var files = info.GetFiles("*.json");
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    var user = Configuration.DataStorage.RestoreObject<GlobalWasagotchiUserAccount>(Path.Combine(file.Directory.Name, file.Name));
                    wgAccounts.TryAdd(user.Id, user);
                }
            }
            else
            {
                wgAccounts = new ConcurrentDictionary<ulong, GlobalWasagotchiUserAccount>();
            }
        }

        internal static GlobalWasagotchiUserAccount GetWasagotchiAccount(ulong id)
        {
            return wgAccounts.GetOrAdd(id, (key) =>
            {
                var newAccount = new GlobalWasagotchiUserAccount { Id = id };
                Configuration.DataStorage.StoreObject(newAccount, Path.Combine(Constants.WasagotchiAccountsFolder, $"{id}.json"), useIndentations: true);
                return newAccount;
            });
        }

        internal static GlobalWasagotchiUserAccount GetWasagotchiAccount(IUser user)
        {
            return GetWasagotchiAccount(user.Id);
        }

        internal static GlobalWasagotchiUserAccount GetWasagotchiAccounts(IUser user , List<Entities.GlobalWasagotchiUserAccount> accts)
        {
            return GetWasagotchiAccount(user.Id);

        }
        internal static List<GlobalWasagotchiUserAccount> GetAllWasagotchiAccount()
        {
            return wgAccounts.Values.ToList();
        }

        internal static List<GlobalWasagotchiUserAccount> GetFilteredAccounts(Func<GlobalWasagotchiUserAccount, bool> filter)
        {
            return wgAccounts.Values.Where(filter).ToList();
        }

        /// <summary>
        /// This rewrites ALL UserAccounts to the harddrive... Strongly recommend to use SaveAccounts(id1, id2, id3...) where possible instead
        /// </summary>
        internal static void SaveAccounts()
        {
            foreach (var id in wgAccounts.Keys)
            {
                SaveAccounts(id);
            }
        }

        /// <summary>
        /// Saves one or multiple Accounts by provided Ids
        /// </summary>
        internal static void SaveAccounts(params ulong[] ids)
        {
            foreach (var id in ids)
            {
                Configuration.DataStorage.StoreObject(GetWasagotchiAccount(id), Path.Combine(Constants.WasagotchiAccountsFolder, $"{id}.json"), useIndentations: true);
            }
        }
    }
}
