using UnityEngine;
using System.Collections;

public class MathCalculator
{
    public static bool IsDodge(int rate)
    {
        return Random.Range(0, 10000) < rate * 100;
    }
}
