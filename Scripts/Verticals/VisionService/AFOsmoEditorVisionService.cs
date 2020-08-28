using Byjus.Gamepod.AbacusFTUE.Util;
using UnityEngine;
using Byjus.Gamepod.AbacusFTUE.Externals;
using Byjus.Gamepod.Common.Vision;
using Byjus.Gamepod.AbacusFTUE.Views;
using System.Linq;
using System.Collections.Generic;

#if !CC_STANDALONE

namespace Byjus.Gamepod.AbacusFTUE.Verticals {
    public class AFOsmoEditorVisionService : AFIVisionService {

        public AFExtInput GetExtInput() {
            var objs = GameObject.FindObjectsOfType<EditorVisionObject>().ToList();
            var mimicObjs = AFVisionUtil.GetMimicItems(objs.ToList());

            var camDimens = AFCameraUtil.MainDimens();
            var ret = AFVisionUtil.ParseInput(mimicObjs, camDimens);
            return ret;
        }

        public void Init() {

        }
    }
}
#endif