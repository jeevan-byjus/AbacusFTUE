using UnityEngine;
using TMPro;
using System;
using System.Collections;
using Osmo.Container.Common;

namespace Byjus.Gamepod.AbacusFTUE.Views {
    public class CharacterView : MonoBehaviour {
        [SerializeField] float typingSpeed = 1f;
        [SerializeField] AudioSource characterVoiceBox;
        [SerializeField] TextMeshPro textField;
        [SerializeField] AudioClip introAudio;
        [SerializeField] TutorialPrompt[] prompts;
        [SerializeField] Animator characterAnimator;

        ICharacterParent parent;

        private float typeDelay {
            get {
                float delay = Mathf.Abs(1f / typingSpeed);
                delay = Mathf.Clamp(delay, Time.deltaTime, 1f); // Nobody should be typing slower than 1 alphabet per second, and we cannot type faster than the frame-rate
                return delay;
            }
        }

        int currPromptInd;
        public TutorialPrompt CurrPrompt { get { return prompts[currPromptInd]; } }

        public void Init(ICharacterParent parent) {
            this.parent = parent;

            StartCoroutine(PlayIntro(() => {
                parent.OnCharacterLoopStarted();
                currPromptInd = 0;
                StartCoroutine(CoreLoop());
            }));
        }

        IEnumerator PlayIntro(Action onDone) {
            PlayCharacterAudio(introAudio);
            yield return new WaitForSeconds(introAudio.length + 1);
            onDone();
        }
        
        public void Resume() {
            StartCoroutine(CoreLoop());
        }

        public void Pause() {
            StopAllCoroutines();
            if (characterVoiceBox.isPlaying) {
                characterVoiceBox.Stop();
            }
            textField.text = "";
        }

        int currValue;
        public void OnAbacusValue(int value) {
            currValue = value;
        }

        IEnumerator CoreLoop() {
            for (; currPromptInd < prompts.Length; currPromptInd++) {
                var prompt = prompts[currPromptInd];

                var questionRoutine = StartCoroutine(RevealPrompt(prompt.questionPrompt));
                PlayCharacterAudio(prompt.questionAudio);
                yield return new WaitForSeconds(prompt.questionAudio.length);

                yield return new WaitUntil(() => { return currValue == prompt.answer; });

                PlayCharacterAudio(prompt.explanationAudio);
                StartCoroutine(RevealPrompt(prompt.explanationPrompt));
                yield return new WaitForSeconds(prompt.explanationAudio.length + 1);
            }

            parent.OnCharacterLoopDone();
        }

        void PlayCharacterAudio(AudioClip audio) {
            characterVoiceBox.clip = audio;
            characterVoiceBox.Play();
        }

        private IEnumerator RevealPrompt(string sentence) {
            for (int i = 0; i < sentence.Length; i++) {
                var partialSentence = sentence.Substring(0, i + 1);
                yield return new WaitForSeconds(typeDelay);
                textField.text = partialSentence;
            }
        }
    }

    public interface ICharacterParent {
        void OnCharacterLoopDone();
        void OnCharacterLoopStarted();
        void OnQuestionPhaseStarted();
        void OnExplanationPhaseStarted();
    }
}





