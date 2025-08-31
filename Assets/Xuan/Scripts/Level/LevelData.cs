using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("Board Size")]
    [Min(3)] public int width = 9;
    [Min(3)] public int height = 9;

    [Header("Move / Score Goals")]
    public int moveLimit = 25;
    public int targetScore = 15000;

    [Header("Possible Candies")] public CandyData[] candies;
}
