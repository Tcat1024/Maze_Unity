using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {
    public float MazeDoorPercent;
    public IntVector2 Size;
    public MazeCell MazeCellPerfab;
    public MazeEdgeBase MazePassEdgePerfab;
    public MazeEdgeBase MazeDoorEdgePerfab;
    public MazeEdgeBase[] MazeWallEdgePerfabs;
    public MazeRoomConfig[] MazeRoomSettings;


    private List<MazeCell> m_lCellList = new List<MazeCell>();
    private List<MazeRoom> m_lRoomList = new List<MazeRoom>();
    private MazeCell[] m_vCells;

    void SetCell(IntVector2 pos, MazeCell cell)
    {
        m_vCells[Size.X * pos.Y + pos.X] = cell;
    }

    public MazeCell GetCell(IntVector2 pos)
    {
        return m_vCells[Size.X * pos.Y + pos.X];
    }

    Vector3 ConvertIntVecToPosition(IntVector2 pos)
    {
        return new Vector3(pos.X - (float)Size.X / 2, 0, pos.Y - (float)Size.Y / 2);
    }

    IntVector2 RandomPosition()
    {
        return new IntVector2(Random.Range(0, Size.X), Random.Range(0, Size.Y));
    }

    public IEnumerator GenerateMaze()
    {
        m_vCells = new MazeCell[Size.X * Size.Y];
        m_lCellList.Clear();
        m_lRoomList.Clear();
        IntVector2 curPos = RandomPosition();

        MazeCell curCell = CreateMazeCell(curPos);
        curCell.Initialization(CreateRoom(-1));
        m_lCellList.Add(curCell);
        var delay = new WaitForSeconds(0.01f);
        while(m_lCellList.Count > 0)
        {
            yield return delay;
            MakeNextCell();
        }

        foreach (MazeRoom room in m_lRoomList)
        {
            room.Show(false);
        }
    }

    void MakeNextCell()
    {
        int curIdx = m_lCellList.Count - 1;
        MazeCell curCell = m_lCellList[curIdx];

        if (curCell.IsFullEdge())
        {
            m_lCellList.Remove(curCell);
            return;
        }

        MazeDiraction tarDir = curCell.RandomUnsetDirection();
        IntVector2 curPos = curCell.Position + tarDir.ConvertToIntVector2();

        if (curPos.X < 0 || curPos.X >= Size.X || curPos.Y < 0 || curPos.Y >= Size.Y)
        {
            CreateWall(curCell, null, tarDir);
        }
        else
        {
            MazeCell tarCell = GetCell(curPos);
            if (tarCell)
            {
                if(tarCell.Room.RoomSettingIndex == curCell.Room.RoomSettingIndex)
                {
                    CreatePassage(curCell, tarCell, tarDir);
                    if(tarCell.Room != curCell.Room)
                    {
                        MazeRoom tarRoom = tarCell.Room;
                        for(int i= 0; i < tarRoom.Cells.Count; ++i)
                        {
                            curCell.Room.AddCell(tarRoom.Cells[i]);
                            m_lRoomList.Remove(tarRoom);
                            Destroy(tarRoom);
                        }
                    }
                }
                else
                {
                    CreateWall(curCell, tarCell, tarDir);
                }
            }
            else
            {
                tarCell = CreateMazeCell(curPos);

                if(Random.Range(0f, 1f) < MazeDoorPercent)
                {
                    tarCell.Initialization(CreateRoom(curCell.Room.RoomSettingIndex));
                    CreateDoor(curCell, tarCell, tarDir);
                }
                else
                {
                    tarCell.Initialization(curCell.Room);
                    CreatePassage(curCell, tarCell, tarDir);
                }
                m_lCellList.Add(tarCell);
            }
        }
    }

    void CreateDoor(MazeCell curCell, MazeCell otherCell, MazeDiraction dir)
    {
        MazeEdgeBase edge = Instantiate(MazeDoorEdgePerfab);
        edge.Initialization(curCell, otherCell, dir);
        edge = Instantiate(MazeDoorEdgePerfab);
        edge.Initialization(otherCell, curCell, dir.Opposite());
    }

    void CreatePassage(MazeCell curCell, MazeCell otherCell, MazeDiraction dir)
    {
        MazeEdgeBase edge = Instantiate(MazePassEdgePerfab);
        edge.Initialization(curCell, otherCell, dir);
        edge = Instantiate(MazePassEdgePerfab);
        edge.Initialization(otherCell, curCell, dir.Opposite());
    }

    void CreateWall(MazeCell curCell, MazeCell otherCell, MazeDiraction dir)
    {
        MazeEdgeBase tarEdge = MazeWallEdgePerfabs[Random.Range(0, MazeWallEdgePerfabs.Length)];
        MazeEdgeBase edge = Instantiate(tarEdge);
        edge.Initialization(curCell, otherCell, dir);
        if(otherCell)
        {
            edge = Instantiate(tarEdge);
            edge.Initialization(otherCell, curCell, dir.Opposite());
        }
    }

    MazeRoom CreateRoom(int curRoomIndex)
    {
        MazeRoom room = ScriptableObject.CreateInstance<MazeRoom>();
        int tarIndex = Random.Range(0, MazeRoomSettings.Length);
        if(tarIndex == curRoomIndex)
        {
            tarIndex = (tarIndex + 1) % MazeRoomSettings.Length;
        }
        room.RoomSettingIndex = tarIndex;
        room.RoomSetting = MazeRoomSettings[tarIndex];
        m_lRoomList.Add(room);
        return room;
    }

    MazeCell CreateMazeCell(IntVector2 tarPos)
    {
        MazeCell cell = Instantiate(MazeCellPerfab);
        cell.name = string.Format("MazeCell_{0}_{1}", tarPos.X, tarPos.Y);
        cell.transform.parent = this.transform;
        cell.transform.localPosition = ConvertIntVecToPosition(tarPos);
        cell.Position = tarPos;
        SetCell(tarPos, cell);
        return cell;
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
