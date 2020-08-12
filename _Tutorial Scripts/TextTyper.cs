using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class TextTyper : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 1f;
    public float delayBetweenPrompts = 5f;
    [SerializeField] private AudioSource characterVoiceBox;
    [SerializeField] private TutorialPrompt[] prompts;
    private Abacus abacus;
    private int questionIndex;
    private TextMeshPro textField;

    private float typeDelay
    {
        get
        {
            float delay = Mathf.Abs(1f/typingSpeed);
            delay = Mathf.Clamp(delay, Time.deltaTime, 1f);     // Nobody should be typing slower than 1 alphabet per second, and we cannot type faster than the frame-rate
            return delay;
        }
    }

    private void Awake() 
    {
        textField = GetComponent<TextMeshPro>();
        abacus = FindObjectOfType<Abacus>();

        abacus.OnValueChanged += CheckAnswer;
        questionIndex =0;
    }

    public void TypeOut(string sentence)
    {
        StartCoroutine(RevealPrompt(sentence));
    }

    public void AskQuestion(int questionNumber)
    {
        TypeOut(prompts[questionNumber].questionPrompt);
        characterVoiceBox.clip = prompts[questionNumber].questionAudio;

        if(prompts[questionNumber].questionAudio != null)
        {
            characterVoiceBox.Play();
        }
    }

    public void GiveExplanation(int hintNumber)
    {
        TypeOut(prompts[hintNumber].explanationPrompt);
        characterVoiceBox.clip = prompts[hintNumber].explanationAudio;

        if(prompts[hintNumber].explanationAudio != null)
        {
            characterVoiceBox.Play();
        }
    }

    private IEnumerator RevealPrompt(string sentence)
    {
        string partialSentence = ""; 
        for(int i=0; i<sentence.Length; i++)
        {
            partialSentence = sentence.Substring(0,i+1);
            yield return new WaitForSeconds(typeDelay);
            textField.text = partialSentence;
        }
        yield return new WaitForSeconds(0f);
    }

    private string ColourNumber(int number)
    {
        //179
        string output = "";
        string onesColourHex = "<color=#FF3142>";       // reddish
        string tensColourHex = "<color=#00AC6D>";       // greenish
        string hundredsColourHex = "<color=#00BCC1>";   // blueish
        string endColour = "</color>";

        int ones = number % 10;
        int tens = (number/10) % 10;
        int hundreds = (number/100) % 10;

        if(number < 10)
        {
            output = "<b>" + onesColourHex + ones.ToString() + endColour + "</b>";
        }
        else if(number < 100)
        {
            output = "<b>" + tensColourHex + tens.ToString() + endColour +  onesColourHex + ones.ToString() + endColour +"</b>";
        }
        else
        {
            output = "<b>" +  hundredsColourHex + hundreds.ToString() + endColour + tensColourHex + tens.ToString() + endColour +  onesColourHex + ones.ToString() + endColour +"</b>";
        }

        return output;
    }

    private void CheckAnswer(object sender, EventArgs e)
    {
        if(abacus.Total == prompts[questionIndex].answer)
        {
            GiveExplanation(questionIndex);
            
            questionIndex++;
            Invoke("ShowNextQuestion", delayBetweenPrompts);

        }
    }

    void ShowNextQuestion()
    {
        AskQuestion(questionIndex);
    }
}
