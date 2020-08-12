using UnityEngine;
using Byjus.Gamepod.Common;
using Byjus.Gamepod.AbacusFTUE.Util;
using Byjus.Gamepod.AbacusFTUE.Views;
using System.Linq;

namespace Byjus.Gamepod.AbacusFTUE.Verticals {
    public class AFStandaloneVisionService : AFIVisionService {

        public void Init() {

        }

        public AFExtInput GetExtInput() {
            var objs = GameObject.FindObjectsOfType<EditorVisionObject>().ToList();
            var mimicObjs = AFVisionUtil.GetMimicItems(objs.ToList());

            var camDimens = AFCameraUtil.MainDimens();
            var ret = AFVisionUtil.ParseInput(mimicObjs, camDimens);
            Debug.LogError("Returning: " + ret);
            return ret;
        }
    }
}