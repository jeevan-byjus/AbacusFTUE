using System;
using UnityEngine;
using TMPro;

namespace Byjus.Gamepod.AbacusFTUE.Views {
    public class AbacusDisplay : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI onesDigitText;
        [SerializeField] private TextMeshProUGUI tensDigitText;
        [SerializeField] private TextMeshProUGUI hundredsDigitText;
        [SerializeField] private TextMeshProUGUI onesExpandedText;
        [SerializeField] private TextMeshProUGUI tensExpandedText;
        [SerializeField] private TextMeshProUGUI hundredsExpandedText;
        [SerializeField] private TextMeshProUGUI leftPlusSignText;
        [SerializeField] private TextMeshProUGUI rightPlusSignText;

        public void Init() {

        }

        public void UpdateDisplay(int total) {
            int one = total % 10;
            int ten = (total / 10) % 10;
            int hundred = total / 100;

            onesDigitText.text = one.ToString();
            tensDigitText.text = ten.ToString();
            hundredsDigitText.text = hundred.ToString();

            onesExpandedText.text = one.ToString();
            tensExpandedText.text = (ten * 10).ToString();
            hundredsExpandedText.text = (hundred * 100).ToString();

            ShowOnlyAppropriateDigits(total);
        }

        private void ShowOnlyAppropriateDigits(int total) {
            if (total >= 100) {
                hundredsDigitText.enabled = true;
                tensDigitText.enabled = true;
                onesDigitText.enabled = true;

            } else if (total >= 10) {
                hundredsDigitText.enabled = false;
                tensDigitText.enabled = true;
                onesDigitText.enabled = true;

            } else if (total > 0) {
                hundredsDigitText.enabled = false;
                tensDigitText.enabled = false;
                onesDigitText.enabled = true;

            } else if (total == 0) {
                hundredsDigitText.enabled = false;
                tensDigitText.enabled = true; // this is so that the text appears in the center
                onesDigitText.enabled = false;

                tensDigitText.text = 0.ToString(); // A little gratuitous, but doesn't hurt

            }
        }
    }
}
