namespace TwosCompany.Zones {

    public class TCFinaleEvent : MapNodeContents {

        public string story = "mezz_Sorrel_TCFinaleEvent_1";
        public override void Render(G g, Vec v) {
            Draw.Sprite(Spr.map_question, v.x, v.y);
        }

        public override List<Tooltip> GetTooltips(G g) {
            return new List<Tooltip>
            {
            new TTText(Loc.T("map.node.mystery", "An unknown energy signature. It's sure to be something interesting..."))
        };
        }

        public override Route MakeRoute(State s) {
            return Dialogue.MakeDialogueRouteOrSkip(s, story, OnDone.map);
        }
    }
}
