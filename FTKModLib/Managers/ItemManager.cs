using FTKModLib.Utils;
using GridEditor;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FTKModLib.Managers {
    /// <summary>
    ///    Manager for handling items added to the game.
    /// </summary>
    public class ItemManager : IManager {
        private static ItemManager _instance;
        /// <summary>
        ///     The singleton instance of this manager.
        /// </summary>
        public static ItemManager Instance {
            get {
                if (_instance == null) _instance = new ItemManager();
                return _instance;
            }
        }

        public Dictionary<string, int> enums = new();
        public List<FTK_items> items = new();
        public bool AddItem(FTK_items item) {
            items.Add(item);
            string itemJson = JsonConvert.SerializeObject(item, Formatting.Indented, new JsonSerializerSettings() {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Error = (serializer, err) => err.ErrorContext.Handled = true,
                ContractResolver = new IgnorePropertiesResolver(new[] { "_icon", "_iconNonClickable", "_prefab", "m_Icon", "m_IconNonClickable", "m_Prefab" })
            });
            Debug.Log($"{itemJson}"); //displayed for debug purposes, remove later?
            return true;
        }

        public void Init() {
            Console.WriteLine("Initializing " + this.GetType().Name);
        }
    }

    /// <summary>
    ///    The patches necessary to make adding custom items possible.
    /// </summary>
    class HarmonyPatches {
        [HarmonyPatch(typeof(TableManager), "Initialize")]
        class TableManager_Initialize_Patch {
            /// <summary>
            /// FTK_itemDB injection point
            /// </summary>
            static void Postfix() {
                //LOAD CUSTOM ITEMS
                int successfulLoads = 0;
                ItemManager itemManager = ItemManager.Instance;
                TableManager tableManager = TableManager.Instance;

                Debug.Log("Preparing to load custom items");
                itemManager.items.Sort(); //will keep Enums in sync?
                foreach (var item in itemManager.items) {
                    if (item == null) continue;

                    if(!itemManager.enums.ContainsKey(item.m_ID)) {
                        //Anything over 100000? is a weapon, anything under is a regular item.
                        if (!item.m_IsWeapon) itemManager.enums.Add(item.m_ID, 100000 - itemManager.items.Count);
                        else itemManager.enums.Add(item.m_ID, (int)Enum.GetValues(typeof(FTK_itembase.ID)).Cast<FTK_itembase.ID>().Max() + itemManager.items.Count);
                    }

                    tableManager.Get<FTK_itemsDB>().AddEntry(item.m_ID);
                    tableManager.Get<FTK_itemsDB>().m_Array[TableManager.Instance.Get<FTK_itemsDB>().m_Array.Length - 1] = item;
                    tableManager.Get<FTK_itemsDB>().CheckAndMakeIndex();
                    successfulLoads++;
                }
                Debug.Log($"Successfully loaded {successfulLoads} custom items");
            }
        }

        [HarmonyPatch(typeof(FTK_itembase), "GetLocalizedName")]
        class FTK_itembase_GetLocalizedName_Patch {
            static bool Prefix(ref string __result, FTK_itembase __instance) {
                if (ItemManager.Instance.enums.ContainsKey(__instance.m_ID)) {
                    __result = __instance.m_ID; // just return the id name for now...
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(FTK_itemsDB), "GetIntFromID")]
        class FTK_itemsDB_GetIntFromID_Patch {
            static bool Prefix(ref int __result, FTK_itemsDB __instance, string _id) {
                //Attempts to return our enum and calls the original function if it errors.
                if (ItemManager.Instance.enums.ContainsKey(_id)) {
                    __result = ItemManager.Instance.enums[_id];
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(FTK_weaponStats2DB), "GetIntFromID")]
        class FTK_weaponStats2DB_GetIntFromID_Patch {
            static bool Prefix(ref int __result, FTK_weaponStats2DB __instance, string _id) {
                //Attempts to return our enum and calls the original function if it errors.
                if (ItemManager.Instance.enums.ContainsKey(_id)) {
                    __result = ItemManager.Instance.enums[_id];
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(FTK_itembase), "GetEnum")]
        class FTK_itembase_GetEnum_Patch {
            static bool Prefix(ref FTK_itembase.ID __result, string _id) {
                //Not 100% sure if this is required.
                if (ItemManager.Instance.enums.ContainsKey(_id)) {
                    __result = (FTK_itembase.ID)ItemManager.Instance.enums[_id];
                    return false;
                }
                return true;
            }
        }
    }
}
