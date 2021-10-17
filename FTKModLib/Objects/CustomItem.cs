using FTKItemName;
using Google2u;
using GridEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Weapon;

namespace FTKModLib.Objects {

    /// <summary>
    /// <para>Items and Weapons are two seperate things in this game,
    /// but this class will combine them into one class for simplicity</para>
    ///
    /// <para>FTKItem          = functionality of item</para>
    /// <para>FTK_weaponStats2 = details of weapon</para>
    /// <para>FTK_items        = details of item</para>
    /// </summary>
    public class CustomItem : FTKItem {
        internal string PLUGIN_ORIGIN = "null";
        internal readonly FTK_items itemDetails = new FTK_items();
        internal readonly FTK_weaponStats2 weaponDetails = new FTK_weaponStats2();

        /// <summary>
        /// This is the lookup string for the item, recommended to make this as unique as possible
        /// </summary>
        public string ID {
            get => itemDetails.m_ID;
            set { 
                itemDetails.m_ID = value;
                weaponDetails.m_ID = value;
            }
        }
        private TextItemsRow textItemsRow;
        private CustomLocalizedString name;
        /// <summary>
        /// This is the item's ingame display name, supports localized language
        /// </summary>
        public CustomLocalizedString Name {
            get => name;
            set {
                textItemsRow = new TextItemsRow(
                    ID,
                    value._en, value._fr, value._it,
                    value._de, value._es, value._pt_br,
                    value._ru, value._zh_cn, value._zh_tw,
                    value._pl, value._ja, value._ko
                );
                name = value;
            }
        }
        public string GetName() {
            if (textItemsRow == null) {
                return ID;
            }
            MethodInfo private_LocalizeRelease = typeof(FTKHub).GetMethod("LocalizeRelease", BindingFlags.NonPublic | BindingFlags.Static);
            return (string)private_LocalizeRelease.Invoke(private_LocalizeRelease, new object[] { textItemsRow });
        }
        private TextItemsDescriptionRow textItemsDescriptionRow;
        private CustomLocalizedString description;
        /// <summary>
        /// <para>This is the item's ingame description, supports localized language</para>
        /// <para>Custom Weapons do not use this field</para>
        /// </summary>
        public CustomLocalizedString Description {
            get => description;
            set {
                textItemsDescriptionRow = new TextItemsDescriptionRow(
                    ID,
                    value._en, value._fr, value._it,
                    value._de, value._es, value._pt_br,
                    value._ru, value._zh_cn, value._zh_tw,
                    value._pl, value._ja, value._ko
                );
                description = value;
            }
        }
        public override string GetDescription(CharacterOverworld _cow) {
            if (!string.IsNullOrEmpty(this.GetCannotUseReason(_cow))) {
                return this.GetCannotUseReason(_cow);
            }
            if(textItemsDescriptionRow == null) {
                return "This custom item is missing a description!";
            }
            MethodInfo private_LocalizeRelease = typeof(FTKHub).GetMethod("LocalizeRelease", BindingFlags.NonPublic | BindingFlags.Static);
            string format = (string)private_LocalizeRelease.Invoke(private_LocalizeRelease, new object[] { textItemsDescriptionRow });
            int num = this.GetValue(_cow);
            if (num < 0 && this.DisplayPositiveValue()) {
                num *= -1;
            }
            return string.Format(format, num);
        }
        public FTK_itemRarityLevel.ID ItemRarity {
            get => itemDetails.m_ItemRarity;
            set {
                itemDetails.m_ItemRarity = value;
                weaponDetails.m_ItemRarity = value;
            }
        }
        public string[] OnUseStatIncrementIDs {
            get => itemDetails.m_OnUseStatIncrementIDs;
            set {
                itemDetails.m_OnUseStatIncrementIDs = value;
                weaponDetails.m_OnUseStatIncrementIDs = value;
            }
        }
        public bool SuppressUseSound {
            get => itemDetails.m_SuppressUseSound;
            set {
                itemDetails.m_SuppressUseSound = value;
                weaponDetails.m_SuppressUseSound = value;
            }
        }
        public FTK_itembase.ObjectSlot ObjectSlot {
            get => itemDetails.m_ObjectSlot;
            set {
                itemDetails.m_ObjectSlot = value;
                weaponDetails.m_ObjectSlot = value;
            }
        }
        public FTK_itembase.ObjectType ObjectType {
            get => itemDetails.m_ObjectType;
            set {
                itemDetails.m_ObjectType = value;
                weaponDetails.m_ObjectType = value;
            }
        }
        public bool IsWeapon {
            get {
                return ObjectType == FTK_itembase.ObjectType.weapon;
            }
        }
        public bool CursedItem {
            get => itemDetails.m_CursedItem;
            set {
                itemDetails.m_CursedItem = value;
                weaponDetails.m_CursedItem = value;
            }
        }
        public bool BackpackEquip {
            get => itemDetails.m_BackpackEquip;
            set {
                itemDetails.m_BackpackEquip = value;
                weaponDetails.m_BackpackEquip = value;
            }
        }
        public string CollectLoreItemUnlock {
            get => itemDetails.m_CollectLoreItemUnlock;
            set {
                itemDetails.m_CollectLoreItemUnlock = value;
                weaponDetails.m_CollectLoreItemUnlock = value;
            }
        }
        public bool FilterDebug {
            get => itemDetails.m_FilterDebug;
            set {
                itemDetails.m_FilterDebug = value;
                weaponDetails.m_FilterDebug = value;
            }
        }
        public bool FilterEndDungeon {
            get => itemDetails.m_FilterEndDungeon;
            set {
                itemDetails.m_FilterEndDungeon = value;
                weaponDetails.m_FilterEndDungeon = value;
            }
        }
        public bool Dropable {
            get => itemDetails.m_Dropable;
            set {
                itemDetails.m_Dropable = value;
                weaponDetails.m_Dropable = value;
            }
        }
        public bool DungeonMerchant {
            get => itemDetails.m_DungeonMerchant;
            set {
                itemDetails.m_DungeonMerchant = value;
                weaponDetails.m_DungeonMerchant = value;
            }
        }
        public bool TownMarket {
            get => itemDetails.m_TownMarket;
            set {
                itemDetails.m_TownMarket = value;
                weaponDetails.m_TownMarket = value;
            }
        }
        public int MaxLevel {
            get => itemDetails.m_MaxLevel;
            set {
                itemDetails.m_MaxLevel = value;
                weaponDetails.m_MaxLevel = value;
            }
        }
        public bool NightMarket {
            get => itemDetails.m_NightMarket;
            set {
                itemDetails.m_NightMarket = value;
                weaponDetails.m_NightMarket = value;
            }
        }
        public bool PriceScale {
            get => itemDetails.m_PriceScale;
            set {
                itemDetails.m_PriceScale = value;
                weaponDetails.m_PriceScale = value;
            }
        }
        public int ShopStock {
            get => itemDetails._shopStock;
            set {
                itemDetails._shopStock = value;
                weaponDetails._shopStock = value;
            }
        }
        public int GoldValue {
            get => itemDetails._goldValue;
            set {
                itemDetails._goldValue = value;
                weaponDetails._goldValue = value;
            }
        }
        public GameObject Prefab {
            get {
                if (IsWeapon) {
                    if(weaponDetails.m_Prefab == null) {
                        weaponDetails.m_Prefab = FTKHub.Instance.m_UnarmedWeapons[0];
                    }
                    return weaponDetails.m_Prefab;
                }
                return itemDetails.m_Prefab;
            }
            set {
                itemDetails.m_Prefab = value;
                weaponDetails.m_Prefab = value;
            }
        }
        public void ForceUpdatePrefab() {
            if (IsWeapon) {
                Weapon weapon = Prefab.GetComponentInChildren<Weapon>();
                weapon.m_ProficiencyEffects = ProficiencyEffects;
                weapon.m_HitTargetVel = HitTargetVel;
                weapon.m_WeaponSize = WeaponSize;
                weapon.m_HitTarget ??= HitTarget;
                weapon.m_LuteMiss ??= LuteMiss;
                weapon.m_LuteHit ??= LuteHit;
                weapon.m_BowStringRenderer ??= BowStringRenderer;
                weapon.m_DropWeaponVelScale = DropWeaponVelScale;
                weapon.m_IsHideWeaponWhenDrop = IsHideWeaponWhenDrop;
                weapon.m_IsPlayAttackParticleWhenDrop = IsPlayAttackParticleWhenDrop;
                weapon.m_IsDetachAttackParticle = IsDetachAttackParticle;
                weapon.m_AttackParticle ??= AttackParticle;
                weapon.m_ParticleTarget ??= ParticleTarget;
                weapon.m_BreakRoot ??= BreakRoot;
                weapon.m_WeaponHolderName ??= WeaponHolderName;
                weapon.m_Particles ??= Particles;
                weapon.m_WeaponType = WeaponType;
                weapon.m_WeaponSubType = WeaponSubType;
                weapon.m_OffHand ??= OffHand;
                weapon.m_AnimationController ??= AnimationController;
                weapon.m_IdleAnimOverride = IdleAnimOverride;
                weapon.m_AmmoCapacity = AmmoCapacity;
                weapon.m_ImpactSound ??= ImpactSound;
                weapon.m_ImpactSoundOverride ??= ImpactSoundOverride;
                weapon.m_WeaponMaterial = WeaponMaterial;
            }
        }

        // fields used for weaponDetails
        public string AttackDisplay {
            get => weaponDetails.m_AttackDisplay;
            set {
                weaponDetails.m_AttackDisplay = value;
            }
        }

        /// <summary>
        /// <para>This cannot be less than one.</para>
        /// </summary>
        public int Slots {
            get => weaponDetails._slots;
            set {
                if (value <= 0) value = 1;
                weaponDetails._slots = value;
            }
        }
        public float MaxDmg {
            get => weaponDetails._maxdmg;
            set {
                weaponDetails._maxdmg = value;
            }
        }
        public FTK_weaponStats2.DamageType DmgType {
            get => weaponDetails._dmgtype;
            set {
                weaponDetails._dmgtype = value;
            }
        }
        public FTK_weaponStats2.SkillType SkillType {
            get => weaponDetails._skilltest;
            set {
                weaponDetails._skilltest = value;
            }
        }
        public float DmgGain {
            get => weaponDetails._dmggain;
            set {
                weaponDetails._dmggain = value;
            }
        }
        public bool CanBreak {
            get => weaponDetails.m_CanBreak;
            set {
                weaponDetails.m_CanBreak = value;
            }
        }
        public bool NoFocus {
            get => weaponDetails.m_NoFocus;
            set {
                weaponDetails.m_NoFocus = value;
            }
        }
        public bool NoRegularAttack {
            get => weaponDetails.m_NoRegularAttack;
            set {
                weaponDetails.m_NoRegularAttack = value;
            }
        }
        // fields used for Weapon component inside prefab
        public Dictionary<ProficiencyID, HitEffect> ProficiencyEffects;
        public Vector3 HitTargetVel;
        public FTK_ragdollDeath.ID WeaponSize;
        public Transform HitTarget;
        public AkLuteSoundID LuteMiss;
        public AkLuteSoundID LuteHit;
        public BowStringRenderer BowStringRenderer;
        public float DropWeaponVelScale = 0.5f;
        public bool IsHideWeaponWhenDrop;
        public bool IsPlayAttackParticleWhenDrop;
        public bool IsDetachAttackParticle;
        public MagicParticle AttackParticle;
        public Transform ParticleTarget;
        public Transform BreakRoot;
        public string WeaponHolderName = "WEAPON_HOLDER";
        public GameObject Particles;
        public WeaponType WeaponType;
        public WeaponSubType WeaponSubType;
        public GameObject OffHand;
        public RuntimeAnimatorController AnimationController;
        public IdleAnimOverride IdleAnimOverride;
        public int AmmoCapacity;
        public AkEventID ImpactSound;
        public AkEventID ImpactSoundOverride;
        public WeaponMaterial WeaponMaterial;
    }
}
