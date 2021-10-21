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

            foreach (var animator in AssetManager.GetAnimationControllers<Weapon>()) {
                if (animator.name.StartsWith("player_")) Utils.Logger.LogInfo(animator.name);
            }

            ItemManager.AddItem(new ExampleHerb(), Instance);
            ItemManager.AddItem(new ExampleBlade(), Instance);
            ItemManager.AddItem(new ExampleGun(), Instance);

            Harmony harmony = new Harmony(Info.Metadata.GUID);
            harmony.PatchAll();
        }

        class HarmonyPatches {
            [HarmonyPatch(typeof(TableManager), "Initialize")]
            class TableManager_Initialize_Patch {
                static void Postfix() {
                    ClassManager.ModifyClass(
                        FTK_playerGameStart.ID.blacksmith,
                        new CustomClass(FTK_playerGameStart.ID.blacksmith) {
                            StartWeapon = (FTK_itembase.ID)ItemManager.Instance.enums["CustomBlade1"]
                        }
                        .AddToStartItems(new FTK_itembase.ID[] {
                            (FTK_itembase.ID)ItemManager.Instance.enums["CustomHerb1"],
                            (FTK_itembase.ID)ItemManager.Instance.enums["CustomGun1"]
                        })
                    );
                    Utils.Logger.LogInfo($"Modified blacksmith class to give our example items");
                }
            }
        }
    }
}
