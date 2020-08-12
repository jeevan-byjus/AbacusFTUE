using Byjus.Gamepod.AbacusFTUE.Verticals;
using UnityEngine;

#if CC_STANDALONE
namespace Byjus.Gamepod.Template.Externals {

    /// <summary>
    /// The top most parent in view hierarchy in case we are running standalone
    /// </summary>
    public class AFStandaloneExternalParent : MonoBehaviour {
        public AFHierarchyManager hierarchyManager;

        void AssignRefs() {
            hierarchyManager = FindObjectOfType<AFHierarchyManager>();
        }

        private void Start() {
            AssignRefs();
            Factory.SetVisionService(new AFStandaloneVisionService());
            hierarchyManager.Setup();
        }
    }
}
#endif