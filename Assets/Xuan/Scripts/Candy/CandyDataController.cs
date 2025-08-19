using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyDataController : MonoBehaviour
{
    [Header("Candy Column")]
    public CandyData redCol;
    public CandyData greenCol;
    public CandyData blueCol;
    public CandyData purpleCol;
    public CandyData orangeCol;
    public CandyData yellowCol;

    [Header("Candy Row")]
    public CandyData redRow;
    public CandyData greenRow;
    public CandyData blueRow;
    public CandyData purpleRow;
    public CandyData orangeRow;
    public CandyData yellowRow;

    [Header("Candy Super")]
    public CandyData candySuper;

    public void SetCandySpecial(Candy candy , CandySpecialType typeSpecial, CandyType type)
    {
        CandyData candyData = null;
        Debug.Log("Set Candy Special: " + typeSpecial + " " + type);
        switch (typeSpecial)
        {
            case CandySpecialType.StripedHorizontal:
                switch (type)
                {
                    case CandyType.Red:
                        candyData = redRow;
                        break;
                    case CandyType.Green:
                        candyData = greenRow;
                        break;
                    case CandyType.Blue:
                        candyData = blueRow;
                        break;
                    case CandyType.Purple:
                        candyData = purpleRow;
                        break;
                    case CandyType.Orange:
                        candyData = orangeRow;
                        break;
                    case CandyType.Yellow:
                        candyData = yellowRow;
                        break;
                }
                break;
            case CandySpecialType.StripedVertical:

                switch(type)
                {
                    case CandyType.Red:
                        candyData = redCol;
                        break;
                    case CandyType.Green:
                        candyData = greenCol;
                        break;
                    case CandyType.Blue:
                        candyData = blueCol;
                        break;
                    case CandyType.Purple:
                        candyData = purpleCol;
                        break;
                    case CandyType.Orange:
                        candyData = orangeCol;
                        break;
                    case CandyType.Yellow:
                        candyData = yellowCol;
                        break;
                }
                break;
            case CandySpecialType.Wrapped:
                break;
            case CandySpecialType.ColorBomb:
                candyData = candySuper;
                break;
        }

        if(candyData != null && candy != null)
        {
            candy.Init(candyData.candies.candyType, candyData.candies.specialType, candyData.candies.icon);
        }
    }
}
