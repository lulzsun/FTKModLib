using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using FTKModLib.Managers;

namespace FTKModLib {
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class FTKModLib : BaseUnityPlugin {
        public const string PLUGIN_GUID = "com.FTKModLib.FTKModLib";
        public const string PLUGIN_NAME = "FTKModLib";
        public const string PLUGIN_VERSION = "1.0.0";

        internal static FTKModLib Instance;

        private List<IManager> Managers;

        private void Awake() {
            Instance = this;
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");

            Managers = new List<IManager>() {
                ItemManager.Instance
            };
            foreach (IManager manager in Managers) {
                manager.Init();
            }

            // This is required for proper serialization for CustomObjects
            // which use an Enum as an identifier.
            // Otherwise, saving and loading games would not work if this
            // was false.
            FullSerializer.fsConfig.SerializeEnumsAsInteger = true;

            Harmony harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();
        }
    }
}