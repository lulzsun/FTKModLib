using BepInEx;
using FTKModLib.Objects;
using GridEditor;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Logger = FTKModLib.Utils.Logger;

/// <summary>
///    Manager for handling player classes in the game.
/// </summary>
namespace FTKModLib.Managers {
    public class ClassManager : BaseManager<ClassManager> {
        public Dictionary<string, int> enums = new();
        public Dictionary<int, CustomClass> classesDictionary = new();
        public List<CustomClass> classesList = new();

        private static int successfulLoads = 0;

        /// <summary>
        /// Gets a class from TableManager's FTK_playerGameStartDB
        /// <para>Must be called in a TableManager.Initialize postfix patch.</para>
        /// </summary>
        /// <param name="id">The class's id.</param>
        /// <returns>Returns FTK_playerGameStart</returns>
        public static FTK_playerGameStart GetClass(FTK_playerGameStart.ID id) {
            FTK_playerGameStartDB playerClassesDB = TableManager.Instance.Get<FTK_playerGameStartDB>();
            return playerClassesDB.m_Array[(int)id];
        }

        /// <summary>
        /// Add a custom class.
        /// <para>Must be called in a TableManager.Initialize postfix patch.</para>
        /// </summary>
        /// <param name="customClass">The custom class to be added.</param>
        /// <param name="plugin">Allows FTKModLib to know which plugin called this method. Not required but recommended to make debugging easier.</param>
        /// <returns>Returns FTK_playerGameStart.ID enum as int</returns>
        public static int AddClass(CustomClass customClass, BaseUnityPlugin plugin = null) {
            if (plugin != null) customClass.PLUGIN_ORIGIN = plugin.Info.Metadata.GUID;
            Instance.classesList.Add(customClass);

            ClassManager classManager = ClassManager.Instance;
            TableManager tableManager = TableManager.Instance;

            try {
                classManager.enums.Add(customClass.m_ID,
                    (int)Enum.GetValues(typeof(FTK_playerGameStart.ID)).Cast<FTK_playerGameStart.ID>().Max() + (successfulLoads + 1)
                );
                classManager.classesDictionary.Add(classManager.enums[customClass.m_ID], customClass);
                GEDataArrayBase geDataArrayBase = tableManager.Get(typeof(FTK_playerGameStartDB));
                if (!geDataArrayBase.IsContainID(customClass.m_ID)) {
                    geDataArrayBase.AddEntry(customClass.m_ID);
                    ((FTK_playerGameStartDB)geDataArrayBase).m_Array[tableManager.Get<FTK_playerGameStartDB>().m_Array.Length - 1] = customClass;
                    geDataArrayBase.CheckAndMakeIndex();
                }
                successfulLoads++;
                Logger.LogInfo($"Loaded '{customClass.ID}' of name '{customClass.DisplayName}' from {customClass.PLUGIN_ORIGIN}");
                return classManager.enums[customClass.m_ID];
            }
            catch (Exception e) {
                Logger.LogError(e);
                Logger.LogError($"Failed to load '{customClass.ID}' of name '{customClass.DisplayName}' from {customClass.PLUGIN_ORIGIN}");
                return -1;
            }
        }

        /// <summary>
        /// Modifies a class from TableManager's FTK_playerGameStartDB
        /// <para>Must be called in a TableManager.Initialize postfix patch.</para>
        /// </summary>
        /// <param name="id">The class's id.</param>
        /// <param name="customClass">The new class to override over.</param>
        /// <returns></returns>
        public static void ModifyClass(FTK_playerGameStart.ID id, CustomClass customClass) {
            FTK_playerGameStartDB playerClassesDB = TableManager.Instance.Get<FTK_playerGameStartDB>();
            playerClassesDB.m_Array[(int)id] = customClass;
        }

        /// <summary>
        ///    <para>The patches necessary to make adding custom items possible.</para>
        ///    <para>Many of them are used to fix issues calling an invalid 'FTK_playerGameStart.ID' 
        ///    by substituting with 'ClassManager.enums' dictionary value.</para>
        /// </summary>
        class HarmonyPatches {
            [HarmonyPatch(typeof(FTK_playerGameStartDB), "GetEntry")]
            class FTK_playerGameStartDB_GetEntry_Patch {
                static bool Prefix(ref FTK_playerGameStart __result, FTK_playerGameStart.ID _enumID) {
                    if (ClassManager.Instance.classesDictionary.TryGetValue((int)_enumID, out CustomClass customClass)) {
                        __result = customClass;
                        return false;
                    }
                    return true;
                }
            }

            [HarmonyPatch(typeof(FTK_playerGameStartDB), "GetIntFromID")]
            class FTK_playerGameStartDB_GetIntFromID_Patch {
                static bool Prefix(ref int __result, FTK_playerGameStartDB __instance, string _id) {
                    //Attempts to return our enum and calls the original function if it errors.
                    if (ClassManager.Instance.enums.ContainsKey(_id)) {
                        __result = ClassManager.Instance.enums[_id];
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}
