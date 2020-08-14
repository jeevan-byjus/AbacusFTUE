using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Byjus.Gamepod.AbacusFTUE.Views
{
    public class AbacusFTUEUIManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup questionsUiGroup;
        [SerializeField] TextTyper textTyper;
        [SerializeField] Animator firstQuestionAnimator;
        [SerializeField] Animator questionUiAnimator;
        [SerializeField] AbacusHintSystem abacusHintSystem;
        [SerializeField] Button hintButton;
        [SerializeField] Button resetButton;
        private bool initiated = false;
        private int currentAbacusQuestion;
        private int currentAbacusReading = 0;
        [SerializeField] AudioSource ftueAudioSource;

        private float delayAbacusReading = 0;

        void OnEnable()
        {
            delayAbacusReading = 0;
            AFGameManagerView.OnAbacusValueChanged += OnAbacusValueChanged;
            initiated = false;
            abacusHintSystem.gameObject.SetActive(false);
            TextTyper.questionOrExplanation += QuestionOrExplanation;
            
        }

        private void Start()
        {
            hintButton.onClick.AddListener(OnHintButtonClicked);
            resetButton.onClick.AddListener(OnResetButtonClicked);
        }

        void OnDisable()
        {
            AFGameManagerView.OnAbacusValueChanged -= OnAbacusValueChanged;
            TextTyper.questionOrExplanation -= QuestionOrExplanation;
        }

        void OnAbacusValueChanged(int abacusValue)
        {
            if(abacusValue >= 0 && !initiated && delayAbacusReading >=3)
            {
                if (!ftueAudioSource.isPlaying)
                {
                    initiated = true;
                    textTyper.DisableQuestions();
                    questionsUiGroup.alpha = 1;
                    currentAbacusReading = abacusValue;
                    firstQuestionAnimator.SetTrigger("Dissapear");
                    questionUiAnimator.SetTrigger("Appear");
                    Invoke("StartAskingQuestions", 1.5f);
                }
            }
        }

        void OnHintButtonClicked()
        {
            currentAbacusQuestion = textTyper.prompts[textTyper.questionIndex].answer;
            abacusHintSystem.gameObject.SetActive(true);
            abacusHintSystem.GetComponent<Animator>().SetTrigger(currentAbacusQuestion.ToString());
        }

        void OnResetButtonClicked()
        {
            abacusHintSystem.gameObject.SetActive(false);
        }

        void StartAskingQuestions()
        {
            textTyper.AskQuestion(0);
        }

        private void Update()
        {
            delayAbacusReading += Time.deltaTime;
            
        }

        void QuestionOrExplanation(bool questioning)
        {
            if (!questioning)
            {
                hintButton.gameObject.SetActive(false);
                resetButton.gameObject.SetActive(false);

            }
            else
            {
                hintButton.gameObject.SetActive(true);
                resetButton.gameObject.SetActive(true);
            }
        }
    }

    
}
