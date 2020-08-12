using UnityEngine;
using System.Collections.Generic;
using Byjus.Gamepod.AbacusFTUE.Util;
using Byjus.Gamepod.AbacusFTUE.Views;
using System.Linq;

#if !CC_STANDALONE
using Osmo.SDK.VisionPlatformModule;
using Osmo.SDK.Vision;

namespace Byjus.Gamepod.AbacusFTUE.Verticals {

    /// <summary>
    /// Implementation of VisionService on iPad Osmo Build
    /// This is the class which connects with Vision and manages conversion from Vision to InGame models
    /// Shouldn't have any game logic here
    /// But, can contain accumulation, and any other parsing logic if required.
    /// sole purpose is to read Vision data, convert to in game models and send those when requested for
    /// </summary>
    public class OsmoVisionService : MonoBehaviour, IVisionService {
        string lastJson;
        BoundingBox visionBoundingBox;

        public void Init() {
            lastJson = "";
            visionBoundingBox = new BoundingBox(new List<Vector2> { new Vector2(-100, 90), new Vector2(100, 90), new Vector2(100, -200), new Vector2(-100, -200) });

            VisionConnector.Register(
                    apiKey: API.Key,
                    objectName: "OsmoVisionService",
                    functionName: "DispatchEvent",
                    mode: 152,
                    async: false,
                    hires: false
                );
        }

        public void DispatchEvent(string json) {
            if (json == null) { return; }
            lastJson = json;
        }

        public ExtInput GetExtInput() {
            if (string.IsNullOrEmpty(lastJson)) { return null; }

            Debug.LogError("Got json " + lastJson);

            var items = JsonUtility.FromJson<JOutput>(lastJson);
            var objs = items.items;

            var defective1Id = objs.FindIndex(x => x.id == VisionUtil.ABACUS_BEAD_1_ID && string.Equals(x.type, VisionUtil.TYPE_DOMINO));
            if (defective1Id != -1) { objs.RemoveAt(defective1Id); }

            var camDimens = new Vector2(visionBoundingBox.topWidth, visionBoundingBox.height);
            var abacus = VisionUtil.ParseAbacus(objs, camDimens);

            var ret = new ExtInput {
                abacus = abacus
            };

            return ret;
        }
    }
}
#endif