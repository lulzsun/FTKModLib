using BepInEx;
using FTKModLib.Managers;
using FTKModLib.Objects;
using GridEditor;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace FTKModLib.Example {
    [BepInPlugin("com.FTKModLib.Example", "FTKModLib Example Mod", "1.0.0")]
    [BepInDependency(FTKModLib.PLUGIN_GUID)]
    public class ExampleMod : BaseUnityPlugin {
        private void Awake() {
            Logger.LogInfo($"Plugin {Info.Metadata.GUID} is loaded!");

            ItemManager.AddItem(new ExampleItem(), this);
            ItemManager.AddItem(new ExampleWeapon(), this);

            Harmony harmony = new Harmony(Info.Metadata.GUID);
            harmony.PatchAll();
        }
    }

    public class ExampleItem : CustomItem {
        public ExampleItem() {
            ID = "CustomItem1";
            Name = new CustomLocalizedString("Example Item");
            Description = new CustomLocalizedString("This is the super powerful ultra example item!\n\nIt does nothing.");
            ShopStock = 69;
            TownMarket = true;
            ItemRarity = FTK_itemRarityLevel.ID.rare;
        }
    }

    public class ExampleWeapon : CustomItem {
        public ExampleWeapon() {
            ID = "CustomWeapon1";
            ObjectType = FTK_itembase.ObjectType.weapon; // This is required for the item to be registered as a weapon
            Name = new CustomLocalizedString("Example Weapon");
            ShopStock = 69;
            TownMarket = true;
            ItemRarity = FTK_itemRarityLevel.ID.rare;
            Slots = 1;
            SkillType = FTK_weaponStats2.SkillType.toughness;
            MaxDmg = 69420;
        }
    }

    //Gives item to Blacksmith as an additional starting item for an example.
    class HarmonyPatches {
        [HarmonyPatch(typeof(TableManager), "Initialize")]
        class TableManager_Initialize_Patch {
            static void Postfix() {
                ItemManager itemManager = ItemManager.Instance;
                FTK_playerGameStartDB playerGameStartDB = TableManager.Instance.Get<FTK_playerGameStartDB>();

                playerGameStartDB.m_Array[(int)FTK_playerGameStart.ID.blacksmith].m_StartWeapon = (FTK_itembase.ID)itemManager.enums["CustomWeapon1"];
                FTK_itembase.ID[] blacksmithOriginalStartItems = playerGameStartDB.m_Array[(int)FTK_playerGameStart.ID.blacksmith].m_StartItems;
                playerGameStartDB.m_Array[(int)FTK_playerGameStart.ID.blacksmith].m_StartItems = (
                    blacksmithOriginalStartItems.AddItem((FTK_itembase.ID)itemManager.enums["CustomItem1"]).ToArray()
                );
                playerGameStartDB.CheckAndMakeIndex();

                Debug.Log($"Added CustomItem1 to hunter's m_StartItems");
            }
        }
    }
}
