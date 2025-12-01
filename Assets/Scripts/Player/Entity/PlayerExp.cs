using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExp : MonoBehaviour, IExpReceiver
{
    public int currentExp = 0;
  
    public void OnExpPickup(int amount)
    {
        currentExp += amount;
        Debug.Log("경험치 획득: " + amount + " 현재 경험치: " + currentExp);
    }
}
