using CobaltCoreModding.Definitions.ExternalItems;
using Microsoft.Xna.Framework.Media;
using System;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss } )]
    public class BurdenOfHindsight : Artifact {
        public override string Description() => "All " + Manifest.JostColH + "Jost</c> cards deal 1 more damage while you have <c=status>OFF. STANCE</c>," +
            " and <c=card>Off Balance</c> costs 1 less energy. <c=downside>If you end your turn with any DEF. STANCE, lose all stance.</c>";

        public MilitiaArmband? armband;

        public override void OnReceiveArtifact(State state) {
            foreach (Artifact artifact in state.EnumerateAllArtifacts())
                if (artifact is CrumpledWrit writ)
                    writ.discountOffBalance = true;
                else if (artifact is MilitiaArmband armband)
                    this.armband = armband;
        }
        public override void OnRemoveArtifact(State state) {
            foreach (Artifact artifact in state.EnumerateAllArtifacts())
                if (artifact is CrumpledWrit writ)
                    writ.discountOffBalance = true;
                else if (artifact is MilitiaArmband armband)
                    this.armband = armband;
        }

        public override void OnTurnEnd(State state, Combat combat) {
            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
            ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
            ExternalStatus standFirm = Manifest.Statuses?["StandFirm"] ?? throw new Exception("status missing: standfirm");
            if (state.ship.Get((Status)defensiveStance.Id!) > 0 && state.ship.Get((Status)standFirm.Id!) <= 0) {
                this.Pulse();
                state.ship.Set((Status) defensiveStance.Id!, 0);
                state.ship.Set((Status) offensiveStance.Id!, 0);
                combat.QueueImmediate(new ADummyAction() {
                    timer = 0.0,
                    dialogueSelector = ".mezz_burdenOfHindsight",
                });
                Audio.Play(FSPRO.Event.Status_ShieldDown);
                if (armband != null)
                    armband.OnTurnEnd(state, combat);
            }
        }

        public override int ModifyBaseDamage(
          int baseDamage,
          Card? card,
          State state,
          Combat? combat,
          bool fromPlayer) {
            if (!fromPlayer || card == null)
                return 0;
            Deck? deck = card?.GetMeta()?.deck;
            ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
            return deck.GetValueOrDefault().ToString().Equals(ManifHelper.GetDeckId("jost").ToString()) && state.ship.Get((Status) offensiveStance.Id!) > 0 ? 1 : 0;
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { 
            new TTCard() {
                card = new OffBalance() { discount = -1 },
            }
        };
    }
}