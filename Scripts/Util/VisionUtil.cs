using System;
using UnityEngine;
using Byjus.Gamepod.Common.Vision;
using Byjus.Gamepod.Common.Abacus.Vision;
using Byjus.Gamepod.Common;
using System.Collections.Generic;
using Byjus.Gamepod.AbacusFTUE.Verticals;
using System.Linq;
using Byjus.Gamepod.AbacusFTUE.Views;

namespace Byjus.Gamepod.AbacusFTUE.Util {
    public class VisionUtil {
        public static float CAMERA_ADJUST_X_RATIO = 0.8f;
        public static float CAMERA_ADJUST_Y_RATIO = 0.8f;

        public static float SW_EQUAL_POSITION_DIFF_PERCENT = 0.5f / 100;
        public static float SW_SAME_POINT_MOVED_DIFF_PERCENT = 30.0f / 100;

        public const int POSITION_ROUND_OFF_TO_DIGITS = 4;

        public const float INPUT_DELAY = 0.6f;

        public const int ABACUS_MARKER_ID = 0;
        public const int ABACUS_BEAD_1_ID = 1;
        public const int ABACUS_BEAD_5_ID = 5;
        public const int ABACUS_BEAD_10_ID = 10;
        public const int ABACUS_BEAD_50_ID = 50;
        public const int ABACUS_BEAD_100_ID = 100;
        public const int ABACUS_BEAD_500_ID = 500;
        public const int ARROW_ID = 11;
        public const int TRIGGER_ID = 12;
        public const int POWER_UP_RED = 13;
        public const int POWER_UP_BLUE = 14;
        public const int POWER_UP_GREEN = 15;

        public const string TYPE_ABACUS_MARKER = "abacus_frame";
        public const string TYPE_ABACUS_BEAD = "abacus_bead";
        public const string TYPE_DOMINO = "domino";

        public static string VecToString(Vector2 pos) {
            return "(" + pos.x + ", " + pos.y + ")";
        }

        public static Vector2 RoundedPos(Vector2 pos) {
            return new Vector2(Rounded(pos.x), Rounded(pos.y));
        }

        public static float Rounded(float x) {
            return (float) Math.Round(x, POSITION_ROUND_OFF_TO_DIGITS);
        }

        public static List<JItem> GetMimicItems(List<EditorVisionObject> editorObjs) {
            var ret = new List<JItem>();
            foreach (var obj in editorObjs) {
                var screenPos = obj.transform.position;
                var pos = Camera.main.ScreenToWorldPoint(screenPos);

                ret.Add(new JItem {
                    id = obj.ObjectID,
                    pt = new Point(pos.x, pos.y),
                    angle = obj.transform.rotation.z,
                });

            }

            return ret;
        }

        public static ExtInput ParseInput(List<JItem> jsonItems, Vector2 camDimens) {
            var abacus = ParseAbacus(jsonItems, camDimens);

            var ret = new ExtInput {
                abacus = abacus
            };

            return ret;
        }

        public static ExtAbacus ParseAbacus(List<JItem> jsonItems, Vector2 camDimens) {
            var ret = new ExtAbacus {
                beads100 = new List<ExtObject>(),
                beads10 = new List<ExtObject>(),
                beads1 = new List<ExtObject>(),
            };

            foreach (var obj in jsonItems) {
                var pos = new Vector2(obj.pt.x, obj.pt.y);
                var isAlive = Mathf.Abs(pos.x) < camDimens.x / 2 && Mathf.Abs(pos.y) < camDimens.y / 2;

                //if (!isAlive) {
                //    continue;
                //}

                switch (obj.id) {
                    case VisionUtil.ABACUS_MARKER_ID:
                        ret.boardMarker = new ExtObject { position = pos };
                        break;
                    case VisionUtil.ABACUS_BEAD_5_ID:
                        ret.bead5 = new ExtObject { position = pos };
                        break;
                    case VisionUtil.ABACUS_BEAD_1_ID:
                        ret.beads1.Add(new ExtObject { position = pos });
                        break;
                    case VisionUtil.ABACUS_BEAD_50_ID:
                        ret.bead50 = new ExtObject { position = pos };
                        break;
                    case VisionUtil.ABACUS_BEAD_10_ID:
                        ret.beads10.Add(new ExtObject { position = pos });
                        break;
                    case VisionUtil.ABACUS_BEAD_500_ID:
                        ret.bead500 = new ExtObject { position = pos };
                        break;
                    case VisionUtil.ABACUS_BEAD_100_ID:
                        ret.beads100.Add(new ExtObject { position = pos });
                        break;
                }
            }

            return ret;
        }

        public static int GetLaneIndex(Vector2 position, Vector2 camDimens) {
            var laneIndex = 0;
            var relX = position.x + camDimens.x / 2;
            var percent = relX / camDimens.x;

            if (percent < .33f) { } else if (percent < .67f) { laneIndex = 1; } else { laneIndex = 2; }
            return laneIndex;
        }
    }
}