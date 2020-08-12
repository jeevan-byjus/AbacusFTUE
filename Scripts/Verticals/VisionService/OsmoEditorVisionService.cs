using Byjus.Gamepod.AbacusFTUE.Util;
using UnityEngine;
using Byjus.Gamepod.AbacusFTUE.Externals;
using Byjus.Gamepod.Common;
using Byjus.Gamepod.AbacusFTUE.Views;
using System.Linq;
using System.Collections.Generic;

#if !CC_STANDALONE

namespace Byjus.Gamepod.AbacusFTUE.Verticals {
    public class OsmoEditorVisionService : IVisionService {

        public ExtInput GetExtInput() {
            var objs = GameObject.FindObjectsOfType<EditorVisionObject>().ToList();
            var mimicObjs = VisionUtil.GetMimicItems(objs.ToList());

            var camDimens = CameraUtil.MainDimens();
            var ret = VisionUtil.ParseInput(mimicObjs, camDimens);
            Debug.LogError("Returning: " + ret);
            return ret;
        }

        public void Init() {

        }
    }
}
#endif