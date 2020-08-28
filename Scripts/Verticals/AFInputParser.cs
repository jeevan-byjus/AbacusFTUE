using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Byjus.Gamepod.AbacusFTUE.Util;
using Byjus.Gamepod.Common.Abacus.Vision;

namespace Byjus.Gamepod.AbacusFTUE.Verticals {
    public class AFInputParser : MonoBehaviour {
        public AFIExtInputListener inputListener;
        AbacusReader reader;

        AFIVisionService visionService;
        int inputCount;

        int lastStableValue;
        int currValue;
        int numInvalidValues;
        int stabilityCount;
        const int valueStabilityThreshold = 1;
        const int invalidStabilityThreshold = 60;

        public void Init() {
            visionService = AFFactory.GetVisionService();
            inputCount = 0;
            reader = new AbacusReader();

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

            int value = reader.Evaluate(input.abacus);
            StabilityCalculations2(value);
            Debug.LogError("Read value: " + value + ", Stable value: " + lastStableValue);
            inputListener.OnAbacusValue(lastStableValue);

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
    }

    public interface AFIExtInputListener {
        void OnAbacusValue(int value);
    }

}