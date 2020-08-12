﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Byjus.Gamepod.AbacusFTUE.Util;
using Byjus.Gamepod.Common.Abacus.Vision;

namespace Byjus.Gamepod.AbacusFTUE.Verticals {
    public class InputParser : MonoBehaviour {
        public IExtInputListener inputListener;
        AbacusReader reader;

        IVisionService visionService;
        int inputCount;

        int lastStableValue;
        int currValue;
        int stabilityCount;
        const int valueStabilityThreshold = 1;

        public void Init() {
            visionService = Factory.GetVisionService();
            inputCount = 0;
            reader = new AbacusReader();

            lastStableValue = -1;
            currValue = -1;
            stabilityCount = 0;

            StartCoroutine(ListenForInput());
        }

        IEnumerator ListenForInput() {
            yield return new WaitForSeconds(Constants.INPUT_DELAY);
            inputCount++;


            var input = visionService.GetExtInput();
            if (input == null) {
                StartCoroutine(ListenForInput());
                yield break;
            }

            int value = reader.Evaluate(input.abacus);
            StabilityCalculations(value);
            inputListener.OnAbacusValue(value);

            StartCoroutine(ListenForInput());
        }

        void StabilityCalculations(int value) {
            if (value != currValue && value != -1) {
                currValue = value;
                stabilityCount = 1;
            } else if (value == currValue) {
                stabilityCount++;
                if (stabilityCount >= valueStabilityThreshold) {
                    lastStableValue = currValue;
                    stabilityCount = 0;
                }
            }
        }
    }

    public interface IExtInputListener {
        void OnAbacusValue(int value);
    }

}