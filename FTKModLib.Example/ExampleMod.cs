using BepInEx;
using FTKModLib.Managers;
using GridEditor;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace FTKModLib.Example {
    [BepInPlugin("com.FTKModLib.Example", "FTKModLib Example Mod", "1.0.0")]
    [BepInDependency(FTKModLib.PLUGIN_GUID)]
    [BepInProcess("FTK.exe")]
    public class ExampleMod : BaseUnityPlugin {
        public static AssetBundle assetBundle;

        private void Awake() {
            Utils.Logger.LogInfo($"Plugin {Info.Metadata.GUID} is loaded!");

            assetBundle = AssetManager.LoadAssetBundleFromResources("customitemsbundle", Assembly.GetExecutingAssembly());

            foreach(var animator in AssetManager.GetAnimationControllers<Weapon>()) {
                if(animator.name.StartsWith("player_")) Utils.Logger.LogInfo(animator.name);
            }

            ItemManager.AddItem(new ExampleHerb(), this);
            ItemManager.AddItem(new ExampleBlade(), this);
            ItemManager.AddItem(new ExampleGun(), this);

            Harmony harmony = new Harmony(Info.Metadata.GUID);
            harmony.PatchAll();
        }
    }

    //Gives item to Blacksmith as an additional starting item for an example.
    class HarmonyPatches {
        [HarmonyPatch(typeof(TableManager), "Initialize")]
        class TableManager_Initialize_Patch {
            static void Postfix() {
                ItemManager itemManager = ItemManager.Instance;
                FTK_playerGameStartDB playerGameStartDB = TableManager.Instance.Get<FTK_playerGameStartDB>();

                FTK_itembase.ID[] blacksmithOriginalStartItems = playerGameStartDB.m_Array[(int)FTK_playerGameStart.ID.blacksmith].m_StartItems;
                blacksmithOriginalStartItems = blacksmithOriginalStartItems.AddItem((FTK_itembase.ID)itemManager.enums["CustomHerb1"]).ToArray();
                blacksmithOriginalStartItems = blacksmithOriginalStartItems.AddItem((FTK_itembase.ID)itemManager.enums["CustomGun1"]).ToArray();
                playerGameStartDB.m_Array[(int)FTK_playerGameStart.ID.blacksmith].m_StartItems = blacksmithOriginalStartItems;
                playerGameStartDB.m_Array[(int)FTK_playerGameStart.ID.blacksmith].m_StartWeapon = (FTK_itembase.ID)itemManager.enums["CustomBlade1"];

                Utils.Logger.LogInfo($"Added CustomItem1 to hunter's m_StartItems");
            }
        }
    }
}
