using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Byjus.Gamepod.AbacusFTUE.Verticals;
using Byjus.Gamepod.AbacusFTUE.Util;
using Byjus.Gamepod.Common.Abacus;
using UnityEngine.UI;
#if !CC_STANDALONE
using Osmo.SDK;
using Osmo.Container.Common;
using Osmo.SDK.Internal;

namespace Byjus.Gamepod.AbacusFTUE.Externals {

    /// <summary>
    /// The top most parent in game hierarchy in case the setup is for Osmo
    /// </summary>
    public class AFOsmoExternalParent : OsmoGameBase 
    {
        [SerializeField] TangibleManager mManager;
        [SerializeField] AFOsmoVisionService osmoVisionServiceView;
        [SerializeField] AFHierarchyManager hierarchyManager;

        [SerializeField] GameObject visionObjsParent;
        [SerializeField] Abacus abacusPrefab;
        [SerializeField] Abacus abacus;
        [SerializeField] Button showAbacusBtn;
        bool abacusVisible;

        public Vector2 GetCameraDimens() {
            return new Vector2(TangibleCamera.Width, TangibleCamera.Height);
        }

        void AssignRefs() {
            mManager = FindObjectOfType<TangibleManager>();
            osmoVisionServiceView = FindObjectOfType<AFOsmoVisionService>();
            hierarchyManager = FindObjectOfType<AFHierarchyManager>();
        }

        protected override void GameStart() {
            if (Bridge != null) {
                Bridge.Helper.SetOnMainMenuScreen(false);
                Bridge.Helper.OnSettingsButtonClick += OnSettingsButtonClicked;
                Bridge.Helper.SetSettingsButtonVisibility(false);
                Bridge.Helper.SetVisionActive(true);
                Bridge.Helper.SetOsmoWorldStickersAllowed(true);

                Debug.LogError("GameStart called");
                AssignRefs();
                SetupUI();

#if UNITY_EDITOR
                AFFactory.SetVisionService(new AFOsmoEditorVisionService());
#elif BUILD_NO_ABACUS
                AFFactory.SetVisionService(new AFOsmoEditorVisionService());
#else
                AFFactory.SetVisionService(osmoVisionServiceView);
#endif
                hierarchyManager.Setup();

            } else {
                Debug.LogWarning("[VisionTest] You are running without the Osmo bridge. No Osmo services will be loaded. Bridge.Helper will be null");
            }
        }

        void SetupUI() {
            abacus = Instantiate(abacusPrefab, visionObjsParent.transform);
            abacus.Init();
            abacusVisible = true;
            ToggleAbacus();

#if BUILD_NO_ABACUS
            showAbacusBtn.gameObject.SetActive(true);
#else
            showAbacusBtn.gameObject.SetActive(false);
#endif
        }

        private void Update() {
            if (Input.GetKeyUp(KeyCode.X)) {
                ToggleAbacus();
            }
        }

        public void ToggleAbacus() {
            abacusVisible = !abacusVisible;
            abacus.gameObject.SetActive(abacusVisible);
        }

        void OnSettingsButtonClicked() {
            Debug.LogWarning("Settings Clicked");
        }

     
    }

    
}
#endif