using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Byjus.Gamepod.Common.Abacus;
using UnityEngine.UI;

namespace Byjus.Gamepod.AbacusFTUE.Views {
    public class AbacusFTUEView : MonoBehaviour, ICharacterParent {
        [SerializeField] CanvasGroup questionsUiGroup;
        [SerializeField] CharacterView character;
        [SerializeField] Animator firstQuestionAnimator;
        [SerializeField] Animator questionUiAnimator;
        [SerializeField] Button hintButton;
        [SerializeField] Button resetButton;
        [SerializeField] AbacusDisplay display;
        [SerializeField] Abacus abacus;
        [SerializeField] Animator abacusHintSystem;

        bool introDone;
        bool coreLoopPaused;

        IAbacusFTUEParent parent;

        public void Init(IAbacusFTUEParent parent) {
            this.parent = parent;

            introDone = false;
            display.Init();
            character.Init(this);
            abacus.Init();
        }

        public void OnCharacterLoopStarted() {
            introDone = true;
            questionsUiGroup.alpha = 1;
            firstQuestionAnimator.SetTrigger("Disappear");
            questionUiAnimator.SetTrigger("Appear");
        }

        public void OnAbacusValue(int value) {
            if (!introDone) {
                return;
            }

            if (value == -1 && !coreLoopPaused) {
                character.Pause();
                firstQuestionAnimator.SetTrigger("Appear");
                questionUiAnimator.SetTrigger("Disappear");
                coreLoopPaused = true;

            } else if (value != -1 && coreLoopPaused) {
                coreLoopPaused = false;
                firstQuestionAnimator.SetTrigger("Disappear");
                questionUiAnimator.SetTrigger("Appear");
                character.Resume();
            }

            if (value >= 0) {
                character.OnAbacusValue(value);
                display.UpdateDisplay(value);
                abacus.SetValue(value);
            }
        }

        public void ShowHint() {
            var question = character.CurrPrompt.answer + "";
            abacusHintSystem.SetTrigger(question);
        }

        public void OnQuestionPhaseStarted() {
            hintButton.gameObject.SetActive(true);
            resetButton.gameObject.SetActive(true);
        }

        public void OnExplanationPhaseStarted() {
            hintButton.gameObject.SetActive(false);
            resetButton.gameObject.SetActive(false);
        }

        public void OnCharacterLoopDone() {
            parent.OnDone();
        }

    }

    public interface IAbacusFTUEParent {
        void OnDone();
    }

}
