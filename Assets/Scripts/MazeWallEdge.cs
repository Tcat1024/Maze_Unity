using UnityEngine;
using System.Collections;

public class MazeWallEdge : MazeEdgeBase {

    public override void Initialization(MazeCell curCell, MazeCell otherCell, MazeDiraction dir) 
    {
        base.Initialization(curCell, otherCell, dir);
        transform.GetChild(0).GetComponent<Renderer>().material = curCell.Room.RoomSetting.WallMaterial;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
