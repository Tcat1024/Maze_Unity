using UnityEngine;
using System.Collections;

public class MazeCell : MonoBehaviour {
    [HideInInspector]
    public IntVector2 Position;
    [HideInInspector]
    public MazeEdgeBase[] m_vEdges = new MazeEdgeBase[MazeDiraction.Count];
    [HideInInspector]
    public MazeRoom Room;

    private int m_iEdgeCount = 0;

    public void SetEdge(MazeDiraction dir, MazeEdgeBase edge)
    {
        m_vEdges[dir.ConvertToInt()] = edge;
        ++m_iEdgeCount;
    }

    public MazeEdgeBase GetEdge(MazeDiraction dir)
    {
        return m_vEdges[dir.ConvertToInt()];
    }

    public bool IsFullEdge()
    {
        return m_iEdgeCount >= MazeDiraction.Count;
    }

    public MazeDiraction RandomUnsetDirection()
    {
        MazeDiraction result = null;
        int Idx = Random.Range(0, MazeDiraction.Count - m_iEdgeCount);
        for(int i =0; i< MazeDiraction.Count; ++i)
        {
            if (m_vEdges[i] == null)
            {
                --Idx;
                if(Idx < 0)
                {
                    result = MazeDiraction.ConvertFromInt(i);
                    break;
                }
            }
        }
        return result;
    }

    public void Initialization(MazeRoom room)
    {
        room.AddCell(this);

        transform.GetChild(0).GetComponent<Renderer>().material = room.RoomSetting.FloorMaterial;
    }

    public void OnPlayerEnter()
    {
        Room.Show(true);
        for(int i = 0; i < m_vEdges.Length; ++i)
        {
            if(m_vEdges[i])
            {
                m_vEdges[i].OnPlayerEnter();
            }
        }
    }

    public void OnPlayerExit()
    {
        Room.Show(false);
        for (int i = 0; i < m_vEdges.Length; ++i)
        {
            if (m_vEdges[i])
            {
                m_vEdges[i].OnPlayerExit();
            }
        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
