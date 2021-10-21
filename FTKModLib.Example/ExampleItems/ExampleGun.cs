using FTKModLib.Managers;
using FTKModLib.Objects;
using GridEditor;
using UnityEngine;

namespace FTKModLib.Example {
    public class ExampleGun : CustomItem {
        public ExampleGun() {
            ID = "CustomGun1";
            Name = new("Example Gun");
            Prefab = ExampleMod.assetBundle.LoadAsset<GameObject>("Assets/CustomItems/customGun01.prefab");
            ObjectSlot = FTK_itembase.ObjectSlot.twoHands;
            ObjectType = FTK_itembase.ObjectType.weapon; // This is required for the item to be registered as a weapon
            SkillType = FTK_weaponStats2.SkillType.toughness;
            WeaponType = Weapon.WeaponType.firearm;
            ProficiencyEffects = new() { // these are the weapon attacks/skills for this custom item
                [FTK_proficiencyTable.ID.gunFire] = FTK_hitEffect.ID.defaultRange,
                [FTK_proficiencyTable.ID.fire1] = FTK_hitEffect.ID.defaultRange,
                [FTK_proficiencyTable.ID.gunDazeAOE] = FTK_hitEffect.ID.defaultRange,
            };
            AnimationController = AssetManager.GetAnimationControllers<Weapon>().Find(i => i.name == "player_Musket_Combat");
            Slots = 2;
            MaxDmg = 10;
            ShopStock = 1;
            TownMarket = true;
            ItemRarity = FTK_itemRarityLevel.ID.rare;
        }
    }
}
