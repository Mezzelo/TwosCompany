using System;
using System.Collections.Generic;
using daisyowl.text;
using TwosCompany.Helper;

namespace TwosCompany.ModBG {
    public class TCCredits : Route, OnInputPhase {

        public double? leaveTimer;

        private static Color header = Colors.textMain;

        private static Color item = Colors.textBold;

        private List<(Color, string)>? creditsCache;

        private string? localeBuiltFor;

        private Platform platformBuiltFor;

        private bool hasPressedFirstTime;
        private double current = 4.0;
        private double timePerCredit = 3.7;
        private int currentCredit = -1;

        public override bool GetShowCockpit() {
            return false;
        }

        public override bool GetShowCursor() {
            return currentCredit > creds.Count - 2;
        }

        public override bool GetShowCornerMenu() {
            return false;
        }

        public override MusicState? GetMusic(G g) {
            if (currentCredit == -1) {
                return new MusicState() {
                    scene = Song.SlowSilence,
                    combat = 1.0,
                };
            }
            return new MusicState() {
                scene = Song.Epilogue,
                combat = 1.0,
            };
        }

        private List<(string, string[])> creds = new List<(string, string[])> {
            ("TWO'S COMPANY", new string[] {}),
            ("A mod for COBALT CORE, by:", new string[] {"Rocket Rat Games"}),
            ("Art, design & programming by:", new string[] {"Mezzelo"}),
            ("made possible with modloaders courtesy of:", new string[] {
                "Ewanderer",
                "Shockah",
            }),
            ("with thanks to:", new string[] {
                "Arin/Action Martini",
                "TheJazMaster",
                "Shockah",
                "neozoid",
                "Kelsey",
                "SoggoruWaffle",
            }),
            ("thank you for playing.", new string[] {
                GetPressToContinueText(),
            }),

        };
        public void OnInputPhase(G g, Box b) {
            hasPressedFirstTime = hasPressedFirstTime || Input.GetGpDown(Btn.A, consume: false) || Input.GetGpDown(Btn.B) || Input.mouseLeftDown;
            if (hasPressedFirstTime && (Input.GetGpHeld(Btn.A) || Input.GetGpHeld(Btn.B) || Input.mouseLeft)) {
                if (!leaveTimer.HasValue) {
                    leaveTimer = 0.0;
                }

                leaveTimer += g.dt;
            }
            else {
                leaveTimer = null;
            }

            if (leaveTimer > 2.0) {
                LeaveScreen(g);
            }
        }

        public static string GetPressToContinueText() {
            return PlatformIcons.GetPlatform() switch {
                Platform.PS => Loc.T("credits.pressToContinue.ps", "(Hold X to continue)"),
                Platform.Xbox => Loc.T("credits.pressToContinue.xbox", "(Hold A to continue)"),
                Platform.NX => Loc.T("credits.pressToContinue.nx", "(Hold A to continue)"),
                _ => Loc.T("credits.pressToContinue", "(Click and hold to continue)"),
            };
        }

        private void LeaveScreen(G g) {
            Route nextRoute3 = (Route)(UnlockedDeck.MakeIfHasRewards(g.state) ?? ((object)new NewRunOptions()));
            g.state.ChangeRoute(() => nextRoute3);
        }

        public override void Render(G g) {
            current -= g.dt;
            if (current <= 0.0 && currentCredit < creds.Count - 1) {
                currentCredit++;
                current += timePerCredit;
            }
            double y = 35.0;
            Draw.Fill(Colors.black);
            g.Push(null, null, null, autoFocus: false, noHoverSound: false, gamepadUntargetable: false, ReticleMode.Quad, null, null, this);
            for (int n = 0; n < currentCredit + 1; n++) {
                Draw.Text(
                    creds[n].Item1,
                    240.0, y, font: null,
                    color: Colors.textMain,
                    align: TAlign.Center,
                    outline: Colors.black);
                for (int i = 0; i < creds[n].Item2.Count(); i++) {
                    y += 10.0;
                    Draw.Text(
                        creds[n].Item2[i],
                        240.0, y, font: null,
                        color: Colors.textBold,
                        align: TAlign.Center,
                        outline: Colors.black);
                }
                y += 15.0;
            }
            g.Pop();
            if (leaveTimer > 0.0) {
                Draw.Text($"{"Hold to exit"}{"......".AsSpan(0, (int)Math.Ceiling(6.0 * (leaveTimer.Value / 2.0)))}", 10.0, 254.0, null, Colors.textBold);
            }
        }
    }
}
