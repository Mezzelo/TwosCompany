using System;
using TwosCompany;
using TwosCompany.Actions;

namespace TwosCompany.Midrow {
    public class Conduit : StuffBase {

        public enum ConduitType {
            normal = 0,
            kinetic = 1,
            feedback = 2,
            amplifier = 3,
            shield = 4,
        }

        public ConduitType condType = ConduitType.normal;
        public bool disabled = false;

        public override Spr? GetIcon() {
            string sprite = "IconConduit";
            if (condType == ConduitType.kinetic)
                sprite += "Kinetic";
            else if (condType == ConduitType.feedback)
                sprite += "Feedback";
            else if (condType == ConduitType.shield)
                sprite += "Shield";
            else if (condType == ConduitType.amplifier)
                sprite += "Amplifier";
            return (Spr)(Manifest.Sprites[sprite]?.Id ?? throw new Exception("missing conduit icon"));
        }
        public override List<CardAction>? GetActions(State s, Combat c) {
            if (condType == ConduitType.normal || condType == ConduitType.shield || !disabled)
                return null;
            return new List<CardAction>() {
                new AEnableConduit() {
                    conduit = this,
                }
            };
        }

        public override string GetDialogueTag() => "conduit";

        public override double GetWiggleAmount() => 0.5;

        public override double GetWiggleRate() => 1;

        public override List<Tooltip> GetTooltips() {
            List<Tooltip> tooltipList = new List<Tooltip>();
            string sprite = "Conduit";
            if (condType == ConduitType.kinetic)
                sprite += "Kinetic";
            else if (condType == ConduitType.feedback)
                sprite += "Feedback";
            else if (condType == ConduitType.shield)
                sprite += "Shield";
            else if (condType == ConduitType.amplifier)
                sprite += "Amplifier";

            tooltipList.Add(new TTGlossary(
                Manifest.Glossary[sprite].Head));
            List<Tooltip> tooltips = tooltipList;
            if (this.bubbleShield)
                tooltips.Add(new TTGlossary("midrow.bubbleShield"));
            return tooltips;
        }

        public override void Render(G g, Vec v) {
            string sprite = "DroneConduit";
            if (condType == ConduitType.kinetic) {
                sprite += "Kinetic";
                if (disabled)
                    sprite += "Disabled";
            }
            else if (condType == ConduitType.feedback) {
                sprite += "Feedback";
                if (disabled)
                    sprite += "Disabled";
            }
            else if (condType == ConduitType.shield) {
                sprite += "Shield";
                if (disabled)
                    sprite += "Disabled";
            }
            else if (condType == ConduitType.amplifier)
                sprite += "Amplifier";
            this.DrawWithHilight(g,
                (Spr)(Manifest.Sprites[sprite]?.Id ?? throw new Exception("missing conduit sprite")),
                v + this.GetOffset(g), false, false
            );
        }
    }
}