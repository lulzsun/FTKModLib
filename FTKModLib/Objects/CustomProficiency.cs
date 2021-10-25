using GridEditor;
using System.Reflection;
using UnityEngine;

namespace FTKModLib.Objects {
    using ProficiencyManager = Managers.ProficiencyManager;

    public class CustomProficiency : ProficiencyBase {
        internal string PLUGIN_ORIGIN = "null";
        internal FTK_proficiencyTable proficiencyDetails {
            get => this.m_ProficiencyData;
            set => this.m_ProficiencyData = value;
        }

        public CustomProficiency(FTK_proficiencyTable.ID baseProf = FTK_proficiencyTable.ID.None) {
            if (baseProf != FTK_proficiencyTable.ID.None) {
                FTK_proficiencyTable source = ProficiencyManager.GetProficiency(baseProf);
                foreach (FieldInfo field in typeof(FTK_proficiencyTable).GetFields()) {
                    field.SetValue(this.proficiencyDetails, field.GetValue(source));
                }
            } else {
                if(proficiencyDetails == null) proficiencyDetails = new();
            }
        }

        public FTK_proficiencyTable.ID ProficiencyID {
            get => this.m_ProficiencyID;
            set => this.m_ProficiencyID = value;
        }
        public Category Category {
            get => this.m_Category;
            set => this.m_Category = value;
        }
        public SubCategory SubCategory {
            get => this.m_SubCategory;
            set => this.m_SubCategory = value;
        }
        public bool IsEndOnTurn {
            get => this.m_IsEndOnTurn;
            set => this.m_IsEndOnTurn = value;
        }
        public float CustomValue {
            get {
                return this.m_CustomValue;
            }
            set {
                this.m_CustomValue = value;
                m_ProficiencyData.m_CustomValue = value;
            }
        }

        /// <summary>
        /// This is the lookup string for the proficiency, recommended to make this as unique as possible
        /// </summary>
        public string ID {
            get => m_ProficiencyData.m_ID;
            set => m_ProficiencyData.m_ID = value;
        }
        private new CustomLocalizedString name;
        public CustomLocalizedString Name {
            get => name;
            set {
                name = value;
                m_ProficiencyData.m_DisplayTitle = name.GetLocalizedString();
            }
        }
        public ProficiencyBase ProficiencyPrefab {
            get => m_ProficiencyData.m_ProficiencyPrefab;
            set => m_ProficiencyData.m_ProficiencyPrefab = value;
        }
        public float TendencyWeight {
            get => m_ProficiencyData.m_TendencyWeight;
            set => m_ProficiencyData.m_TendencyWeight = value;
        }
        public CharacterStats.EnemyTendency Tendency {
            get => m_ProficiencyData.m_Tendency;
            set => m_ProficiencyData.m_Tendency = value;
        }
        public HitEffect HitEffectOverride {
            get => m_ProficiencyData.m_HitEffectOverride;
            set => m_ProficiencyData.m_HitEffectOverride = value;
        }
        public Weapon.WeaponType WpnTypeOverride {
            get => m_ProficiencyData.m_WpnTypeOverride;
            set => m_ProficiencyData.m_WpnTypeOverride = value;
        }
        public FTK_weaponStats2.DamageType DmgTypeOverride {
            get => m_ProficiencyData.m_DmgTypeOverride;
            set => m_ProficiencyData.m_DmgTypeOverride = value;
        }
        public int SlotOverride {
            get => m_ProficiencyData.m_SlotOverride;
            set => m_ProficiencyData.m_SlotOverride = value;
        }
        public float PerSlotSkillRoll {
            get => m_ProficiencyData.m_PerSlotSkillRoll;
            set => m_ProficiencyData.m_PerSlotSkillRoll = value;
        }
        public bool IgnoresArmor {
            get => m_ProficiencyData.m_IgnoresArmor;
            set => m_ProficiencyData.m_IgnoresArmor = value;
        }
        public float DmgMultiplier {
            get => m_ProficiencyData.m_DmgMultiplier;
            set => m_ProficiencyData.m_DmgMultiplier = value;
        }
        public bool ChaosOption {
            get => m_ProficiencyData.m_ChaosOption;
            set => m_ProficiencyData.m_ChaosOption = value;
        }
        public bool Suicide {
            get => m_ProficiencyData.m_Suicide;
            set => m_ProficiencyData.m_Suicide = value;
        }
        public bool Harmless {
            get => m_ProficiencyData.m_Harmless;
            set => m_ProficiencyData.m_Harmless = value;
        }
        public bool TargetFriendly {
            get => m_ProficiencyData.m_TargetFriendly;
            set => m_ProficiencyData.m_TargetFriendly = value;
        }
        public bool GunShot {
            get => m_ProficiencyData.m_GunShot;
            set => m_ProficiencyData.m_GunShot = value;
        }
        public CharacterDummy.TargetType Target {
            get => m_ProficiencyData.m_Target;
            set => m_ProficiencyData.m_Target = value;
        }
        public string DisplayTitle {
            get => m_ProficiencyData.m_DisplayTitle;
            set => m_ProficiencyData.m_DisplayTitle = value;
        }
        public bool CheckID {
            get => m_ProficiencyData.m_CheckID;
            set => m_ProficiencyData.m_CheckID = value;
        }
        public Color Tint {
            get => m_ProficiencyData.m_Tint;
            set => m_ProficiencyData.m_Tint = value;
        }
        public bool AlwaysHitFx {
            get => m_ProficiencyData.m_AlwaysHitFx;
            set => m_ProficiencyData.m_AlwaysHitFx = value;
        }
        public Sprite BattleButton {
            get => m_ProficiencyData.m_BattleButton;
            set => m_ProficiencyData.m_BattleButton = value;
        }
        public FTK_ragdollDeath.ID WeaponSizeOverride {
            get => m_ProficiencyData.m_WeaponSizeOverride;
            set => m_ProficiencyData.m_WeaponSizeOverride = value;
        }
        public FTK_ragdollDeath.DirectionOverride DirectionOverride {
            get => m_ProficiencyData.m_DirectionOverride;
            set => m_ProficiencyData.m_DirectionOverride = value;
        }
        public Sprite SlotIcon {
            get => m_ProficiencyData.m_SlotIcon;
            set => m_ProficiencyData.m_SlotIcon = value;
        }
        public int RepeatCount {
            get => m_ProficiencyData.m_RepeatCount;
            set => m_ProficiencyData.m_RepeatCount = value;
        }
        public float ChanceToAffect {
            get => m_ProficiencyData.m_ChanceToAffect;
            set => m_ProficiencyData.m_ChanceToAffect = value;
        }
        public int DamagePerAttack {
            get => m_ProficiencyData.m_DamagePerAttack;
            set => m_ProficiencyData.m_DamagePerAttack = value;
        }
        public int BoatDamage {
            get => m_ProficiencyData.m_BoatDamage;
            set => m_ProficiencyData.m_BoatDamage = value;
        }
        public bool FullSlots {
            get => m_ProficiencyData.m_FullSlots;
            set => m_ProficiencyData.m_FullSlots = value;
        }
        public float Quickness {
            get => m_ProficiencyData.m_Quickness;
            set => m_ProficiencyData.m_Quickness = value;
        }
    }
}
