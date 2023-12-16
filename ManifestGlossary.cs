using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using TwosCompany.Helper;
using TwosCompany.Cards.Nola;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Ilya;
using Microsoft.Extensions.Logging;
using HarmonyLib;
using System.Reflection;

namespace TwosCompany {
    public partial class Manifest : IGlossaryManifest, IStatusManifest {

        public static Dictionary<string, ExternalStatus> Statuses = new Dictionary<string, ExternalStatus>();
        public static Dictionary<string, ExternalGlossary> Glossary = new Dictionary<string, ExternalGlossary>();

        private void addStatus(string name, string displayName, string desc, bool isGood,
            System.Drawing.Color mainColor, System.Drawing.Color? borderColor, IStatusRegistry statReg, bool timeStop) {
            Statuses.Add(name, new ExternalStatus("Mezz.TwosCompany." + name, isGood,
                mainColor, borderColor.HasValue ? borderColor : null, Manifest.Sprites["Icon" + name], timeStop));
            Statuses[name].AddLocalisation(displayName, desc);
            statReg.RegisterStatus(Statuses[name]);
        }

        private void addGlossary(string name, string displayName, string desc, IGlossaryRegisty glossReg) {
            Glossary.Add(name, new ExternalGlossary("Mezz.TwosCompany." + name, name, false, ExternalGlossary.GlossayType.action, Manifest.Sprites["Icon" + name]));
            Glossary[name].AddLocalisation("en", displayName, desc);
            glossReg.RegisterGlossary(Glossary[name]);
        }
        public void LoadManifest(IStatusRegistry registry) {
            addStatus("TempStrafe", "Temp Strafe", "Fire for {0} damage immediately after every move you make. <c=downside>Goes away at start of next turn.</c>",
                true, System.Drawing.Color.Violet, System.Drawing.Color.FromArgb(unchecked((int)0xff5e5ce3)), registry, true);
            addStatus("MobileDefense", "Mobile Defense", "Whenever this ship moves, it gains {0} <c=status>TEMP SHIELD</c>. <c=downside>Decreases by 1 at end of turn.</c>",
                true, System.Drawing.Color.Cyan, null, registry, true);
            // addStatus("Outmaneuver", "Outmaneuver", "Gain {0} <c=status>EVADE</c> for every attack targeting your ship at the start of your turn.</c>",
            //     true, System.Drawing.Color.Cyan, null, registry, true);
            addStatus("Onslaught", "Onslaught", "Whenever you play a card this turn, draw a card of the <c=keyword>same color</c> from your draw pile." +
                " Decreases by 1 for each card drawn." +
                " <c=downside>Goes away at end of turn.</c>",
                true, System.Drawing.Color.Cyan, null, registry, true);
            // addStatus("Dominance", "Dominance", "Gain {0} <c=status>EVADE</c> each turn. If you don't hit your enemy before your turn ends, <c=downside>lose this status.</c>",
            //     true, System.Drawing.Color.FromArgb(unchecked((int)0x2F48B7)), null, registry, true);
            // addStatus("Repeat", "Encore", "Play your next card <c=keyword>an additional time</c>. Reduce this status by <c=keyword>1</c> for every card played.",
            //     true, System.Drawing.Color.Cyan, null, registry, true);
            // addStatus("Threepeat", "Encore B", "Play your next card <c=keyword>two more times</c>. Reduce this status by <c=keyword>1</c> for every card played.",
            //     true, System.Drawing.Color.Cyan, null, registry, true);
            addStatus("UncannyEvasion", "Damage Control", "Gain {0} <c=status>AUTODODGE</c> if you end your turn without any <c=status>SHIELD</c>, temporary or otherwise.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffff44b6)), null, registry, true);
            addStatus("FalseOpening", "False Opening", "Gain {0} <c=status>OVERDRIVE</c> whenever you receieve damage from an attack or missile this turn. " +
                "<c=downside>Goes away at start of next turn.</c>",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffff3838)), null, registry, true);
            addStatus("FalseOpeningB", "False Opening B", "Gain {0} <c=status>POWERDRIVE</c> whenever you receieve damage from an attack or missile this turn. " +
                "<c=downside>Goes away at start of next turn.</c>",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffffd33e)), System.Drawing.Color.FromArgb(unchecked((int)0xffff9e48)), registry, false);
            addStatus("Enflamed", "Enflamed", "Gain {0} <c=downside>HEAT</c> every turn.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffff6666)), null, registry, true);

        }
        public void LoadManifest(IGlossaryRegisty registry) {
            addGlossary("EnergyPerCard", "Urgent",
                "This card's cost increases by <c=downside>{0}</c> this turn for every other card played while in your hand. Resets when played, discarded, or when combat ends."
                , registry);
            addGlossary("EnergyPerPlay", "Rising Cost",
                "This card's cost increases by <c=downside>{0}</c> when played. Resets when discarded, or when combat ends."
                , registry);
            addGlossary("LowerCostHint", "Discount Other",
                "Lower a card's cost by <c=keyword>{0}</c> until played, or until combat ends."
                , registry);
            addGlossary("RaiseCostHint", "Expensive Other",
                "Raise a card's cost by <c=keyword>{0}</c> until played, or until combat ends."
                , registry);
            addGlossary("TurnIncreaseCost", "Timed Cost",
                "This card's cost increases by <c=keyword>{0}</c> every turn while held. Resets when played, discarded, or when combat ends."
                , registry);
            addGlossary("LowerPerPlay", "Lowering Cost",
                "This card's cost decreases by <c=keyword>{0}</c> when played. Resets when discarded, or when combat ends."
                , registry);
            addGlossary("AllIncrease", "Intensify",
                "All of this card's values increase by <c=keyword>{0}</c> when played. Resets when drawn, or when combat ends."
                , registry);
            addGlossary("AllIncreaseCombat", "Lasting Intensify",
                "All of this card's values increase by <c=keyword>{0}</c> when played. Resets <c=downside>when combat ends.</c>"
                , registry);
            addGlossary("PointDefense", "Point Defense",
                "Align your cannon {0} to the {1} hostile <c=drone>midrow object</c> over your ship. " +
                "If there are none, <c=downside>discard instead</c>. " +
                "Removes <c=cardtrait>retain for this turn when played."
                , registry);
            addGlossary("CallAndResponseHint", "Call and Response",
                "Store the selected card. Whenever you play this card, draw the stored card from the <c=keyword>draw or discard pile</c>{0}.\n" +
                "If the stored card was <c=cardtrait>exhausted</c> or <c=downside>single use</c>, choose another."
                , registry);
            addGlossary("ShieldCost", "Shield Cost",
                "Lose {0} <c=status>SHIELD</c>. If you don't have enough, this action does not happen."
                , registry);
            addGlossary("EvadeCost", "Evade Cost",
                "Lose {0} <c=status>EVADE</c>. If you don't have enough, this action does not happen."
                , registry);
            addGlossary("HeatCost", "Heat Cost",
                "Lose {0} <c=status>HEAT</c>. If you don't have enough, this action does not happen."
                , registry);
            addGlossary("DisguisedHint", "Disguised Card",
                "This card may actually be one or more <c=keyword>different</c> kinds of cards, and will not reveal itself until played."
                , registry);
            addGlossary("DisguisedPermaHint", "Permanent Disguise",
                "This card may actually be one or more <c=keyword>different</c> kinds of cards, and <c=downside>will not reveal itself even if played</c>."
                , registry);
        }
    }
}
