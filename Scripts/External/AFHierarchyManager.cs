using UnityEngine;
using Byjus.Gamepod.AbacusFTUE.Views;
using Byjus.Gamepod.AbacusFTUE.Verticals;

namespace Byjus.Gamepod.AbacusFTUE.Externals {
    public class AFHierarchyManager : MonoBehaviour {
        [SerializeField] AFInputParser inputParser;
        [SerializeField] AFGameManagerView gameManager;

        public void Setup() {
            inputParser.inputListener = gameManager;
            inputParser.Init();

            gameManager.Init();
        }
    }
}