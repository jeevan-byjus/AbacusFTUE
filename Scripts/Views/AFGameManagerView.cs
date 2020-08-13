using UnityEngine;
using Byjus.Gamepod.AbacusFTUE.Verticals;
using UnityEngine.UI;

namespace Byjus.Gamepod.AbacusFTUE.Views {

    public class AFGameManagerView : MonoBehaviour, AFIExtInputListener {

        public delegate void  AbacusValue(int x);
        public static event AbacusValue OnAbacusValueChanged;

        public void OnAbacusValue(int value) {
            Debug.LogError("Abacus value received: " + value);
            if(OnAbacusValueChanged != null)
            {
                OnAbacusValueChanged(value);
            }
        }
    }

 
}