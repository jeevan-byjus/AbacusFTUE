using UnityEngine;

public class KeyTrainManager : MonoBehaviour
{
   [SerializeField] private AbacusKey[] onesKeyTrain;
   [SerializeField] private AbacusKey[] tensKeyTrain;
   [SerializeField] private AbacusKey[] hundredsKeyTrain;

   public void PushTrain(AbacusKey key)
   {
       int index;

       if(key.Value/10 == 0)        // Value belonging in ones
       {
           index = FindIndexOf(key, onesKeyTrain);
           PushKeyArray(onesKeyTrain, index);
       }
       else if(key.Value/100 == 0)        // Value belonging in tens
       {
           index = FindIndexOf(key, tensKeyTrain);
           PushKeyArray(tensKeyTrain, index);
       }
       else //if(key.Value/1000 == 0)        // Value belonging in hundreds
       {
           index = FindIndexOf(key, hundredsKeyTrain);
           PushKeyArray(hundredsKeyTrain, index);
       }
   }

   private int FindIndexOf(AbacusKey key, AbacusKey[] keyArray)
   {
       int index =-1;   // Index of -1 means not found
       for(int i=0; i<keyArray.Length; i++)
       {
           if(key == keyArray[i])
           {
               index =i;
           }
       }
       return index;
   }

   private void PushKeyArray(AbacusKey[] keyArray, int index)
   {
       // Bail if invalid index
       if(index == -1)
       {
           return;
       }
       // If key at index is active, activate everything above it
       if(keyArray[index].engaged)
       {
           for(int i=index-1; i>=0; i--)
           {
               keyArray[i].SetKey(true);
           }
       }
       // Else, deactivate everything below it 
       else // if(keyArray[index].engaged == false)
       {
           for(int i=index+1; i<keyArray.Length; i++)
           {
               keyArray[i].SetKey(false);
           }
       }
   }
}
