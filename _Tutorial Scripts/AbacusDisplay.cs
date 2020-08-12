using System;
using UnityEngine;
using TMPro;

public class AbacusDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI onesDigitText; 
    [SerializeField] private TextMeshProUGUI tensDigitText; 
    [SerializeField] private TextMeshProUGUI hundredsDigitText; 
    [SerializeField] private TextMeshProUGUI onesExpandedText; 
    [SerializeField] private TextMeshProUGUI tensExpandedText; 
    [SerializeField] private TextMeshProUGUI hundredsExpandedText; 
    [SerializeField] private TextMeshProUGUI leftPlusSignText; 
    [SerializeField] private TextMeshProUGUI rightPlusSignText; 

    private Abacus abacus;
    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        abacus = FindObjectOfType<Abacus>();
    }

    public void DisplayTotal()
    {
        int total = abacus.Total;

        int ones = total % 10;            // 729 = 720 + 9
        total = total / 10;

        int tens = total % 10;            // 72 = 70 + 2
        total = total / 10;

        int hundreds = total % 10;          // 7
        total = total / 10;        // If nothing is wrong, at this stage the total should have been whittled down to zero

        onesDigitText.text = ones.ToString();
        tensDigitText.text = tens.ToString();
        hundredsDigitText.text = hundreds.ToString();

        onesExpandedText.text = ones.ToString();
        tensExpandedText.text = (tens*10).ToString();
        hundredsExpandedText.text = (hundreds*100).ToString();

        ShowOnlyAppropriateDigits();
    }

    private void ShowOnlyAppropriateDigits()
    {
        int total = abacus.Total;

        if(total >= 100)
        {
            hundredsDigitText.enabled = true;
            tensDigitText.enabled = true;
            onesDigitText.enabled = true;

            /*
            hundredsExpandedText.enabled = true;
            tensExpandedText.enabled = true;
            onesExpandedText.enabled = true;

            leftPlusSignText.enabled = true;
            rightPlusSignText.enabled = true;
            */
        }
        else if (total >= 10)
        {
            hundredsDigitText.enabled = false;
            tensDigitText.enabled = true;
            onesDigitText.enabled = true;

            /*
            hundredsExpandedText.enabled = false;
            tensExpandedText.enabled = true;
            onesExpandedText.enabled = true;

            leftPlusSignText.enabled = false;
            rightPlusSignText.enabled = true;
            */
        }
        else if (total > 0)
        {
            hundredsDigitText.enabled = false;
            tensDigitText.enabled = false;
            onesDigitText.enabled = true;

            /*
            hundredsExpandedText.enabled = false;
            tensExpandedText.enabled = false;
            onesExpandedText.enabled = true;

            leftPlusSignText.enabled = false;
            rightPlusSignText.enabled = false;
            */
        }
        else    // if(total == 0)
        {
            hundredsDigitText.enabled = false;
            tensDigitText.enabled = true;
            onesDigitText.enabled = false;

            tensDigitText.text = 0.ToString();       // A little gratuitous, but doesn't hurt

            /*
            hundredsExpandedText.enabled = true;
            tensExpandedText.enabled = true;
            onesExpandedText.enabled = true;

            leftPlusSignText.enabled = true;
            rightPlusSignText.enabled = true;
            */
        }
    }
}
