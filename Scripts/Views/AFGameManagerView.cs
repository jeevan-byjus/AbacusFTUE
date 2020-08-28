using UnityEngine;
using Byjus.Gamepod.AbacusFTUE.Verticals;

namespace Byjus.Gamepod.AbacusFTUE.Views {
    public class AFGameManagerView : MonoBehaviour, IAbacusFTUEParent, AFIExtInputListener {
        [SerializeField] AbacusFTUEView ftueView;

        public void Init() {
            ftueView.Init(this);
        }

        public void OnAbacusValue(int value) {
            ftueView.OnAbacusValue(value);
        }

        public void OnAbacusFTUEDone(bool finished) {
            // ftue done, exit maybe
            Application.Quit();
        }
    }


}