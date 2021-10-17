﻿using BepInEx;
using FTKItemName;
using FTKModLib.Objects;
using FTKModLib.Utils;
using GridEditor;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public Dictionary<int, CustomItem> itemsDictionary = new();
        public List<CustomItem> itemsList = new();

        /// <summary>
        /// Add a custom item.
        /// </summary>
        /// <param name="customItem">The custom item to be added.</param>
        /// <param name="plugin">Allows FTKModLib to know which plugin called this method. Not required but recommended to make debugging easier.</param>
        /// <returns></returns>
        public static bool AddItem(CustomItem customItem, BaseUnityPlugin plugin=null) {
            if(plugin != null) customItem.PLUGIN_ORIGIN = plugin.Info.Metadata.GUID;
            Instance.itemsList.Add(customItem);
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
                // LOAD CUSTOM ITEMS
                int successfulLoads = 0;
                ItemManager itemManager = ItemManager.Instance;
                TableManager tableManager = TableManager.Instance;

                Debug.Log("Preparing to load custom items");
                itemManager.itemsDictionary.Clear();
                itemManager.enums.Clear();
                itemManager.itemsList.Sort(delegate (CustomItem x, CustomItem y) {
                    return x.ID.CompareTo(y.ID);
                });//will keep Enums in sync?
                List <CustomItem> brokenItems = new();
                foreach (CustomItem item in itemManager.itemsList) {
                    try {
                        item.ForceUpdatePrefab(); // this is necessary to apply Weapon fields
                        FTK_itembase itemDetails = item.itemDetails;
                        itemManager.enums.Add(itemDetails.m_ID,
                            // Anything over 100000? is a weapon, anything under is a regular item.
                            !item.IsWeapon ? 100000 - itemManager.itemsList.Count : 
                            itemManager.itemsList.Count + (int)Enum.GetValues(typeof(FTK_itembase.ID)).Cast<FTK_itembase.ID>().Max() - brokenItems.Count
                        );
                        itemManager.itemsDictionary.Add(itemManager.enums[itemDetails.m_ID], item);
                        GEDataArrayBase geDataArrayBase = tableManager.Get(!item.IsWeapon ? typeof(FTK_itemsDB) : typeof(FTK_weaponStats2DB));
                        geDataArrayBase.AddEntry(itemDetails.m_ID);
                        if (!item.IsWeapon) ((FTK_itemsDB)geDataArrayBase).m_Array[tableManager.Get<FTK_itemsDB>().m_Array.Length - 1] = (FTK_items)itemDetails;
                        else ((FTK_weaponStats2DB)geDataArrayBase).m_Array[tableManager.Get<FTK_weaponStats2DB>().m_Array.Length - 1] = item.weaponDetails;
                        geDataArrayBase.CheckAndMakeIndex();
                        successfulLoads++;
                        Debug.Log($"Loaded '{item.ID}' of type '{item.ObjectType}' from {item.PLUGIN_ORIGIN}");
                    }
                    catch (Exception e) {
                        Debug.LogError(e);
                        brokenItems.Add(item);
                        Debug.LogError($"Failed to load '{item.ID}' of type '{item.ObjectType}' from {item.PLUGIN_ORIGIN}");
                    }
                }
                foreach(CustomItem item in brokenItems) {
                    itemManager.itemsList.Remove(item);
                }
                Debug.Log($"Successfully loaded {successfulLoads} out of {itemManager.itemsList.Count} custom items");
            }
        }

        [HarmonyPatch(typeof(FTK_itembase), "GetLocalizedName")]
        class FTK_itembase_GetLocalizedName_Patch {
            static bool Prefix(ref string __result, FTK_itembase __instance) {
                if (ItemManager.Instance.enums.TryGetValue(__instance.m_ID, out int dictKey)) {
                    __result = ItemManager.Instance.itemsDictionary[dictKey].GetName();
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(FTKItem), "Get")]
        class FTKItem_Get_Patch {
            static bool Prefix(ref FTKItem __result, FTK_itembase.ID _id) {
                if (ItemManager.Instance.itemsDictionary.TryGetValue((int)_id, out CustomItem customItem)) {
                    FieldInfo private_gItemDictionary = typeof(FTKItem).GetField("gItemDictionary", BindingFlags.NonPublic | BindingFlags.Static);

                    if ((Dictionary<FTK_itembase.ID, FTKItem>)private_gItemDictionary.GetValue(private_gItemDictionary) == null) {
                        private_gItemDictionary.SetValue(private_gItemDictionary, new Dictionary<FTK_itembase.ID, FTKItem>());
                    }
                    FieldInfo protected_ItemID = customItem.GetType().GetField("m_ItemID", BindingFlags.NonPublic | BindingFlags.Instance);
                    protected_ItemID.SetValue(customItem, _id);
                    __result = customItem;
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
