using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CandyData")]
public class CandyData : ScriptableObject
{
   [SerializeField]
   public CandyInfo candies; 
}

[System.Serializable]
public class CandyInfo
{
    public CandyType candyType;
    public Candy prefabs;
}
public enum CandyType
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    Orange,
}
public enum CandySpecialType
{
    None,
    StripedHorizontal,
    StripedVertical,
    Wrapped,
    ColorBomb
}