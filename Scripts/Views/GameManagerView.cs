using UnityEngine;
using Byjus.Gamepod.AbacusFTUE.Controllers;

namespace Byjus.Gamepod.AbacusFTUE.Views {

    public class GameManagerView : MonoBehaviour, IGameManagerView {
        public IGameManagerCtrl ctrl;
    }

    public interface IGameManagerView {
        
    }
}