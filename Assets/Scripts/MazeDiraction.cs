using UnityEngine;
using System.Collections;

public class MazeDiraction
{
    enum DirType
    {
        Left,
        Front,
        Right,
        Back,
        Count
    }
 
    DirType m_curType;
    private MazeDiraction(DirType type)
    {
        m_curType = type;
    }
    public static MazeDiraction Left = new MazeDiraction(DirType.Left);
    public static MazeDiraction Right = new MazeDiraction(DirType.Right);
    public static MazeDiraction Front = new MazeDiraction(DirType.Front);
    public static MazeDiraction Back = new MazeDiraction(DirType.Back);
    static MazeDiraction[] s_vDefaultDirs =
    {
        Left,
        Front,
        Right,
        Back,
    };
    public static MazeDiraction ConvertFromInt(int Idx)
    {
        if (Idx < 0 || Idx >= s_vDefaultDirs.Length)
            return null;
        return s_vDefaultDirs[Idx];
    }


    public static int Count = (int)DirType.Count;

    public int ConvertToInt()
    {
        return (int)m_curType;
    }

    static IntVector2[] s_vMazeDirToIntVec2 = {
        new IntVector2(-1, 0),
        new IntVector2(0, 1),
        new IntVector2(1, 0),
        new IntVector2(0, -1),
    };

    public IntVector2 ConvertToIntVector2()
    {
        return s_vMazeDirToIntVec2[ConvertToInt()];
    }

    static Quaternion[] s_vMazeDirToRoate = {
        Quaternion.Euler(0, -90, 0),
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(0, 90, 0),
        Quaternion.Euler(0, 180, 0),
    };
    public Quaternion ConvertToRoate()
    {
        return s_vMazeDirToRoate[ConvertToInt()];
    }


    static MazeDiraction[] s_vOppositeDirs =
    {
        Right,
        Back,
        Left,
        Front,
    };
    public MazeDiraction Opposite()
    {
        return s_vOppositeDirs[ConvertToInt()];
    }

    public static MazeDiraction RandomValue()
    {
        return s_vDefaultDirs[Random.Range(0, Count)];
    }

    static int[] s_vMazeDirToOffset = {
        -1, 
        0,
        1,
        2,
    };

    public static MazeDiraction operator * (MazeDiraction cur, MazeDiraction tar)
    {
        int curIdx = cur.ConvertToInt();
        return ConvertFromInt((curIdx + s_vMazeDirToOffset[tar.ConvertToInt()] + s_vDefaultDirs.Length) % s_vDefaultDirs.Length);
    }
}


