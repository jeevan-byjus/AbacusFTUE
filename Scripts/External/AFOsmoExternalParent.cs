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
    public class AFOsmoExternalParent : OsmoGameBase {
        [SerializeField] TangibleManager mManager;
        [SerializeField] AFOsmoVisionService osmoVisionServiceView;
        [SerializeField] AFHierarchyManager hierarchyManager;

        [SerializeField] GameObject visionObjsParent;
        [SerializeField] Abacus abacusPrefab;
        [SerializeField] Abacus abacus;
        [SerializeField] Button showAbacusBtn;
        bool abacusVisible;

        const int maxFrames = 10;
        const float automaticCaptureDelay = 60f;
        const int automaticCaptureFrames = 5;
        const string captureFlags = "castlecreeps";
        const float captureWaitDelay = 10f;
        const float launchAutomaticCaptureDelay = 10f;

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
                Bridge.Helper.OnSettingsButtonClick += OnSettingsButtonClicked;
                Bridge.Helper.SetSettingsButtonVisibility(false);
                Bridge.Helper.SetVisionActive(true);

                // very hacky.. have to fix
                var tutorialShown = PlayerPrefs.GetInt("showed_tutorial", 0) == 1;
                if (!tutorialShown) {
                    var backBtn = GameObject.Find("arcade back btn");
                    if (backBtn != null) {
                        backBtn.gameObject.SetActive(false);
                    }
                }

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

                Bridge.Helper.Tangible.EnableCaptures(maxFrames);
                StartCoroutine(WaitAndLaunchAutomaticCapture());

            } else {
                Debug.LogWarning("[VisionTest] You are running without the Osmo bridge. No Osmo services will be loaded. Bridge.Helper will be null");
            }
        }

        void SetupUI() {
            abacus = Instantiate(abacusPrefab, visionObjsParent.transform);
            abacus.Init();
            abacusVisible = true;
            ToggleAbacus();

#if UNITY_EDITOR || BUILD_NO_ABACUS
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


        IEnumerator WaitAndLaunchAutomaticCapture() {
            yield return new WaitForSeconds(launchAutomaticCaptureDelay);

            SetAutomaticCaptureFlag(true);
        }

        IEnumerator AutomaticCapture() {
            Debug.Log(Time.time + " AFOsmoCaptureService: Automatic Capture");
            yield return new WaitForSeconds(automaticCaptureDelay);
            Capture(automaticCaptureFrames);
            yield return new WaitForSeconds(captureWaitDelay);

            StartCoroutine(AutomaticCapture());
        }

        public void SetAutomaticCaptureFlag(bool enabled) {
            if (enabled) {
                StartCoroutine(AutomaticCapture());
            } else {
                StopAllCoroutines();
            }
        }

        void Capture(int numFrames) {
            Debug.Log(Time.time + " AFOsmoCaptureService: Capture (" + numFrames + ")");
            Bridge.Helper.Tangible.SendCaptures(numFrames, captureFlags);
        }

        bool manualCaptureInProgress;

        public void ManualCapture() {
            Debug.Log(Time.time + " AFOsmoCaptureService: Manual Capture\nIn Progress: " + manualCaptureInProgress);
            if (manualCaptureInProgress) {
                Debug.LogError("Manual capture in progress");
                return;
            }

            StartCoroutine(ManualCapture(5));
        }

        IEnumerator ManualCapture(int numFrames) {
            Debug.Log(Time.time + " AFOsmoCaptureService: Manual Capture (" + numFrames + ")");
            manualCaptureInProgress = true;
            Capture(numFrames);
            yield return new WaitForSeconds(captureWaitDelay);
            manualCaptureInProgress = false;
        }
    }


}
#endif