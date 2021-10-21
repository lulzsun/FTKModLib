using FTKModLib.Objects;
using GridEditor;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace FTKModLib.Managers {
    public class ClassManager : BaseManager<ClassManager> {
        public Dictionary<string, int> enums = new();
        public Dictionary<int, CustomClass> classesDictionary = new();
        public List<CustomClass> classesList = new();

        public static FTK_playerGameStart GetClass(FTK_playerGameStart.ID id) {
            FTK_playerGameStartDB playerClassesDB = TableManager.Instance.Get<FTK_playerGameStartDB>();
            return playerClassesDB.m_Array[(int)id];
        }

        public static void ModifyClass(FTK_playerGameStart.ID id, CustomClass customClass) {
            FTK_playerGameStartDB playerClassesDB = TableManager.Instance.Get<FTK_playerGameStartDB>();
            playerClassesDB.m_Array[(int)id] = customClass;
        }
    }
}
