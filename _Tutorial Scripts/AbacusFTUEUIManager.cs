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

        void OnEnable()
        {
            AFGameManagerView.OnAbacusValueChanged += OnAbacusValueChanged;
            initiated = false;
            abacusHintSystem.gameObject.SetActive(false);
        }

        private void Start()
        {
            hintButton.onClick.AddListener(OnHintButtonClicked);
            resetButton.onClick.AddListener(OnResetButtonClicked);
        }

        void OnDisable()
        {
            AFGameManagerView.OnAbacusValueChanged -= OnAbacusValueChanged;
        }


        void OnAbacusValueChanged(int abacusValue)
        {
            if(abacusValue >= 0 && !initiated)
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
            
            if (currentAbacusReading != 0)
                textTyper.AskQuestion(0);
            else
                textTyper.AskQuestion(1);

        }
    }

    
}
