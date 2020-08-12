using UnityEngine;
using Byjus.Gamepod.AbacusFTUE.Verticals;

namespace Byjus.Gamepod.AbacusFTUE.Views {

    public class AFGameManagerView : MonoBehaviour, AFIExtInputListener {
        public void OnAbacusValue(int value) {
            Debug.LogError("Abacus value received: " + value); 
        }
    }
}