using System;
using System.Collections;
using UnityEngine;

public class AbacusKey : MonoBehaviour
{
    #region Events
    public event EventHandler OnKeyToggled;
    #endregion
    
    #region Variables
    [SerializeField] private int value;
    public bool engaged;
    [SerializeField] private KeyPositions keyPositions;

    //private Abacus abacus;

    private KeyTrainManager keyTrain;
    #endregion

    #region Fields
    public int Value
    {
        get
        {
            return value;
        }
    }

    public Vector3 BumpDirection
    {
        get
        {
            // +1 or -1
            float directionPolarity = Mathf.Sign(keyPositions.activeYPosition - keyPositions.restingYPosition);
            Vector3 directionWhenEngaged = directionPolarity* new Vector3(0f,1f,0f);
            // Flip, if not currently engaged
            Vector3 bumpDirection = engaged? directionWhenEngaged : (-directionWhenEngaged);
            return bumpDirection;
        }
    }
    #endregion
    private void Awake() 
    {
        keyTrain = FindObjectOfType<KeyTrainManager>();    
        //abacus = FindObjectOfType<Abacus>();
    }

    #region Functions in class
    public void ToggleKey()
    {
        engaged = !engaged;
        float posY = engaged? keyPositions.activeYPosition : keyPositions.restingYPosition;
        transform.localPosition = new Vector3(transform.localPosition.x, posY, transform.localPosition.z);
        keyTrain.PushTrain(this);
        OnKeyToggled?.Invoke(this,EventArgs.Empty);
    }

    public void SetKey(bool active)
    {
        // Similar to toggle key, but allows you to specifically set to true/false
        // Also, does not push the rest of the train triggering a recursive loop of infinite depth
        engaged = active;
        float posY = engaged? keyPositions.activeYPosition : keyPositions.restingYPosition;
        transform.localPosition = new Vector3(transform.localPosition.x, posY, transform.localPosition.z);
    }

    IEnumerator AnimateKey(bool active)
    {

        yield return new WaitForSeconds(0f);
    }

    public void Reset() 
    {
        engaged = false;
        float posY = keyPositions.restingYPosition;
        transform.localPosition = new Vector3(transform.localPosition.x, posY, transform.localPosition.z);
    }
    #endregion
    
}
#region Structs used in class
[System.Serializable]
public struct KeyPositions
{
    public float activeYPosition;
    public float restingYPosition;
}
#endregion