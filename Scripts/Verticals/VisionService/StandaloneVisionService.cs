using UnityEngine;
using Byjus.Gamepod.Common;
using Byjus.Gamepod.AbacusFTUE.Util;
using Byjus.Gamepod.AbacusFTUE.Views;
using System.Linq;

namespace Byjus.Gamepod.AbacusFTUE.Verticals {
    public class StandaloneVisionService : IVisionService {

        public void Init() {

        }

        public ExtInput GetExtInput() {
            var objs = GameObject.FindObjectsOfType<EditorVisionObject>().ToList();
            var mimicObjs = VisionUtil.GetMimicItems(objs.ToList());

            var camDimens = CameraUtil.MainDimens();
            var ret = VisionUtil.ParseInput(mimicObjs, camDimens);
            Debug.LogError("Returning: " + ret);
            return ret;
        }
    }
}