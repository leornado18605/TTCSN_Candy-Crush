using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CandyData")]
public class CandyData : ScriptableObject
{
   [SerializeField]
   public List<CandyInfo> candies = new List<CandyInfo>(); 
}

[System.Serializable]
public class CandyInfo
{
    public CandyType candyType;
    public Sprite candyIcon;

}
