using BepInEx;
using HarmonyLib;
using FTKModLib.Managers;
using System.Collections.Generic;

namespace FTKModLib {
    [BepInPlugin(PLUGIN_GUID, "FTKModLib", "1.0.0")]
    [BepInProcess("FTK.exe")]
    public class FTKModLib : BaseUnityPlugin {
        public const string PLUGIN_GUID = "com.FTKModLib.FTKModLib";

        internal static FTKModLib Instance;
        private List<object> _managers;

        private void Awake() {
            Instance = this;

            Utils.Logger.Init();

            // This is required for proper serialization for CustomObjects
            // which use an Enum as an identifier.
            // Otherwise, saving and loading games would not work if this
            // was false.
            FullSerializer.fsConfig.SerializeEnumsAsInteger = true;

            _managers = new List<object>() {
                AssetManager.Instance,
                Managers.ProficiencyManager.Instance,
                ItemManager.Instance,
                ClassManager.Instance,
            };

            Harmony harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();

            Utils.Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
        }
    }
}