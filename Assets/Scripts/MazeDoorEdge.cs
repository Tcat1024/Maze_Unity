using UnityEngine;
using System.Collections;

public class MazeDoorEdge : MazePassEdge
{
    private bool m_bMirrored = false;
    public bool IsMirrored
    {
        get
        {
            return m_bMirrored;
        }
        set
        {
            if (m_bMirrored == value)
                return;
            m_bMirrored = value;
            Hanger.localScale = new Vector3(-Hanger.localScale.x, Hanger.localScale.y, Hanger.localScale.z);
            Hanger.localPosition = new Vector3(-Hanger.localPosition.x, Hanger.localPosition.y, Hanger.localPosition.z);
        }
    }
    private Quaternion m_qOpenRoate = Quaternion.Euler(0, 90, 0);
    private Quaternion m_qMirrorOpenRoate = Quaternion.Euler(0, -90, 0);


    private MazeDoorEdge otherSideOfDoor
    {
        get
        {
            return cellOther.GetEdge(Diraction.Opposite()) as MazeDoorEdge;
        }
    }


    public Transform Hanger;

    public override void Initialization(MazeCell curCell, MazeCell otherCell, MazeDiraction dir)
    {
        base.Initialization(curCell, otherCell, dir);

        IsMirrored = otherSideOfDoor != null;

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform tar = transform.GetChild(i);
            if(tar == Hanger)
                continue;
            tar.GetComponent<Renderer>().material = curCell.Room.RoomSetting.WallMaterial;
        }
    }

    public override void OnPlayerEnter()
    {
        base.OnPlayerEnter();
        otherSideOfDoor.Hanger.localRotation = Hanger.localRotation = IsMirrored ? m_qMirrorOpenRoate : m_qOpenRoate;
        cellOther.Room.Show(true);
    }

    public override void OnPlayerExit()
    {
        base.OnPlayerExit();
        otherSideOfDoor.Hanger.localRotation = Hanger.localRotation = Quaternion.Euler(0, 0, 0);
        cellOther.Room.Show(false);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
