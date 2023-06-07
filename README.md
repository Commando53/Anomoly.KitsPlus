# Anomoly.KitsPlus

A simple kits plugin for RocketMod with extra features including storing kits data in rather JSON or MYSQL

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
-   `/createkit <name> [<cooldown>] [<vehicle>] [<xp>]` - Create a kit with the specified name and optional cooldown, vehicle, xp, and items from your inventory
    -   Aliases: None
    -   Permission: `createkit`
-   `/deletekit <name>` - Delete a kit by its name
    -   Aliases: None
    -   Permission: `deletekit`
-   `/giftkit <player> <kit name>` - Gift a kit to a player while taking the cooldown yourself
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
</Translations>
```

## Developers

### API

KitsPlus exposes an API for other plugins to use. The API is exposed through the `KitsPlus.Instance` property.

```csharp

KitsPlusPlugin.Instance.KitDb // The database used for storing kits data
KitsPlusPlugin.Instance.Configuration // The configuration used for the plugin
```

```csharp
public interface IKitDatabase: IDisposable
{
    string Name { get; }

    List<Kit> GetKits();
    List<Kit> GetKits(IRocketPlayer player);

    Kit GetKitByName(string name);
    Kit GetKitByName(IRocketPlayer player, string name);

    bool CreateKit(Kit kit);
    int DeleteKit(string name);
}
```

### Models

Kit

-   Name: The name of the kit
-   XP: The amount of XP to give the player
-   Vehicle: The vehicle to give the player
-   Cooldown: The cooldown of the kit in seconds
-   Items: The items to give the player

```csharp
[Serializable]
public class Kit
{
    public string Name { get; set; }

    public uint? XP { get; set; }

    public ushort? Vehicle { get; set; }

    public int Cooldown { get; set; }

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
