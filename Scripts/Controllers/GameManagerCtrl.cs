using UnityEngine;
using Byjus.Gamepod.AbacusFTUE.Views;
using Byjus.Gamepod.AbacusFTUE.Verticals;
using System.Collections.Generic;

namespace Byjus.Gamepod.AbacusFTUE.Controllers {
    public class GameManagerCtrl : IGameManagerCtrl, IExtInputListener {
        public IGameManagerView view;

        public void Init() {
        }

        public void OnAbacusValue(int value) {
            Debug.LogError("Abacus Value: " + value);
        }
    }

    public interface IGameManagerCtrl {
        void Init();
    }
}