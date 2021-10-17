using BepInEx;
using FTKModLib.Managers;
using FTKModLib.Objects;
using GridEditor;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace FTKModLib.Example {
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(FTKModLib.PLUGIN_GUID)]
    public class ExampleMod : BaseUnityPlugin {
        public const string PLUGIN_GUID = "com.FTKModLib.Example";
        public const string PLUGIN_NAME = "FTKModLib Example Mod";
        public const string PLUGIN_VERSION = "1.0.0";

        private void Awake() {
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");

            ItemManager.Instance.AddItem(new CustomItem() {
                m_ID = "CustomItem1",
                name = new CustomLocalizedString("Example Item"),
                description = new CustomLocalizedString("This is the super powerful ultra example item!\n\nIt does nothing."),
                _shopStock = 69,
                m_TownMarket = true,
            });

            Harmony harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();
        }
    }

    //Gives item to Hunter as an additional starting item for an example.
    class HarmonyPatches {
        [HarmonyPatch(typeof(TableManager), "Initialize")]
        class TableManager_Initialize_Patch {
            static void Postfix() {
                ItemManager itemManager = ItemManager.Instance;
                TableManager tableManager = TableManager.Instance;

                FTK_itembase.ID[] hunterOriginalStartItems = tableManager.Get<FTK_playerGameStartDB>().m_Array[(int)FTK_playerGameStart.ID.hunter].m_StartItems;
                tableManager.Get<FTK_playerGameStartDB>().m_Array[(int)FTK_playerGameStart.ID.hunter].m_StartItems = (
                    hunterOriginalStartItems.AddItem((FTK_itembase.ID)itemManager.enums["CustomItem1"]).ToArray()
                );
                tableManager.Get<FTK_playerGameStartDB>().CheckAndMakeIndex();

                Debug.Log($"Added CustomItem1 to hunter's m_StartItems");
            }
        }
    }
}
