using UnityEngine;
using System.Collections;

[System.Serializable]
public struct IntVector2
{
    public int X;
    public int Y;
    public IntVector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.X + b.X, a.Y + b.Y);
    }
}
