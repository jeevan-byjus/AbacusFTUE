using System.Collections.Generic;
using UnityEngine;
using Byjus.Gamepod.Common.Abacus.Vision;
using Byjus.Gamepod.Common.Vision;
using Byjus.Gamepod.AbacusFTUE.Util;

namespace Byjus.Gamepod.AbacusFTUE.Verticals {
    /// <summary>
    /// This is the interface used by whichever class wants to read Vision Data
    /// Difference is it should mainly work with in-game models and shouldn't use anything platform dependent
    /// so, no vision related models or any other external platform related models
    /// </summary>
    public interface AFIVisionService {
        void Init();
        AFExtInput GetExtInput();
    }

    public class AFExtInput {
        public ExtAbacus abacus;

        public override string ToString() {
            return "Abacus: " + abacus;
        }
    }
}