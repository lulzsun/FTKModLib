using BepInEx;
using FTKModLib.Managers;
using FTKModLib.Objects;
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
        public static BaseUnityPlugin Instance;

        private void Awake() {
            Instance = this;
            Utils.Logger.LogInfo($"Plugin {Info.Metadata.GUID} is loaded!");
            assetBundle = AssetManager.LoadAssetBundleFromResources("customitemsbundle", Assembly.GetExecutingAssembly());

            Harmony harmony = new Harmony(Info.Metadata.GUID);
            harmony.PatchAll();
        }

        class HarmonyPatches {
            /// <summary>
            /// Most calls to managers will most likely require to be called after TableManager.Initialize.
            /// So just make all your changes in this postfix patch unless you know what you're doing.
            /// </summary>
            [HarmonyPatch(typeof(TableManager), "Initialize")]
            class TableManager_Initialize_Patch {
                static void Postfix() {
                    int customHerb = ItemManager.AddItem(new ExampleHerb(), Instance);
                    int customBlade = ItemManager.AddItem(new ExampleBlade(), Instance);
                    int customGun = ItemManager.AddItem(new ExampleGun(), Instance);

                    ClassManager.AddClass(new ExampleClass() { 
                        StartWeapon = (FTK_itembase.ID)customGun 
                    }, Instance);
                    ClassManager.ModifyClass(
                        FTK_playerGameStart.ID.blacksmith,
                        new CustomClass(FTK_playerGameStart.ID.blacksmith) {
                            StartWeapon = (FTK_itembase.ID)customBlade
                        }
                        .AddToStartItems(new FTK_itembase.ID[] {
                            (FTK_itembase.ID)customHerb,
                            (FTK_itembase.ID)customGun
                        })
                    );
                    Utils.Logger.LogInfo($"Modified blacksmith class to give our example items");
                }
            }
        }
    }
}
