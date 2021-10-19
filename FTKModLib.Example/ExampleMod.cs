using BepInEx;
using FTKModLib.Managers;
using FTKModLib.Objects;
using GridEditor;
using HarmonyLib;
using System.Linq;

namespace FTKModLib.Example {
    [BepInPlugin("com.FTKModLib.Example", "FTKModLib Example Mod", "1.0.0")]
    [BepInDependency(FTKModLib.PLUGIN_GUID)]
    [BepInProcess("FTK.exe")]
    public class ExampleMod : BaseUnityPlugin {
        private void Awake() {
            Utils.Logger.LogInfo($"Plugin {Info.Metadata.GUID} is loaded!");

            ItemManager.AddItem(new ExampleItem(), this);
            ItemManager.AddItem(new ExampleWeapon(), this);

            Harmony harmony = new Harmony(Info.Metadata.GUID);
            harmony.PatchAll();
        }
    }

    public class ExampleItem : CustomItem {
        private readonly int healValue = 69;

        public ExampleItem() {
            ID = "CustomItem1";
            Name = new("Example Herb");
            Useable = true; // this is required to be set to true to use the item
            ObjectType = FTK_itembase.ObjectType.herb;
            ObjectSlot = FTK_itembase.ObjectSlot.belt;
            OnUseStatIncrementIDs = new string[] { "STAT_USED_HERBS" };
            Description = new("This is an example herb!\n\nIt heals for {0}!");
            ShopStock = 69;
            TownMarket = true;
            ItemRarity = FTK_itemRarityLevel.ID.rare;
        }

        // copied from HerbItemBase class
        // will allow this item to be used anywhere a herb can be used
        public override bool CanUse(CharacterOverworld _cow, bool _voteButton = false) {
            return base.CanUseOverworld(_cow) || base.CanUseOverworldInPoi(_cow) || base.CanUseCombat(_cow) || base.CanUseDungeon(_cow) || _voteButton;
        }

        // copied from herbGodsbeard1
        public override void OnUse(CharacterOverworld _cow, PlayerInventory.ContainerID _containerID) {
            base.OnUse(_cow, _containerID);
            _cow.m_CharacterStats.GainSpecificHealth(healValue, true, true);
            this.UsingFinished(true, false, false);
        }
    }

    public class ExampleWeapon : CustomItem {
        public ExampleWeapon() {
            ID = "CustomItem2";
            Name = new("Example Weapon");
            ObjectType = FTK_itembase.ObjectType.weapon; // This is required for the item to be registered as a weapon
            SkillType = FTK_weaponStats2.SkillType.toughness;
            WeaponType = Weapon.WeaponType.unarmed;
            ProficiencyEffects = new() { // these are the weapon attacks/skills for this custom item
                [FTK_proficiencyTable.ID.bladeDamage] = FTK_hitEffect.ID.defaultUnarmed,
                [FTK_proficiencyTable.ID.fire1] = FTK_hitEffect.ID.defaultUnarmed,
                [FTK_proficiencyTable.ID.firestorm1] = FTK_hitEffect.ID.defaultUnarmed,
            };
            Slots = 2;
            MaxDmg = 10;
            ShopStock = 1;
            TownMarket = true;
            ItemRarity = FTK_itemRarityLevel.ID.rare;
        }
    }

    //Gives item to Blacksmith as an additional starting item for an example.
    class HarmonyPatches {
        [HarmonyPatch(typeof(TableManager), "Initialize")]
        class TableManager_Initialize_Patch {
            static void Postfix() {
                ItemManager itemManager = ItemManager.Instance;
                FTK_playerGameStartDB playerGameStartDB = TableManager.Instance.Get<FTK_playerGameStartDB>();

                FTK_itembase.ID[] blacksmithOriginalStartItems = playerGameStartDB.m_Array[(int)FTK_playerGameStart.ID.blacksmith].m_StartItems;
                playerGameStartDB.m_Array[(int)FTK_playerGameStart.ID.blacksmith].m_StartItems = (
                    blacksmithOriginalStartItems.AddItem((FTK_itembase.ID)itemManager.enums["CustomItem1"]).ToArray()
                );
                playerGameStartDB.m_Array[(int)FTK_playerGameStart.ID.blacksmith].m_StartWeapon = (FTK_itembase.ID)itemManager.enums["CustomItem2"];

                Utils.Logger.LogInfo($"Added CustomItem1 to hunter's m_StartItems");
            }
        }
    }
}
