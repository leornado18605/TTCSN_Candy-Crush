using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CandyType
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple
}
public class Candy : MonoBehaviour
{
    public int row;
    public int column;
    public CandyType type;
    public void SetPosition(int r, int c)
    {
        row = r;
        column = c;
    }
}
