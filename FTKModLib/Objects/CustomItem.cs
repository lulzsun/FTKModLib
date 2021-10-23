using FTKItemName;
using FTKModLib.Managers;
using GridEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Weapon;
using Logger = FTKModLib.Utils.Logger;

namespace FTKModLib.Objects {

    /// <summary>
    /// <para>Items and Weapons are two seperate things in this game,
    /// but this class will combine them into one class for simplicity</para>
    ///
    /// <para>FTKItem          = functionality of item</para>
    /// <para>FTK_weaponStats2 = details of weapon</para>
    /// <para>FTK_items        = details of item</para>
    /// 
    /// <para>Weapons attacks/skills are referred to as an 'Proficiency' object,
    /// this class will simplify them into a WeaponSkill object.</para>
    /// </summary>
    public class CustomItem : ConsumableBase {
        internal string PLUGIN_ORIGIN = "null";
        internal FTK_items itemDetails = new FTK_items();
        internal FTK_weaponStats2 weaponDetails = new FTK_weaponStats2();

        public CustomItem(FTK_itembase.ID baseItem = FTK_itembase.ID.None) {
            if(baseItem != FTK_itembase.ID.None) {
                CustomItem source = ItemManager.GetItem(baseItem);
                this.itemDetails = source.itemDetails;
                this.weaponDetails = source.weaponDetails;
                foreach (FieldInfo field in typeof(CustomItem).GetFields()) {
                    field.SetValue(this, field.GetValue(source));
                }
            }
            else {
                weaponDetails.m_NoRegularAttack = true;
                itemDetails.m_Icon = new Sprite();
                itemDetails.m_IconNonClickable = new Sprite();
                weaponDetails.m_Icon = new Sprite();
                weaponDetails.m_IconNonClickable = new Sprite();
            }
        }

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
        private CustomLocalizedString name;
        /// <summary>
        /// This is the item's ingame display name, supports localized language
        /// </summary>
        public CustomLocalizedString Name {
            get => name;
            set {
                name = value;
            }
        }
        public string GetName() {
            if (name == null) {
                return ID;
            }
            return name.GetLocalizedString();
        }
        private CustomLocalizedString description;
        /// <summary>
        /// <para>This is the item's ingame description, supports localized language</para>
        /// <para>Custom Weapons do not use this field</para>
        /// </summary>
        public CustomLocalizedString Description {
            get => description;
            set {
                description = value;
            }
        }
        public override string GetDescription(CharacterOverworld _cow) {
            if (!string.IsNullOrEmpty(this.GetCannotUseReason(_cow))) {
                return this.GetCannotUseReason(_cow);
            }
            if(description == null) {
                return "This custom item is missing a description!";
            }
            string format = description.GetLocalizedString();
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
        public bool Useable {
            get => itemDetails._useable;
            set {
                itemDetails._useable = value;
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
        /// <summary>
        /// <para>Prefab of the item.</para>
        /// <para>If item is a weapon and the prefab is null, it will default to an unarmed prefab.</para>
        /// <para>Else, it will use a default cube or nothing at all.</para>
        /// </summary>
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
                if(value == null) {
                    Logger.LogError("Trying to set a null prefab? Defaulting to unarmed prefab is item is a weapon.");
                    return;
                }
                itemDetails.m_Prefab = value;
                weaponDetails.m_Prefab = value;
            }
        }
        public void ForceUpdatePrefab() {
            if (IsWeapon) {
                Weapon weapon = Prefab.GetComponentInChildren<Weapon>();
                if(weapon == null) { // if a weapon component doesn't exist, its probably a custom prefab
                    var mesh = Prefab.transform.Find("root");
                    if (mesh == null) {
                        Logger.LogError("Weapon prefab does not contain a child called 'root'!");
                        throw new Exception();
                    }
                    weapon = mesh.gameObject.AddComponent<Weapon>();
                    Logger.LogInfo($"Added Weapon Script to {Prefab}");
                }

                weapon.m_ProficiencyEffects = new Dictionary<ProficiencyID, HitEffect>();
                foreach (var prof in ProficiencyEffects) {
                    weapon.m_ProficiencyEffects.Add(
                        new ProficiencyID(prof.Key), 
                        TableManager.Instance.Get<FTK_hitEffectDB>().GetEntry(prof.Value).m_Prefab
                    );
                }
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
        /// <para>This is the default number of rolls for attacks</para>
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
        /// <summary>
        /// <para>The weapon's default attack/skill, does raw damage, no effects.</para>
        /// </summary>
        public bool NoRegularAttack {
            get => weaponDetails.m_NoRegularAttack;
            set {
                weaponDetails.m_NoRegularAttack = value;
            }
        }
        // fields used for Weapon component inside prefab
        /// <summary>
        /// <para>The weapon's attacks/skills, using the vanilla proficiencies</para>
        /// </summary>
        public Dictionary<FTK_proficiencyTable.ID, FTK_hitEffect.ID> ProficiencyEffects = new();
        //public Dictionary<int, int> CustomProficiencyEffects = new();
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
        public WeaponType WeaponType = WeaponType.unarmed;
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
