using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeRoom : ScriptableObject
{
    public int RoomSettingIndex;
    public MazeRoomConfig RoomSetting;
    public List<MazeCell> Cells = new List<MazeCell>();
    public void AddCell(MazeCell tarCell)
    {
        tarCell.Room = this;
        Cells.Add(tarCell);
    }

    public void Show(bool bShow)
    {
        foreach (MazeCell cell in Cells)
        {
            cell.gameObject.SetActive(bShow);
        }
    }
}
