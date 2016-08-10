using UnityEngine;
using System.Collections;

public abstract class MazeEdgeBase : MonoBehaviour {
    public MazeCell cellCur;
    public MazeCell cellOther;
    public MazeDiraction Diraction;
    public virtual void Initialization(MazeCell curCell, MazeCell otherCell, MazeDiraction dir)
    {
        cellCur = curCell;
        cellOther = otherCell;
        Diraction = dir;
        curCell.SetEdge(dir, this);
        transform.parent = curCell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = dir.ConvertToRoate();
    }

    public virtual void OnPlayerEnter()
    {

    }

    public virtual void OnPlayerExit()
    {

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
