using System;
using System.Collections.Generic;
using UnityEngine;

namespace Byjus.Gamepod.AbacusFTUE.Views
{
    public class Abacus : MonoBehaviour
    {
        public event EventHandler OnValueChanged;
        private List<AbacusKey> keys;
        [SerializeField] private List<AbacusKey> onesKeys;
        [SerializeField] private AbacusKey fiveKey;
        [SerializeField] private List<AbacusKey> tensKeys;
        [SerializeField] private AbacusKey fiftyKey;
        [SerializeField] private List<AbacusKey> hundredsKeys;
        [SerializeField] private AbacusKey fiveHundredKey;
        private AbacusDisplay display;

        public int Total
        {
            get
            {
                int total = 0;
                foreach (AbacusKey key in keys)
                {
                    if (key.engaged)
                    {
                        total += key.Value;
                    }
                }
                return total;
            }

            set
            {
                
                ResetKeys();
                int total = value;
                Debug.LogError(total);
                
                if (total >= 500)
                {
                    fiveHundredKey.SetKey(true);
                    total -= 500;
                }

                if (total != 0 && total / 100f >= 1f)
                {
                    int count = total / 100;
                    for (int i = 0; i < count; i++)
                    {
                        hundredsKeys[i].SetKey(true);
                        total -= 100;
                    }
                }

                if (total >= 50)
                {
                    fiftyKey.SetKey(true);
                    total -= 50;
                }
                if (total != 0 && total / 10f >= 1f)
                {
                    Debug.LogError(total);
                    int count = total / 10;
                    for (int i = 0; i < count; i++)
                    {
                        tensKeys[i].SetKey(true);
                        total -= 10;
                    }
                }

                if (total >= 5)
                {
                    fiveKey.SetKey(true);
                    total -= 5;
                }
                if (total >= 1)
                {
                    int count = total;
                    for (int i = 0; i < count; i++)
                    {
                        onesKeys[i].SetKey(true);
                        total -= 1;
                    }
                }

                UpdateAbacusDisplay(this, EventArgs.Empty);
            }
        }
        private float AbacusYPosition
        {
            get
            {
                return transform.position.y;
            }
            set
            {
                transform.position = new Vector3(transform.position.x, value, transform.position.z);
            }
        }
        private void Awake()
        {
            FindAbacusKeys();
            ResetKeys();
            display = FindObjectOfType<AbacusDisplay>();
            display.Initialize();
        }

        private void FindAbacusKeys()
        {
            AbacusKey[] temp = FindObjectsOfType<AbacusKey>();
            keys = new List<AbacusKey>();
            foreach (AbacusKey key in temp)
            {
                keys.Add(key);
                key.OnKeyToggled += UpdateAbacusDisplay;
            }
        }

        private void ResetKeys()
        {
            foreach (AbacusKey key in keys)
            {
                key.Reset();
            }
        }

        private void UpdateAbacusDisplay(object sender, EventArgs e)
        {
            display.DisplayTotal();
            // Announce the event
            OnValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public void OnAbacusValueChanged(int abacusValue)
        {
            Debug.LogError("Abacus value recieved by tutorial abacus is  "+abacusValue.ToString());
            if(abacusValue < 0 || abacusValue > 999)
            {
                return;
            }
            Debug.LogError(abacusValue);
            Total = abacusValue;
        }

        void OnEnable()
        {
            AFGameManagerView.OnAbacusValueChanged += OnAbacusValueChanged;
        }

        void OnDisable()
        {
            AFGameManagerView.OnAbacusValueChanged -= OnAbacusValueChanged;
        }
    }
}



