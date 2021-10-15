using BepInEx;
using GridEditor;
using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace FTKModLib {
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    class Main : BaseUnityPlugin {
        internal static Main Instance;

        public class ItemManager {
            private static ItemManager _instance;
            public static ItemManager Instance {
                get {
                    if (_instance == null) _instance = new ItemManager();
                    return _instance;
                }
            }
            public Dictionary<string, int> enums = new();
            public List<FTK_items> items = new();
            public void AddItem(FTK_items item) {
                items.Add(item);
                //Anything over 100000? is a weapon, anything under is a regular item.
                if(!item.m_IsWeapon) enums.Add(item.m_ID, 100000 - items.Count);
                else enums.Add(item.m_ID, (int)Enum.GetValues(typeof(FTK_itembase.ID)).Cast<FTK_itembase.ID>().Max() + items.Count);
            }
        }

        public ItemManager itemManager = ItemManager.Instance;

        private void Awake() {
            Instance = this;
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            itemManager.AddItem(new FTK_items() {
                _shopStock = 99,
                m_TownMarket = true,
                m_ID = "CustomItem1",
            });

            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(TableManager), "Initialize")]
    class TableManager_Initialize_Patch {
        /// <summary>
        /// FTK_itemDB injection point
        /// </summary>
        static void Postfix() {
            JsonSerializerSettings settings = new JsonSerializerSettings() {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Error = (serializer, err) => err.ErrorContext.Handled = true,
                ContractResolver = new IgnorePropertiesResolver(new[] { "_icon", "_iconNonClickable", "_prefab", "m_Icon", "m_IconNonClickable", "m_Prefab" })
            };

            //CUSTOM ITEM TEST
            foreach (var item in Main.Instance.itemManager.items) {
                if (item == null) continue;
                TableManager.Instance.Get<FTK_itemsDB>().AddEntry(item.m_ID);
                TableManager.Instance.Get<FTK_itemsDB>().m_Array[TableManager.Instance.Get<FTK_itemsDB>().m_Array.Length - 1] = item;
                TableManager.Instance.Get<FTK_itemsDB>().CheckAndMakeIndex();

                string itemJson = JsonConvert.SerializeObject(TableManager.Instance.Get<FTK_itemsDB>().m_Array[TableManager.Instance.Get<FTK_itemsDB>().m_Array.Length - 1], Formatting.Indented, settings);
                Debug.Log($"{itemJson}");
            }
        }
    }

    [HarmonyPatch(typeof(FTK_itembase), "GetLocalizedName")]
    class itembaseGetLocalizedNameHook {
        static bool Prefix(ref string __result, FTK_itembase __instance) {
            if (Main.Instance.itemManager.enums.ContainsKey(__instance.m_ID)) {
                __result = __instance.m_ID; // just return the id name for now...
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(FTK_itemsDB), "GetIntFromID")]
    class itemsDBGetIntFromIDHook {
        static bool Prefix(ref int __result, FTK_itemsDB __instance, string _id) {
            //Attempts to return our enum and calls the original function if it errors.
            if (Main.Instance.itemManager.enums.ContainsKey(_id)) {
                __result = Main.Instance.itemManager.enums[_id];
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(FTK_weaponStats2DB), "GetIntFromID")]
    class weaponStats2DBGetIntFromIDHook {
        static bool Prefix(ref int __result, FTK_weaponStats2DB __instance, string _id) {
            //Attempts to return our enum and calls the original function if it errors.
            if (Main.Instance.itemManager.enums.ContainsKey(_id)) {
                __result = Main.Instance.itemManager.enums[_id];
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(FTK_itembase), "GetEnum")]
    class itembaseGetEnumHook {
        static bool Prefix(ref FTK_itembase.ID __result, string _id) {
            //Not 100% sure if this is required.
            if (Main.Instance.itemManager.enums.ContainsKey(_id)) {
                __result = (FTK_itembase.ID)Main.Instance.itemManager.enums[_id];
                return false;
            }
            return true;
        }
    }

    //Gives item to Hunter as starting weapon for an example.
    [HarmonyPatch(typeof(FTK_playerGameStartDB), "GetEntry")]
    class playerGameStartDBGetEntryHook {
        static bool Prefix(ref FTK_playerGameStart __result, FTK_playerGameStartDB __instance, FTK_playerGameStart.ID _enumID) {
            if (_enumID == FTK_playerGameStart.ID.hunter) {
                __result = __instance.GetEntryByInt((int)_enumID);
                __result.m_StartItems[0] = (FTK_itembase.ID)(Main.Instance.itemManager.enums["CustomItem1"]);
                Debug.Log("Hunter hook Activated " + ((FTK_itembase.ID)Main.Instance.itemManager.enums["CustomItem1"]).ToString());
                return false;
            }
            else {
                return true;
            }
        }
    }

    public class IgnorePropertiesResolver : DefaultContractResolver {
        private readonly HashSet<string> ignoreProps;
        public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore) {
            this.ignoreProps = new HashSet<string>(propNamesToIgnore);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (this.ignoreProps.Contains(property.PropertyName)) {
                property.ShouldSerialize = _ => false;
            }
            return property;
        }
    }
}
