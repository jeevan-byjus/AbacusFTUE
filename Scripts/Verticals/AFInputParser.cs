using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Byjus.Gamepod.AbacusFTUE.Util;
using Byjus.Gamepod.Common.Abacus.Vision;

namespace Byjus.Gamepod.AbacusFTUE.Verticals {
    public class AFInputParser : MonoBehaviour {
        public AFIExtInputListener inputListener;
        AbacusReader reader;

        [SerializeField] Transform debugParent;
        [SerializeField] GameObject debugBeadPrefab;
        [SerializeField] GameObject debugIdealBeadPrefab;

        AFIVisionService visionService;
        int inputCount;

        int lastStableValue;
        int currValue;
        int numInvalidValues;
        int stabilityCount;
        const int valueStabilityThreshold = 1;
        const int invalidStabilityThreshold = 60;

        List<GameObject> debugActualObjects;
        List<GameObject> debugIdealObjects;

        public void Init() {
            visionService = AFFactory.GetVisionService();
            inputCount = 0;
            reader = new AbacusReader();
            debugActualObjects = new List<GameObject>();
            debugIdealObjects = new List<GameObject>();

            lastStableValue = -1;
            currValue = -1;
            stabilityCount = 0;
            numInvalidValues = 0;

            StartCoroutine(ListenForInput());
        }

        IEnumerator ListenForInput() {
            yield return new WaitForSeconds(AFConstants.INPUT_DELAY);
            inputCount++;

            var input = visionService.GetExtInput();
            if (input == null) {
                StartCoroutine(ListenForInput());
                yield break;
            }

            PrintDebugActualAbacus(input.abacus);
            int value = reader.Evaluate(input.abacus);
            PrintDebugIdealAbacus(reader.LastCalculatedMeta());

            StabilityCalculations2(value);
            inputListener.OnAbacusValue(value);

            StartCoroutine(ListenForInput());
        }

        // continuous occurences of a value for a certain threshold makes it stable
        // to account for slight inaccuracies, getting a -1 (no/invald abacus) is not counted for continuous occurences
        // but, when abacus is no longer infront of screen, it should make -1 as the stable value
        // so, a separate calculation for continuous -1s
        void StabilityCalculations2(int value) {
            if (value == -1) {
                numInvalidValues++;
                if (numInvalidValues >= invalidStabilityThreshold) {
                    lastStableValue = -1;
                    currValue = -1;
                }

            } else {
                numInvalidValues = 0;

                if (value != currValue) {
                    currValue = value;
                    stabilityCount = 1;
                } else {
                    stabilityCount++;
                    if (stabilityCount >= valueStabilityThreshold) {
                        lastStableValue = currValue;
                        stabilityCount = 0;
                    }
                }
            }
        }

        void StabilityCalculations(int value) {
            if (value == -1) {
                numInvalidValues++;
                if (numInvalidValues >= invalidStabilityThreshold) {
                    lastStableValue = -1;
                    currValue = -1;
                }

            } else {
                numInvalidValues = 0;

                if (value != currValue) {
                    currValue = value;
                    stabilityCount = 1;
                } else {
                    stabilityCount++;
                    if (stabilityCount >= valueStabilityThreshold) {
                        lastStableValue = currValue;
                        stabilityCount = 0;
                    }
                }
            }
        }

        void PrintDebugActualAbacus(ExtAbacus abacus) {
            if (abacus == null ||
               abacus.boardMarker == null ||
               abacus.bead5 == null || abacus.bead50 == null || abacus.bead500 == null ||
               abacus.beads1 == null || abacus.beads1.Count != 4 ||
               abacus.beads10 == null || abacus.beads10.Count != 4 ||
               abacus.beads100 == null || abacus.beads100.Count != 4) {

                return;
            }

            foreach (var obj in debugActualObjects) { Destroy(obj); }
            debugActualObjects.Clear();

            debugActualObjects.Add(CreateActualAtCanvasPosition(abacus.boardMarker.position));
            debugActualObjects.Add(CreateActualAtCanvasPosition(abacus.bead5.position));
            debugActualObjects.Add(CreateActualAtCanvasPosition(abacus.bead50.position));
            debugActualObjects.Add(CreateActualAtCanvasPosition(abacus.bead500.position));

            foreach (var bead in abacus.beads1) {
                debugActualObjects.Add(CreateActualAtCanvasPosition(bead.position));
            }
            foreach (var bead in abacus.beads10) {
                debugActualObjects.Add(CreateActualAtCanvasPosition(bead.position));
            }
            foreach (var bead in abacus.beads100) {
                debugActualObjects.Add(CreateActualAtCanvasPosition(bead.position));
            }
        }

        GameObject CreateActualAtCanvasPosition(Vector2 position) {
            var a = Instantiate(debugBeadPrefab, debugParent);
#if UNITY_EDITOR
            a.GetComponent<RectTransform>().position = position;
#else
            a.GetComponent<RectTransform>().anchoredPosition = position;
#endif
            return a;
        }

        GameObject CreateIdealAtCanvasPosition(Vector2 position) {
            position.x -= 5;
            var a = Instantiate(debugIdealBeadPrefab, debugParent);
#if UNITY_EDITOR
            a.GetComponent<RectTransform>().position = position;
#else
            a.GetComponent<RectTransform>().anchoredPosition = position;
#endif
            return a;
        }

        void PrintDebugIdealAbacus(AbacusParseMetaData abacus) {
            if (abacus == null) { return; }

            foreach (var obj in debugIdealObjects) { Destroy(obj); }
            debugIdealObjects.Clear();

            debugIdealObjects.Add(CreateIdealAtCanvasPosition(abacus.posBoardMarker));
            debugIdealObjects.Add(CreateIdealAtCanvasPosition(abacus.pos5));
            debugIdealObjects.Add(CreateIdealAtCanvasPosition(abacus.pos50));
            debugIdealObjects.Add(CreateIdealAtCanvasPosition(abacus.pos500));

            foreach (var pos in abacus.pos1s) {
                debugIdealObjects.Add(CreateIdealAtCanvasPosition(pos));
            }
            foreach (var pos in abacus.pos10s) {
                debugIdealObjects.Add(CreateIdealAtCanvasPosition(pos));
            }
            foreach (var pos in abacus.pos100s) {
                debugIdealObjects.Add(CreateIdealAtCanvasPosition(pos));
            }
        }
    }

    public interface AFIExtInputListener {
        void OnAbacusValue(int value);
    }

}