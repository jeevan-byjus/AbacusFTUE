using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Byjus.Gamepod.AbacusFTUE.Views
{
    public class AbacusFTUEUIManager : MonoBehaviour
    {
        public CanvasGroup questionsUiGroup;
        public TextTyper textTyper;
        public Animator firstQuestionAnimator;
        public Animator questionUiAnimator;
        private bool intiated = false;
        void OnEnable()
        {
            AFGameManagerView.OnAbacusValueChanged += OnAbacusValueChanged;
            intiated = false;
        }

        void OnDisable()
        {
            AFGameManagerView.OnAbacusValueChanged -= OnAbacusValueChanged;
        }


        void OnAbacusValueChanged(int abacusValue)
        {
            if(abacusValue >= 0 && !intiated)
            {
                intiated = true;
                questionsUiGroup.alpha = 1;
                if(abacusValue != 0)
                    textTyper.AskQuestion(0);
                else
                    textTyper.AskQuestion(1);

                firstQuestionAnimator.SetTrigger("Dissapear");
                questionUiAnimator.SetTrigger("Appear");
            }
        }
    }

    
}
