using FTKItemName;
using Google2u;
using GridEditor;
using System.Reflection;
using UnityEngine;

namespace FTKModLib.Objects {
    public class CustomItem : FTKItem {
        //what are ironoak's naming conventions? why are FTKItem and FTK_items named this way? codesmell...
        // FTK_items = global store of general stats/info
        // FTKItem   = contains the functionality of the item
        private readonly FTK_items info = new FTK_items();
        public T Get<T>() where T : FTK_items {
            return (T)info;
        }
        /// <summary>
        /// This is the lookup string for the item, recommended to make this as unique as possible
        /// </summary>
        public string ID {
            get => info.m_ID;
            set => info.m_ID = value;
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
        /// This is the item's ingame description, supports localized language
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
            get => info.m_ItemRarity;
            set => info.m_ItemRarity = value;
        }
        public string[] OnUseStatIncrementIDs {
            get => info.m_OnUseStatIncrementIDs;
            set => info.m_OnUseStatIncrementIDs = value;
        }
        public bool SuppressUseSound {
            get => info.m_SuppressUseSound;
            set => info.m_SuppressUseSound = value;
        }
        public FTK_itembase.ObjectSlot ObjectSlot {
            get => info.m_ObjectSlot;
            set => info.m_ObjectSlot = value;
        }
        public FTK_itembase.ObjectType ObjectType {
            get => info.m_ObjectType;
            set => info.m_ObjectType = value;
        }
        public bool CursedItem {
            get => info.m_CursedItem;
            set => info.m_CursedItem = value;
        }
        public bool BackpackEquip {
            get => info.m_BackpackEquip;
            set => info.m_BackpackEquip = value;
        }
        public string CollectLoreItemUnlock {
            get => info.m_CollectLoreItemUnlock;
            set => info.m_CollectLoreItemUnlock = value;
        }
        public bool FilterDebug {
            get => info.m_FilterDebug;
            set => info.m_FilterDebug = value;
        }
        public bool FilterEndDungeon {
            get => info.m_FilterEndDungeon;
            set => info.m_FilterEndDungeon = value;
        }
        public bool Dropable {
            get => info.m_Dropable;
            set => info.m_Dropable = value;
        }
        public bool DungeonMerchant {
            get => info.m_DungeonMerchant;
            set => info.m_DungeonMerchant = value;
        }
        public bool TownMarket {
            get => info.m_TownMarket;
            set => info.m_TownMarket = value;
        }
        public int MaxLevel {
            get => info.m_MaxLevel;
            set => info.m_MaxLevel = value;
        }
        public bool NightMarket {
            get => info.m_NightMarket;
            set => info.m_NightMarket = value;
        }
        public bool PriceScale {
            get => info.m_PriceScale;
            set => info.m_PriceScale = value;
        }
        public int ShopStock {
            get => info._shopStock;
            set => info._shopStock = value;
        }
        public int GoldValue {
            get => info._goldValue;
            set => info._goldValue = value;
        }
    }
}
