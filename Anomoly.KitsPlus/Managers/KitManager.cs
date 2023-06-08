using Anomoly.KitsPlus.Data;
using Anomoly.KitsPlus.Databases;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Managers
{
    public class KitManager: IDisposable
    {
        private Kit[] _defaultKits = new Kit[]
        {
            new Kit()
            {
                Name ="Survival",
                XP = null,
                Vehicle = null,
                Cooldown = 300,
                MaxUsage = 0,
                Items = new List<KitItem>()
                {
                    new KitItem()
                    {
                        Id = 251,
                        Amount = 1,
                    },
                    new KitItem()
                    {
                        Id = 81,
                        Amount = 2,
                    },
                    new KitItem()
                    {
                        Id = 16,
                        Amount = 1,
                    }
                }
            }
        };

        private IKitRepository _repo;

        public delegate void KitCreated(Kit createdKit);
        public event KitCreated OnKitCreated;

        public delegate void KitDeleted(string name);
        public event KitDeleted OnKitDeleted;

        public delegate void KitRedeemed(IRocketPlayer player, Kit redeemedKit);
        public event KitRedeemed OnKitRedeemed;

        public delegate void KitGifted(IRocketPlayer gifter, IRocketPlayer giftee, Kit giftedKit);
        public event KitGifted OnKitGifted;

        public KitManager()
        {
            switch (KitsPlusPlugin.Instance.Configuration.Instance.DatabaseType)
            {
                case "json":
                    _repo = new JsonRepository();
                    break;
                case "mysql":
                    _repo = new MySQLRepository();
                    break;
                default:
                    Logger.LogWarning("Unknown database-type. Defaulting to JSON.");
                    _repo = new JsonRepository();
                    break;
            }

            _repo.Initialize(_defaultKits);
        }

        public Kit[] GetKits() => _repo.GetKits().ToArray();
        public Kit GetKit(string name) => GetKits().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        public bool CreateKit(Kit kit)
        {
            var created = _repo.CreateKit(kit);
            if (created)
                OnKitCreated?.Invoke(kit);
            return created;
        }

        public bool DeleteKit(string name)
        {
            var deleted = _repo.DeleteKit(name) > 0;
            if (deleted)
                OnKitDeleted?.Invoke(name);
            return deleted;
        }

        public Kit[] GetPlayerKits(IRocketPlayer player)
            => _repo.GetKits().Where(x => player.HasPermission($"kit.{x.Name}")).ToArray();
        public Kit GetPlayerKit(IRocketPlayer player, string kitName)
            => GetPlayerKits(player).FirstOrDefault(x => x.Name.ToLower() == kitName.ToLower());

        public void GiveKit(IRocketPlayer player, Kit kit, IRocketPlayer gifter = null)
        {
            var sortedItems = kit.Items;

            var itemAssets = sortedItems.OrderByDescending(s =>
            {
                var itemId = s.Id;

                var asset = Assets.find(EAssetType.ITEM, itemId);

                if (asset is ItemClothingAsset)
                    return 1;
                return 0;
            }).ToList();

            var uPlayer = (UnturnedPlayer)player;

            foreach (var item in itemAssets)
            {
                if (!uPlayer.GiveItem(item.Id, item.Amount))
                {
                    Logger.LogError($"Failed giving {player.DisplayName} ({player.Id}) item {item.Id}");
                }
            }

            if (kit.Vehicle.HasValue && kit.Vehicle != 0)
            {
                if (!uPlayer.GiveVehicle((ushort)kit.Vehicle))
                {
                    Logger.LogError($"Failed giving {player.DisplayName} ({player.Id}) vehicle {kit.Vehicle}!");
                }
            }

            if (kit.XP.HasValue && kit.XP != 0)
            {
                uPlayer.Experience += (uint)kit.XP;
            }

            if (gifter == null)
                OnKitRedeemed?.Invoke(player, kit);
            else
                OnKitGifted?.Invoke(gifter, player, kit);
        }

        public void Reset()
        {
            _repo.Reset();
        }

        public void Dispose()
        {
            _repo.Dispose();
            _repo = null;
        }
    }
}
