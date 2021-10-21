using FTKModLib.Objects;
using GridEditor;

namespace FTKModLib.Example {
    public class ExampleHerb : CustomItem {
        private readonly int healValue = 69;

        public ExampleHerb() {
            ID = "CustomHerb1";
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
}
