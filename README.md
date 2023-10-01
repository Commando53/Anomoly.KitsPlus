# Anomoly.KitsPlus but i changed everything XP related to uconomy balance (hey anomoly, it would be really cool if you made an official update for uconomy support thanks)

A simple kits plugin for RocketMod with extra features including storing kits data in rather JSON or MYSQL
this fork will change kits giving xp to kits costing uconomy balance lol.

update checker was also removed

i dont really know exactly how but i would guess migrating from the original kits plugin would make any kits that gives you balance now costs you uconomy balance to use
i also dont know if gifting kits would work. if you are gonna use this dont give players the permission to gift kits.
i see this fork more of a uessentials kits to anomoly.kitsplus but you have to migrate manually cause im lazy

## Dependencies

-   [Optional] MySQL.Data v8.0.33: Required for MySQL database support
-   [Optional] Kits Plugin (RocketMod): Required to migrate kits from the old Kits plugin

## Commands

-   `/kits` - Show all available kits
    -   Aliases: None
    -   Permission: `kits`
-   `/kit <kit name>` - Redeem a kit by its name
    -   Aliases: None
    -   Permission: `kit`, `kit.<kit name>`
-   `/createkit <name> [<cooldown>] [<vehicle>] [<balance>]` - Create a kit with the specified name and optional cooldown, vehicle, Uconomy Balance Cost, and items from your inventory
    -   Aliases: None
    -   Permission: `createkit`
-   `/deletekit <name>` - Delete a kit by its name
    -   Aliases: None
    -   Permission: `deletekit`
-   `/giftkit <player> <kit name>` - Gift a kit to a player while taking the cooldown yourself (I dont think this works properly if you add costs to kits)
    -   Aliases: None
    -   Permission: `giftkit`
-   `/migratekits` - Migrate kits from the old Kits plugin
    -   Aliases: None
    -   Permission: `migratekits`

## Configuration

-   `DatabaseType` - The type of database to use for storing kits data. Valid values are `json` and `mysql`
-   `MySQLConnectionString` - The connection string to use for connecting to the MySQL database
-   `MySQLTablePrefix` - The prefix to use for the MySQL database tables
-   `KitUsagesEnabled` - Whether or not to track kit usages
-   `DisplayKitUsagesOnList` - Whether or not to display kit usages on the `/kits` command
-   `GlobalCooldown` - The global cooldown for all kits in seconds

```xml
<?xml version="1.0" encoding="utf-8"?>
<KitsPlusConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <DatabaseType>json</DatabaseType>
  <MySQLConnectionString>Server=127.0.0.1;Port=3306;Database=unturned;Uid=root;Pwd=pwd;</MySQLConnectionString>
  <MySQLTablePrefix>myserver</MySQLTablePrefix>
  <KitUsagesEnabled>true</KitUsagesEnabled>
  <DisplayKitUsagesOnList>true</DisplayKitUsagesOnList>
  <GlobalCooldown>10</GlobalCooldown>
</KitsPlusConfiguration>
```

## Translations

-   `command_kit_invalid` - The message sent to the player when they do not specify a kit name
-   `command_kit_not_found` - The message sent to the player when the kit they specified does not exist
-   `command_kit_global_cooldown` - The message sent to the player when they are on a global cooldown
-   `command_kit_cooldown` - The message sent to the player when they are on a kit cooldown
-   `command_kit_redeemed` - The message sent to the player when they successfully redeem a kit
-   `command_kit_max_usage` - The message sent to the player when they have used all of their kit usages
-   `command_kit_usage_left` - The message sent to the player when they have kit usages left

-   `command_kit_paid` - The message sent to the player when they successfully paid the cost of a kit
-   `command_kit_not_enough_balance` - The message sent to the player when they dont have enough balance for the kit

-   `command_kits_list` - The message sent to the player when they list all available kits

-   `command_createkit_invalid` - The message sent to the player when they do not specify a kit name
-   `command_createkit_created` - The message sent to the player when they successfully create a kit
-   `command_createkit_existing_name` - The message sent to the player when they try to create a kit with an existing name
-   `command_createkit_failed` - The message sent to the player when they fail to create a kit

-   `command_giftkit_invalid` - The message sent to the player when they do not specify a kit name
-   `command_giftkit_no_player` - The message sent to the player when the player they specified does not exist
-   `command_giftkit_no_kit` - The message sent to the player when the kit they specified does not exist
-   `command_giftkit_success` - The message sent to the player when they successfully gift a kit
-   `command_giftkit_failed` - The message sent to the player when they fail to gift a kit
-   `command_giftkit_gifted` - The message sent to the player when they are gifted a kit
-   `command_giftkit_self` - The message sent to the player when they try to gift themselves a kit

-   `command_deletekit_invalid` - The message sent to the player when they do not specify a kit name
-   `command_deletekit_deleted` - The message sent to the player when they successfully delete a kit

-   `command_migratekits_migrated` - The message sent to the player when they successfully migrate kits
-   `command_migratekits_warning` - The message sent to the player when they need to remove the old Kits plugin
-   `command_migratekits_no_plugin` - The message sent to the player when they fail to find the old Kits plugin
-   `command_resetkits_invalid` - The message sent to the player when the input is invalid
-   `command_resetkits_all` - The message sent to the player when all managers have been reset
-   `command_resetkits_cooldowns` - The message sent to the player when cooldowns have been reset
-   `command_resetkits_usages` - The message sent to the player when usages have been reset
-   `command_resetkits_usages_disabled` The message sent when reseting usages fails.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="command_kit_invalid" Value="Please do /kit {0}!" />
  <Translation Id="command_kit_not_found" Value="Kit not found! View available kits using /kits" />
  <Translation Id="command_kit_global_cooldown" Value="Please wait {0} before redeeming another kit." />
  <Translation Id="command_kit_cooldown" Value="Please wait {0} before redeeming the '{1}' kit again." />
  <Translation Id="command_kit_redeemed" Value="You have successfully redeemed the '{0}' kit!" />
  <Translation Id="command_kit_max_usage" Value="You have used {0}/{0} uses of the '{1}'!" />
  <Translation Id="command_kit_usage_left" Value="You have {0} uses left!" />

  <Translation Id="command_kits_list" Value="Available kits: {0}" />

  <Translation Id="command_createkit_invalid" Value="Please do /createkit {0}!" />
  <Translation Id="command_createkit_existing_name" Value="A kit already exists with the name of '{0}'!" />
  <Translation Id="command_createkit_created" Value="Successfully created the '{0}' kit!" />
  <Translation Id="command_createkit_failed" Value="Failed to create '{0}' kit." />

  <Translation Id="command_giftkit_invalid" Value="Please do /givekit {0}!" />
  <Translation Id="command_giftkit_no_player" Value="No player could be found by the name of '{0}'." />
  <Translation Id="command_giftkit_no_kit" Value="No kit could be found by the name of '{0}'." />
  <Translation Id="command_giftkit_success" Value="Successfully given your '{0}' kit to {1}" />
  <Translation Id="command_giftkit_failed" Value="Failed to give {0} the '{1}' kit!" />
  <Translation Id="command_giftkit_gifted" Value="{0} has gifted you their '{1}' kit!" />
  <Translation Id="command_giftkit_self" Value="You cannot gift yourself a kit!" />

  <Translation Id="command_deletekit_invalid" Value="Please do /deletekit {0}!" />
  <Translation Id="command_deletekit_deleted" Value="Deleted {0} kit(s) with the name of '{1}'." />

  <Translation Id="command_migratekits_migrated" Value="Migrated {0} kits succesfully with {1} failures. " />
  <Translation Id="command_migratekits_warning" Value="Please shutdown the server and remove the old Kits plugin. Restart only after removing!" />
  <Translation Id="command_migratekits_no_plugin" Value="Failed to find Kits plugin. Please make sure to restart the server with the plugin installed." />
    
  <Translation Id="command_resetkits_invalid" Value="Please do /resetkits {0}!" />
  <Translation Id="command_resetkits_all" Value="Reset kits data, cooldowns, and usages!" />
  <Translation Id="command_resetkits_cooldowns" Value="Reset kit &amp; global cooldowns!" />
  <Translation Id="command_resetkits_usages" Value="Reset kit usages!" />
  <Translation Id="command_resetkits_usages_disabled" Value="Failed to reset kit usages as Kit Usages are disabled!" />
</Translations>
```

## Developers

### API

KitsPlus exposes an API for other plugins to use. The API is exposed through the `KitsPlus.Instance` property.

```csharp
KitsPlusPlugin.Instance.KitManager // The manager of Kits & their items
KitsPlusPlugin.Instance.UsageManager // The manager to keep track of kit usages
KitsPlusPlugin.Instance.CooldownManager //The manager to keep track of global & individual kit cooldowns.
KitsPlusPlugin.Instance.Configuration // The configuration used for the plugin
```

### Models

Kit

-   Name: The name of the kit
-   Balance: The amount of Balance the kit would cost
-   Vehicle: The vehicle to give the player
-   Cooldown: The cooldown of the kit in seconds
-   MaxUsage: The max amount of uses for the kit
-   Items: The items to give the player

```csharp
[Serializable]
public class Kit
{
    public string Name { get; set; }

    public uint? Balance { get; set; }

    public ushort? Vehicle { get; set; }

    public int Cooldown { get; set; }
    
    public int MaxUsage { get; set; } = 0;

    public List<KitItem> Items { get; set; }
}
```

Kit Item

-   Id: The item id
-   Amount: The amount of the item

```csharp
[Serializable]
public class KitItem
{
    public ushort Id { get; set; }

    public byte Amount { get; set; }
}
```
