using BepInEx;
using FTKItemName;
using FTKModLib.Objects;
using GridEditor;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Logger = FTKModLib.Utils.Logger;

namespace FTKModLib.Managers {
    /// <summary>
    ///    Manager for handling items added to the game.
    /// </summary>
    public class ItemManager : BaseManager<ItemManager> {
        public Dictionary<string, int> enums = new();
        public Dictionary<int, CustomItem> itemsDictionary = new();
        public List<CustomItem> itemsList = new();

        /// <summary>
        /// Add a custom item.
        /// </summary>
        /// <param name="customItem">The custom item to be added.</param>
        /// <param name="plugin">Allows FTKModLib to know which plugin called this method. Not required but recommended to make debugging easier.</param>
        /// <returns></returns>
        public static bool AddItem(CustomItem customItem, BaseUnityPlugin plugin = null) {
            if (plugin != null) customItem.PLUGIN_ORIGIN = plugin.Info.Metadata.GUID;
            Instance.itemsList.Add(customItem);
            return true;
        }
    }

    /// <summary>
    ///    <para>The patches necessary to make adding custom items possible.</para>
    ///    <para>Many of them are used to fix issues calling an invalid 'FTK_itembase.ID' 
    ///    by substituting with 'ItemManager.enums' dictionary value.</para>
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

                Logger.LogInfo("Preparing to load custom items");
                itemManager.itemsDictionary.Clear();
                itemManager.enums.Clear();
                itemManager.itemsList.Sort(delegate (CustomItem x, CustomItem y) {
                    return x.ID.CompareTo(y.ID);
                });//will keep Enums in sync?
                List<CustomItem> brokenItems = new();
                foreach (CustomItem item in itemManager.itemsList) {
                    try {
                        item.ForceUpdatePrefab(); // this is necessary to apply Weapon fields
                        FTK_itembase itemDetails = item.itemDetails;
                        itemManager.enums.Add(itemDetails.m_ID,
                            // Anything over 100000? is a weapon, anything under is a regular item.
                            !item.IsWeapon ? 100000 - (successfulLoads+1) :
                            (int)Enum.GetValues(typeof(FTK_itembase.ID)).Cast<FTK_itembase.ID>().Max() + (successfulLoads + 1)
                        );
                        itemManager.itemsDictionary.Add(itemManager.enums[itemDetails.m_ID], item);
                        GEDataArrayBase geDataArrayBase = tableManager.Get(!item.IsWeapon ? typeof(FTK_itemsDB) : typeof(FTK_weaponStats2DB));
                        if (!geDataArrayBase.IsContainID(itemDetails.m_ID)) {
                            geDataArrayBase.AddEntry(itemDetails.m_ID);
                            if (!item.IsWeapon) ((FTK_itemsDB)geDataArrayBase).m_Array[tableManager.Get<FTK_itemsDB>().m_Array.Length - 1] = (FTK_items)itemDetails;
                            else ((FTK_weaponStats2DB)geDataArrayBase).m_Array[tableManager.Get<FTK_weaponStats2DB>().m_Array.Length - 1] = item.weaponDetails;
                            geDataArrayBase.CheckAndMakeIndex();
                        }
                        successfulLoads++;
                        Logger.LogInfo($"Loaded '{item.ID}' of type '{item.ObjectType}' from {item.PLUGIN_ORIGIN}");
                    }
                    catch (Exception e) {
                        Logger.LogError(e);
                        brokenItems.Add(item);
                        Logger.LogError($"Failed to load '{item.ID}' of type '{item.ObjectType}' from {item.PLUGIN_ORIGIN}");
                    }
                }
                foreach (CustomItem item in brokenItems) {
                    itemManager.itemsList.Remove(item);
                }
                Logger.LogInfo($"Successfully loaded {successfulLoads} out of {itemManager.itemsList.Count} custom items");
            }
        }

        [HarmonyPatch(typeof(FTK_itembase), "GetItemBase")]
        class FTK_itembase_GetItemBase_Patch {
            static bool Prefix(ref FTK_itembase __result, FTK_itembase.ID _id) {
                if (ItemManager.Instance.itemsDictionary.TryGetValue((int)_id, out CustomItem customItem)) {
                    if (!customItem.IsWeapon)
                        __result = customItem.itemDetails;
                    else
                        __result = customItem.weaponDetails;
                    return false;
                }
                return true;
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

        [HarmonyPatch(typeof(FTK_weaponStats2DB), "GetEntry")]
        class FTK_weaponStats2DB_GetEntry_Patch {
            static bool Prefix(ref FTK_weaponStats2 __result, FTK_itembase.ID _enumID) {
                if (ItemManager.Instance.itemsDictionary.TryGetValue((int)_enumID, out CustomItem customItem)) {
                    __result = customItem.weaponDetails;
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


    /// <summary>
    /// Debug patches that may assist in debugging FTKModLib
    /// </summary>
    class HarmonyDebugPatches {
        //[HarmonyPatch(typeof(FTKNetworkObject), "StateDataDeserialize")]
        class FTKNetworkObject_StateDataDeserialize_Patch {
            static void Prefix(string _s, bool _callDone = true) {
                Logger.LogError("DESERIALIZED:");
                Logger.LogError(_s);
            }
        }

        //[HarmonyPatch(typeof(FTKNetworkObject), "StateDataSerialize")]
        class FTKNetworkObject_StateDataSerialize_Patch {
            static void Postfix(ref string __result, bool _prep = true) {
                Logger.LogError("SERIALIZED:");
                Logger.LogError(__result);
            }
        }
    }
}
