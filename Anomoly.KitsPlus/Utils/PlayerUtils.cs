using Anomoly.KitsPlus.Data;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Utils
{
    public static class PlayerInventoryUtils
    {
        public static List<KitItem> GetKitItemsFromInventory(this UnturnedPlayer player)
        {
            var clothing = player.Player.clothing;

            var items = new List<KitItem>();

            if (clothing.backpack != 0)
                items.Add(new KitItem() { Id = clothing.backpack, Amount = 1 });
            if (clothing.vest != 0)
                items.Add(new KitItem() { Id = clothing.vest, Amount = 1 });
            if (clothing.shirt != 0)
                items.Add(new KitItem() { Id = clothing.shirt, Amount = 1 });
            if (clothing.pants != 0)
                items.Add(new KitItem() { Id = clothing.pants, Amount = 1 });
            if (clothing.mask != 0)
                items.Add(new KitItem() { Id = clothing.mask, Amount = 1 });
            if (clothing.hat != 0)
                items.Add(new KitItem() { Id = clothing.hat, Amount = 1 });
            if (clothing.glasses != 0)
                items.Add(new KitItem() { Id = clothing.glasses, Amount = 1 });

            for (byte page = 0; page < PlayerInventory.PAGES - 2; page++)
            {
                for (byte index = 0; index < player.Inventory.getItemCount(page); index++)
                {
                    var jar = player.Inventory.getItem(page, index);
                    if (jar == null)
                        continue;

                    var item = items.FirstOrDefault(x => x.Id == jar.item.id);

                    if (item == null)
                    {
                        items.Add(new KitItem()
                        {
                            Id = jar.item.id,
                            Amount = 1,
                        });

                        continue;
                    }

                    item.Amount++;
                }
            }

            return items;
        }

        public static bool GiveKit(this IRocketPlayer player, Kit kit, bool setCooldown = true)
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

            if (setCooldown)
            {
                var cMgr = KitsPlusPlugin.Instance.CooldownManager;

                cMgr.SetKitCooldown(player, kit.Name);
                cMgr.SetGlobalCooldown(player.Id);
            }

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

            return true;
        }
    }
}
