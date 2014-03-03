using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects.Parsers;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    [Serializable]
    public partial class ItemDetailsEntry : ResponseObject
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public ItemType ItemType { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("rarity")]
        public RarityType Rarity { get; set; }

        [JsonProperty("vendor_value")]
        public int VendorValue { get; set; }

        [JsonProperty("icon_file_id")]
        public int IconFileId { get; set; }

        [JsonProperty("icon_file_signature")]
        public string IconFileSignature { get; set; }

        [JsonProperty("game_types")]
        public IList<GameType> GameType { get; set; }

        [JsonProperty("flags")]
        public string[] Flags { get; set; }

        [JsonProperty("restrictions")]
        public string[] Restrictions { get; set; }
        [JsonProperty("weapon")]
        public WeaponInfo WeaponDetails { get; set; }
        [JsonProperty("upgrade_component")]
        public UpgradeComponentInfo UpgradeComponentDetails { get; set; }
        //[JsonProperty("trophy")] no details defined for items as of 9/10/2013
        public TrophyInfo TrophyDetails { get; set; }
        [JsonProperty("trinket")]
        public TrinketInfo TrinketDetails { get; set; }
        [JsonProperty("tool")]
        public ToolInfo ToolDetails { get; set; }
        [JsonProperty("gathering")]
        public GatheringInfo GatheringDetails { get; set; }
        [JsonProperty("gizmo")]
        public GizmoInfo GizmoDetails { get; set; }
        //[JsonProperty("crafting")], no details defined for items as of 9/10/2013
        public CraftingMaterialInfo CraftingMaterialDetails { get; set; }
        //public MiniPetInfo MiniPetDetails {get; set;} no details defined for items as of 9/10/2013
        [JsonProperty("container")]
        public ContainerInfo ContainerDetails { get; set; }
        [JsonProperty("consumable")]
        public ConsumableInfo ConsumableDetails { get; set; }
        [JsonProperty("bag")]
        public BagInfo BagDetails { get; set; }
        [JsonProperty("back")]
        public BackInfo BackDetails { get; set; }
        [JsonProperty("armor")]
        public ArmorInfo ArmorDetails { get; set; }



        /// <summary>
        /// Default Constructor
        /// </summary>
        public ItemDetailsEntry()
        {
        }

        private string propertyFormat = "{0}: {1}\n";
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}: {1} ({2})\n", "Name", Name, ItemId);
            sb.AppendFormat(propertyFormat, "Type", ItemType);
            sb.AppendFormat(propertyFormat, "Level", Level);
            sb.AppendFormat(propertyFormat, "Rarity", Rarity);
            sb.AppendFormat(propertyFormat, "Vendor Value", VendorValue);
            sb.AppendLine(_getDetails(this));
            return sb.ToString();
        }

        private string _getDetails(ItemDetailsEntry item)
        {
            StringBuilder sb = new StringBuilder();
            if (item.ArmorDetails != null)
                sb.Append(item.ArmorDetails.GetType().Name + "\n" + item.ArmorDetails.ToString());
            if (item.BackDetails != null)
                sb.Append(item.BackDetails.GetType().Name + "\n" + item.BackDetails.ToString());
            if (item.BagDetails != null)
                sb.Append(item.BagDetails.GetType().Name + "\n" + item.BagDetails.ToString());
            if (item.ConsumableDetails != null)
                sb.Append(item.ConsumableDetails.GetType().Name + "\n" + item.ConsumableDetails.ToString());
            if (item.ContainerDetails != null)
                sb.Append(item.ContainerDetails.GetType().Name + "\n" + item.ContainerDetails.ToString());
            if (item.CraftingMaterialDetails != null)
                sb.Append(item.CraftingMaterialDetails.GetType().Name + "\n" + item.CraftingMaterialDetails.ToString());
            if (item.GizmoDetails != null)
                sb.Append(item.GizmoDetails.GetType().Name + "\n" + item.GizmoDetails.ToString());
            if (item.ToolDetails != null)
                sb.Append(item.ToolDetails.GetType().Name + "\n" + item.ToolDetails.ToString());
            if (item.TrinketDetails != null)
                sb.Append(item.TrinketDetails.GetType().Name + "\n" + item.TrinketDetails.ToString());
            if (item.TrophyDetails != null)
                sb.Append(item.TrophyDetails.GetType().Name + "\n" + item.TrophyDetails.ToString());
            if (item.WeaponDetails != null)
                sb.Append(item.WeaponDetails.GetType().Name + "\n" + item.WeaponDetails.ToString());
            return sb.ToString();
        }
    }

    public partial class ItemDetailsEntry
    {
        #region Nested Classes

        [Serializable]
        public class ItemTypeInfo
        {

            public override string ToString()
            {
                var properties = GetType().GetProperties();
                StringBuilder sb = new StringBuilder();
                foreach (var property in properties)
                {
                    if (property.PropertyType.IsAssignableFrom(typeof(IList<ItemDetailsEntry.InfusionSlotInfo>)))
                    {
                        var collection = property.GetValue(this) as IList<ItemDetailsEntry.InfusionSlotInfo>;
                        sb.AppendFormat("{0}: {1}\n", property.Name, collection.Count);
                        foreach (var item in collection)
                            sb.AppendFormat("  {0}\n", item);
                    }
                    else
                        sb.AppendFormat("{0}: {1}\n", property.Name, property.GetValue(this));
                }
                return sb.ToString();
            }
        }

        [Serializable]
        public class WeaponInfo : ItemTypeInfo
        {

            /// <summary>
            /// Weapon Types
            /// </summary>
            public enum WeaponType
            {
                /// <summary>
                /// long bow
                /// </summary>
                LongBow,

                /// <summary>
                /// pistol
                /// </summary>
                Pistol,

                /// <summary>
                /// warhorn
                /// </summary>
                Warhorn,

                /// <summary>
                /// sword
                /// </summary>
                Sword,

                /// <summary>
                /// staff
                /// </summary>
                Staff,

                /// <summary>
                /// hammer
                /// </summary>
                Hammer,

                /// <summary>
                /// trident
                /// </summary>
                Trident,

                /// <summary>
                /// scepter
                /// </summary>
                Scepter,

                /// <summary>
                /// speargun
                /// </summary>
                Speargun,

                /// <summary>
                /// mace
                /// </summary>
                Mace,

                /// <summary>
                /// axe
                /// </summary>
                Axe,

                /// <summary>
                /// torch
                /// </summary>
                Torch,

                /// <summary>
                /// dagger
                /// </summary>
                Dagger,

                /// <summary>
                /// shield
                /// </summary>
                Shield,

                /// <summary>
                /// harpoon
                /// </summary>
                Harpoon,

                /// <summary>
                /// greatsword
                /// </summary>
                Greatsword,

                /// <summary>
                /// rifle
                /// </summary>
                Rifle,

                /// <summary>
                /// focus
                /// </summary>
                Focus,

                /// <summary>
                /// short bow
                /// </summary>
                ShortBow,

                /// <summary>
                /// toy
                /// </summary>
                Toy,

                /// <summary>
                /// two handed toy
                /// </summary>
                TwoHandedToy,

                /// <summary>
                /// large bundle
                /// </summary>
                LargeBundle
            }

            /// <summary>
            /// Weapon Damage Types.
            /// <remarks>The type of damage a weapon deals</remarks>
            /// </summary>
            public enum WeaponDamageType
            {
                /// <summary>
                /// Deals physical damage.
                /// </summary>
                Physical,

                /// <summary>
                /// Deals fire damage.
                /// </summary>
                Fire,

                /// <summary>
                /// Deals ice damage.
                /// </summary>
                Ice,

                /// <summary>
                /// Deals lightning damage.
                /// </summary>
                Lightning,
            }

            /// <summary>
            /// typeDeals 
            /// </summary>
            [JsonProperty("type")]
            public WeaponType Type
            {
                get;
                set;
            }

            private int _suffixId = -1;

            /// <summary>
            /// Item suffix id
            /// </summary>
            public int SuffixId
            {
                get
                {
                    if (_suffixId < 0) int.TryParse(_suffixIdString, out _suffixId);
                    return _suffixId;
                }
                set { _suffixId = value; }
            }

            [JsonProperty("suffix_item_id")]
            private string _suffixIdString { get; set; }


            /// <summary>
            /// minimum powerDeals 
            /// </summary>
            [JsonProperty("min_power")]
            public int MinPower
            {
                get;
                set;
            }

            /// <summary>
            /// maximum powerDeals 
            /// </summary>
            [JsonProperty("max_power")]
            public int MaxPower
            {
                get;
                set;
            }

            /// <summary>
            /// infusion slotsDeals 
            /// </summary>
            [JsonProperty("infusion_slots")]
            public IList<InfusionSlotInfo> InfusionSlots
            {
                get;
                set;
            }

            /// <summary>
            /// infix upgradeDeals 
            /// </summary>
            [JsonProperty("infix_upgrade")]
            public InfixUpgradeInfo InfixUpgrade
            {
                get;
                set;
            }

            /// <summary>
            /// defenseDeals 
            /// </summary>
            [JsonProperty("defense")]
            public int Defense
            {
                get;
                set;
            }

            /// <summary>
            /// damage typeDeals 
            /// </summary>
            [JsonProperty("damage_type")]
            public WeaponDamageType DamageType
            {
                get;
                set;
            }
        }

        [Serializable]
        public class UpgradeComponentInfo : ItemTypeInfo
        {

            /// <summary>
            /// upgrade component flags.
            /// </summary>
            public enum UpgradeComponentType
            {
                /// <summary>
                /// heavy armor type
                /// </summary>
                HeavyArmor,

                /// <summary>
                /// medium armor type
                /// </summary>
                MediumArmor,

                /// <summary>
                /// light armor type
                /// </summary>
                LightArmor,

                /// <summary>
                ///  axe type
                /// </summary>
                Axe,

                /// <summary>
                /// dagger type
                /// </summary>
                Dagger,

                /// <summary>
                /// focus type
                /// </summary>
                Focus,

                /// <summary>
                /// greatsword type
                /// </summary>
                Greatsword,

                /// <summary>
                /// hammer type
                /// </summary>
                Hammer,

                /// <summary>
                /// harpoon type
                /// </summary>
                Harpoon,

                /// <summary>
                /// long bow type
                /// </summary>
                LongBow,

                /// <summary>
                /// mace type
                /// </summary>
                Mace,

                /// <summary>
                /// pistol type
                /// </summary>
                Pistol,

                /// <summary>
                /// rifle type
                /// </summary>
                Rifle,

                /// <summary>
                /// scepter type
                /// </summary>
                Scepter,

                /// <summary>
                /// shield type
                /// </summary>
                Shield,

                /// <summary>
                /// short bow type
                /// </summary>
                ShortBow,

                /// <summary>
                /// speargun type
                /// </summary>
                Speargun,

                /// <summary>
                /// staff type
                /// </summary>
                Staff,

                /// <summary>
                /// sword type
                /// </summary>
                Sword,

                /// <summary>
                /// torch type
                /// </summary>
                Torch,

                /// <summary>
                /// trident type
                /// </summary>
                Trident,

                /// <summary>
                /// trinket type
                /// </summary>
                Trinket,

                /// <summary>
                /// warhorn type
                /// </summary>
                Warhorn
            }

            /// <summary>
            /// Possible Upgrade Types
            /// </summary>
            public enum UpgradeType
            {
                /// <summary>
                /// rune upgrade
                /// </summary>
                Rune,

                /// <summary>
                /// Default upgrade
                /// </summary>
                Default,

                /// <summary>
                /// sigil upgrade
                /// </summary>
                Sigil,

                /// <summary>
                /// gem upgrade
                /// </summary>
                Gem,
            }

            /// <summary>
            /// upgrade type.
            /// </summary>
            [JsonProperty("type")]
            public UpgradeType Type
            {
                get;
                set;
            }

            /// <summary>
            /// suffix.
            /// </summary>
            [JsonProperty("suffix")]
            public string Suffix
            {
                get;
                set;
            }

            /// <summary>
            /// infusion upgrade type.
            /// </summary>
            [JsonProperty("infusion_upgrade_flags")]
            public IList<string> InfusionUpgradeType
            {
                get;
                set;
            }

            /// <summary>
            /// infix upgrade.
            /// </summary>
            [JsonProperty("infix_upgrade")]
            public InfixUpgradeInfo InfixUpgrade
            {
                get;
                set;
            }

            /// <summary>
            /// flags.
            /// </summary>
            [JsonProperty("flags")]
            public IList<UpgradeComponentType> Flags
            {
                get;
                set;
            }
        }

        [Serializable]
        public class TrophyInfo : ItemTypeInfo
        {

        }

        [Serializable]
        public class TrinketInfo : ItemTypeInfo
        {

            /// <summary>
            /// Trinket Types
            /// </summary>
            public enum TrinketType
            {
                /// <summary>
                /// Ring
                /// </summary>
                Ring,

                /// <summary>
                /// Accessory
                /// </summary>
                Accessory,

                /// <summary>
                /// Amulet
                /// </summary>
                Amulet,
            }

            /// <summary>
            /// Trinket type
            /// </summary>
            [JsonProperty("type")]
            public TrinketType Type
            {
                get;
                set;
            }

            private int _suffixId = -1;

            /// <summary>
            /// Item suffix id
            /// </summary>
            public int SuffixId
            {
                get
                {
                    if (_suffixId < 0) int.TryParse(_suffixIdString, out _suffixId);
                    return _suffixId;
                }
                set { _suffixId = value; }
            }

            [JsonProperty("suffix_item_id")]
            private string _suffixIdString { get; set; }

            /// <summary>
            /// infusion slots.
            /// </summary>
            [JsonProperty("infusion_slots")]
            public IList<InfusionSlotInfo> InfusionSlots
            {
                get;
                set;
            }

            /// <summary>
            /// infix upgrade.
            /// </summary>
            [JsonProperty("infix_upgrade")]
            public InfixUpgradeInfo InfixUpgrade
            {
                get;
                set;
            }
        }

        [Serializable]
        public class ToolInfo : ItemTypeInfo
        {

            /// <summary>
            /// Tool Types
            /// </summary>
            public enum ToolType
            {
                /// <summary>
                /// A salvage tool.
                /// </summary>
                Salvage,

                /// <summary>
                /// A logging tool.
                /// </summary>
                Logging,

                /// <summary>
                /// A foraging tool.
                /// </summary>
                Foraging,

                /// <summary>
                /// A mining tool.
                /// </summary>
                Mining,
            }

            /// <summary>
            /// Tool Type
            /// </summary>
            [JsonProperty("type")]
            public ToolType Type
            {
                get;
                set;
            }

            /// <summary>
            /// Number of charges
            /// </summary>
            public int Charges
            {
                get;
                set;
            }
        }

        [Serializable]
        public class GizmoInfo : ItemTypeInfo
        {
            /// <summary>
            /// Gizmo Types
            /// </summary>
            public enum GizmoType
            {
                /// <summary>
                /// Default gizmo.
                /// </summary>
                Default,

                /// <summary>
                /// A rentable contract npc.
                /// </summary>
                RentableContractNpc,

                /// <summary>
                /// An unlimited consumable.
                /// </summary>
                UnlimitedConsumable,
            }

            /// <summary>
            /// Gizmo Type.
            /// </summary>
            [JsonProperty("type")]
            public GizmoType Type
            {
                get;
                set;
            }
        }

        [Serializable]
        public class CraftingMaterialInfo : ItemTypeInfo
        {

        }

        [Serializable]
        public class ContainerInfo : ItemTypeInfo
        {

            /// <summary>
            /// Container Type
            /// </summary>
            public enum ContainerType
            {
                /// <summary>
                /// A default container.
                /// </summary>
                Default,

                /// <summary>
                /// A gift box.
                /// </summary>
                GiftBox
            }

            /// <summary>
            /// Container Type
            /// </summary>
            [JsonProperty("type")]
            public ContainerType Type
            {
                get;
                set;
            }
        }

        [Serializable]
        public class ConsumableInfo : ItemTypeInfo
        {
            /// <summary>
            /// Consumable type
            /// </summary>
            public enum ConsumableType
            {
                /// <summary>
                /// unlocks stuff
                /// </summary>
                Unlock,
                /// <summary>
                /// changes appearance
                /// </summary>
                AppearanceChange,
                /// <summary>
                /// summons a contract npc.
                /// </summary>
                ContractNpc,
                /// <summary>
                /// food consumable.
                /// </summary>
                Food,
                /// <summary>
                /// alcoholic beverage.
                /// </summary>
                Booze,
                /// <summary>
                /// generic consumable.
                /// </summary>
                Generic,
                /// <summary>
                /// halloween consumable.
                /// </summary>
                Halloween,
                /// <summary>
                /// immediate consumable.
                /// </summary>
                Immediate,
                /// <summary>
                /// transmutate items.
                /// </summary>
                Transmutation,
                /// <summary>
                /// utility consumable.
                /// </summary>
                Utility,
                /// <summary>
                /// Unknown Consumable
                /// </summary>
                Unknown,
            }

            /// <summary>
            /// Consumable type.
            /// </summary>
            [JsonProperty("type")]
            public ConsumableType Type
            {
                get;
                set;
            }
        }

        [Serializable]
        public class BagInfo : ItemTypeInfo
        {

            /// <summary>
            /// bag size.
            /// </summary>
            [JsonProperty("size")]
            public int Size
            {
                get;
                set;
            }

            /// <summary>
            /// Identifies if the bag can be sold.
            /// </summary>
            [JsonProperty("no_sell_or_sort")]
            [JsonConverter(typeof(BoolConverter))]
            public bool NoSellOrSort
            {
                get;
                set;
            }
        }

        [Serializable]
        public class BackInfo : ItemTypeInfo
        {
            private int _suffixId = -1;

            /// <summary>
            /// Item suffix id
            /// </summary>
            public int SuffixId
            {
                get
                {
                    if (_suffixId < 0) int.TryParse(_suffixIdString, out _suffixId);
                    return _suffixId;
                }
                set { _suffixId = value; }
            }

            [JsonProperty("suffix_item_id")]
            private string _suffixIdString { get; set; }


            /// <summary>
            /// infusion slots.
            /// </summary>
            [JsonProperty("infusion_slots")]
            public IList<InfusionSlotInfo> InfusionSlots
            {
                get;
                set;
            }

            /// <summary>
            /// infix upgrade.
            /// </summary>
            [JsonProperty("infix_upgrade")]
            public InfixUpgradeInfo InfixUpgrade
            {
                get;
                set;
            }
        }

        [Serializable]
        public class ArmorInfo : ItemTypeInfo
        {
            /// <summary>
            /// The stot the armor belongs to.
            /// </summary>
            public enum ArmourType
            {
                /// <summary>
                /// boots slot
                /// </summary>
                Boots,

                /// <summary>
                /// helm slot
                /// </summary>
                Helm,

                /// <summary>
                /// leggings slot
                /// </summary>
                Leggings,

                /// <summary>
                /// gloves slot
                /// </summary>
                Gloves,

                /// <summary>
                /// shoulders slot
                /// </summary>
                Shoulders,

                /// <summary>
                /// coat slot
                /// </summary>
                Coat,

                /// <summary>
                /// aquatic helmet slot
                /// </summary>
                HelmAquatic,
            }

            /// <summary>
            /// Specifies armour class.
            /// </summary>
            public enum ArmourClass
            {
                /// <summary>
                /// Clothing.
                /// </summary>
                Clothing,

                /// <summary>
                /// Light armour.
                /// </summary>
                Light,

                /// <summary>
                /// Medium armour.
                /// </summary>
                Medium,

                /// <summary>
                /// eavy armour.
                /// </summary>
                Heavy
            }

            /// <summary>
            /// Armour class
            /// </summary>
            [JsonProperty("weight_class")]
            public ArmourClass Class { get; set; }

            /// <summary>
            /// Armour type
            /// </summary>
            [JsonProperty("type")]
            public ArmourType Type { get; set; }

            private int _suffixId = -1;

            /// <summary>
            /// Item suffix id
            /// </summary>
            public int SuffixId
            {
                get
                {
                    if (_suffixId < 0) int.TryParse(_suffixIdString, out _suffixId);
                    return _suffixId;
                }
                set { _suffixId = value; }
            }

            [JsonProperty("suffix_item_id")]
            private string _suffixIdString { get; set; }


            /// <summary>
            /// Infusion slots.
            /// </summary>
            [JsonProperty("infusion_slots")]
            public IList<InfusionSlotInfo> InfusionSlots { get; private set; }

            /// <summary>
            /// Infix upgrade.
            /// </summary>
            [JsonProperty("infix_upgrade")]
            public InfixUpgradeInfo InfixUpgrade { get; private set; }

            /// <summary>
            /// Defense.
            /// </summary>
            [JsonProperty("defense")]
            public int Defense { get; private set; }
        }

        [Serializable]
        public class InfusionSlotInfo : ItemTypeInfo
        {
            /// <summary>
            /// item.
            /// </summary>
            [JsonProperty("item")]
            public string Item { get; set; }

            /// <summary>
            /// item flags.
            /// </summary>
            [JsonProperty("flags")]
            public IList<UpgradeType> Flags { get; set; }

            /// <summary>
            /// Type of upgrade
            /// <remarks>InfusionSlotInfo flag</remarks>
            /// </summary>
            public enum UpgradeType
            {
                /// <summary>
                /// Defense item.
                /// </summary>
                Defense,

                /// <summary>
                /// Offense item.
                /// </summary>
                Offense,

                /// <summary>
                /// Utility item.
                /// </summary>
                Utility,
            }

            public override string ToString()
            {
                if(Item != string.Empty)
                    return string.Format("{0}", string.Join(",", Flags));
                else return string.Format("{0}: {1}", Item, string.Join(",", Flags));
            }
        }

        [Serializable]
        public class InfixUpgradeInfo : ItemTypeInfo
        {
            /// <summary>
            /// attributes
            /// </summary>
            [JsonProperty("attributes")]
            public IList<ItemAttribute> Attributes { get; set; }

            /// <summary>
            /// buff
            /// </summary>
            [JsonProperty("buff")]
            public ItemBuff Buff { get; set; }

            /// <summary>
            /// item attribute
            /// </summary>
            [Serializable]
            public class ItemAttribute
            {
                /// <summary>
                /// Possible Item Attributes that can be modified
                /// </summary>
                public enum ModifiedAttribute
                {
                    /// <summary>
                    /// Critical damage.
                    /// </summary>
                    CritDamage,

                    /// <summary>
                    /// Condition damage.
                    /// </summary>
                    ConditionDamage,

                    /// <summary>
                    /// Healing.
                    /// </summary>
                    Healing,

                    /// <summary>
                    /// Vitality.
                    /// </summary>
                    Vitality,

                    /// <summary>
                    /// Power.
                    /// </summary>
                    Power,

                    /// <summary>
                    /// Toughness.
                    /// </summary>
                    Toughness,

                    /// <summary>
                    /// Precision.
                    /// </summary>
                    Precision,
                }

                /// <summary>
                /// attribute.
                /// </summary>
                [JsonProperty("attribute")]
                public ModifiedAttribute Attribute { get; set; }

                /// <summary>
                /// modifier.
                /// </summary>
                [JsonProperty("modifier")]
                public int Modifier { get; set; }

                public override string ToString()
                {
                    return string.Format("  {0}: {1}", Attribute, Modifier);
                }
            }

            [Serializable]
            public class ItemBuff
            {
                [JsonProperty("skill_id")]
                public int SkillId { get; set; }
                [JsonProperty("description")]
                public string Description { get; set; }

                public override string ToString()
                {
                    return Description;
                }
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("");
                foreach (var detail in Attributes)
                {
                    sb.AppendLine(detail.ToString());
                }
                sb.AppendFormat("{0}",Buff == null ? "" : Buff.ToString());
                return sb.ToString();
            }
        }

        #endregion Nested Classes
        [Serializable]
        public class GatheringInfo : ItemTypeInfo
        {
            /// <summary>
            /// Gathering Types
            /// </summary>
            public enum GatheringType
            {
                /// <summary>
                /// used for logging
                /// </summary>
                Logging,
                /// <summary>
                /// used for mining
                /// </summary>
                Mining,
                /// <summary>
                /// used for Foraging
                /// </summary>
                Foraging,
            }
            /// <summary>
            /// The gathering type
            /// </summary>
            [JsonProperty("type")]
            public GatheringType Type { get; set; }
        }
    }

}
