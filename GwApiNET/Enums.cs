using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GwApiNET
{
    /// <summary>
    /// Color Item Catagory
    /// </summary>
    public enum ColorItemType
    {
        Cloth,
        Leather,
        Metal,
    }
    /// <summary>
    /// Event States
    /// </summary>
    public enum EventState
    {
        [Description("The event is running now")]
        Active,
        [Description("The event is not running now")]
        Inactive,
        [Description("The event has succeeded")]
        Success,
        [Description("The event has failed")]
        Fail,
        [Description("The event is waiting for certain criteria to be met before activating")]
        Warmup,
        [Description("The criteria for the event to start have been met, but certain activities (such as an NPC dialogue) have not completed yet. After the activites have been completed, the event will become Active.")]
        Preparation,
    }

    /// <summary>
    /// Game Types
    /// </summary>
    public enum GameType
    {
        Activity,
        Dungeon,
        PvE,
        PvP,
        PvpLobby,
        WvW,
    }

    /// <summary>
    /// Item Types
    /// </summary>
    public enum ItemType
    {
        Armor,
        Back,
        Bag,
        Consumable,
        Container,
        CraftingMaterial,
        Gathering,
        Gizmo,
        MiniPet,
        Tool,
        Trinket,
        Trophy,
        UpgradeComponent,
        Weapon
    }
    /// <summary>
    /// Item Rarity Types
    /// </summary>
    public enum RarityType
    {
        Junk,
        Basic,
        Fine,
        Masterwork,
        Rare,
        Exotic,
        Ascended,
        Legendary
    }
    

    /// <summary>
    /// Response Language Identification
    /// </summary>
    public enum Language
    {
        [Language("")]
        None,
        [Language("en")]
        English,
        [Language("fr")]
        French,
        [Language("de")]
        German,
        [Language("es")]
        Spanish,
    }
    /// <summary>
    /// Point of Interest Types
    /// </summary>
    public enum PointOfInterestType
    {
        Vista,
        Unlock,
        Landmark,
        Waypoint,
    }
    /// <summary>
    /// Owner Colors
    /// Identifies Teams
    /// </summary>
    public enum OwnerColor
    {
        Red,
        Green,
        Blue,
        Neutral,
    }
    /// <summary>
    /// Match Map Types
    /// </summary>
    public enum MatchMapType
    {
        RedHome,
        GreenHome,
        BlueHome,
        Center,
    }
    /// <summary>
    /// Discipline Types
    /// </summary>
    public enum DisciplineType
    {
        Leatherworker,
        Artificer,
        Huntsman,
        Weaponsmith,
        Armorsmith,
        Tailor,
        Jeweler,
        Chef,

    }

    /// <summary>
    /// Recipe Types
    /// </summary>
    public enum RecipeType
    {
        /// <summary>
        /// a coat recipe
        /// </summary>
        Coat,

        /// <summary>
        /// a leggings recipe
        /// </summary>
        Leggings,

        /// <summary>
        /// a refinement recipe
        /// </summary>
        Refinement,

        /// <summary>
        /// an insignia recipe
        /// </summary>
        Insignia,

        /// <summary>
        /// a bulk recipe
        /// </summary>
        Bulk,

        /// <summary>
        /// a component recipe
        /// </summary>
        Component,

        /// <summary>
        /// an inscription recipe
        /// </summary>
        Inscription,

        /// <summary>
        /// a boots recpie
        /// </summary>
        Boots,

        /// <summary>
        /// an upgrade component recipe
        /// </summary>
        UpgradeComponent,

        /// <summary>
        /// a glove recipe
        /// </summary>
        Gloves,

        /// <summary>
        /// a helm recipe
        /// </summary>
        Helm,

        /// <summary>
        /// an axe recipe
        /// </summary>
        Axe,

        /// <summary>
        /// a dagger recipe
        /// </summary>
        Dagger,

        /// <summary>
        /// a hammer recipe
        /// </summary>
        Hammer,

        /// <summary>
        /// a greatsword recipe
        /// </summary>
        Greatsword,

        /// <summary>
        /// a mace recipe
        /// </summary>
        Mace,

        /// <summary>
        /// a shield recipe
        /// </summary>
        Shield,

        /// <summary>
        /// a sword recipe
        /// </summary>
        Sword,

        //// Huntsman

        /// <summary>
        /// a harpoon recipe
        /// </summary>
        Harpoon,

        /// <summary>
        /// a long bow recipe
        /// </summary>
        LongBow,

        /// <summary>
        /// a pistol recipe
        /// </summary>
        Pistol,

        /// <summary>
        /// a rifle recipe
        /// </summary>
        Rifle,

        /// <summary>
        /// a short bow recipe
        /// </summary>
        ShortBow,

        /// <summary>
        /// a speargun recipe
        /// </summary>
        Speargun,

        /// <summary>
        /// a torch recipe
        /// </summary>
        Torch,

        /// <summary>
        /// a warhorn recipe
        /// </summary>
        Warhorn,

        //// Artificer

        /// <summary>
        /// a focus recipe
        /// </summary>
        Focus,

        /// <summary>
        /// a potion recipe
        /// </summary>
        Potion,

        /// <summary>
        /// a scepter recipe
        /// </summary>
        Scepter,

        /// <summary>
        /// a staff recipe
        /// </summary>
        Staff,

        /// <summary>
        /// a trident recipe
        /// </summary>
        Trident,

        //// Chef

        /// <summary>
        /// a dessert recipe
        /// </summary>
        Dessert,

        /// <summary>
        /// a dye recipe
        /// </summary>
        Dye,

        /// <summary>
        /// a feast recipe
        /// </summary>
        Feast,

        /// <summary>
        /// a cooking ingredient recipe
        /// </summary>
        IngredientCooking,

        /// <summary>
        /// a meal recipe
        /// </summary>
        Meal,

        /// <summary>
        /// a snack recipe
        /// </summary>
        Snack,

        /// <summary>
        /// a soup recipe
        /// </summary>
        Soup,

        /// <summary>
        /// a seasoning recipe
        /// </summary>
        Seasoning,

        /// <summary>
        /// an amulet recipe
        /// </summary>
        Amulet,

        /// <summary>
        /// an earring recipe
        /// </summary>
        Earring,

        /// <summary>
        /// a ring recipe
        /// </summary>
        Ring,

        /// <summary>
        /// a shoulder recipe
        /// </summary>
        Shoulders,

        /// <summary>
        /// a bag recipe
        /// </summary>
        Bag,

        /// <summary>
        /// a consumable recipe
        /// </summary>
        Consumable,
        /// <summary>
        /// Unknown Recipe
        /// </summary>
        Unknown,
    }

    /// <summary>
    /// Character Profession
    /// </summary>
    public enum Profession
    {
        Guardian = 1,
        Warrior = 2,
        Engineer = 3,
        Ranger = 4,
        Thief = 5,
        Elementalist = 6,
        Mesmer = 7,
        Necromancer = 8
    }

    /// <summary>
    /// PvP Team Color
    /// </summary>
    public enum TeamColor
    {
        /*
        None = 0
        Blue Rose = 9
        Salmon = 376
        Crisp Mint = 55
        */
        None = 0,
        Red = 376,
        Blue = 9,
        Green = 55
    }
}
