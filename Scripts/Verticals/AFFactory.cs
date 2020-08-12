using System;
using UnityEngine;
using Byjus.Gamepod.AbacusFTUE.Externals;

namespace Byjus.Gamepod.AbacusFTUE.Verticals {
    public class AFFactory {
        static AFIVisionService visionService;

        public static void SetVisionService(AFIVisionService visionService) {
            AFFactory.visionService = visionService;
            AFFactory.visionService.Init();
        }

        public static AFIVisionService GetVisionService() {
            return visionService;
        }
    }
}