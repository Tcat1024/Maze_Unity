using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float WalkSpeed = 1f;
    public float TurnSecond = 1f;
    private float m_iTurnDuration = 0f;

    private MazeCell m_curMazeCell;
    private MazeCell m_tarMazeCell;
    private MazeDiraction m_curDiraction;
    private MazeDiraction m_tarDiraction;

    public void Init(Quaternion defaultRoate)
    {
        transform.rotation = defaultRoate;
        m_curDiraction = MazeDiraction.Front;
        m_tarMazeCell = null;
        m_tarDiraction = null;
    }

    public bool IsActing()
    {
        return m_tarDiraction != null || m_tarMazeCell != null;
    }

    public void Move(MazeDiraction dir)
    {
        if (IsActing() || m_curMazeCell == null)
        {
            return;
        }
        MazeDiraction tarDir = m_curDiraction * dir;
        MazeEdgeBase tarEdge = m_curMazeCell.GetEdge(tarDir);
        if (tarEdge is MazePassEdge)
        {
            MoveTo(tarEdge.cellOther);
        }
    }

    public void Turn(MazeDiraction dir)
    {
        if(IsActing())
        {
            return;
        }

        m_tarDiraction = m_curDiraction * dir;
        m_iTurnDuration = 0f;
    }

    public void SetPosTo(MazeCell tarCell)
    {
        if(m_curMazeCell)
        {
            m_curMazeCell.OnPlayerExit();
        }
        m_curMazeCell = tarCell;
        m_tarMazeCell = null;
        transform.position = tarCell.transform.position;
        m_curMazeCell.OnPlayerEnter();
    }

    void MoveTo(MazeCell tarCell)
    {
        if (tarCell == null)
            return;
        if(m_curMazeCell == null)
        {
            SetPosTo(tarCell);
            return;
        }
        m_tarMazeCell = tarCell;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Move(MazeDiraction.Front);
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            Move(MazeDiraction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(MazeDiraction.Back);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(MazeDiraction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Turn(MazeDiraction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Turn(MazeDiraction.Right);
        }

        if (m_tarMazeCell != null && m_tarMazeCell != m_curMazeCell)
        {
            Vector3 disPos = m_tarMazeCell.transform.position - transform.position;
            float walkDist = WalkSpeed * Time.deltaTime;
            if(disPos.sqrMagnitude <= walkDist * walkDist)
            {
                transform.position = m_tarMazeCell.transform.position;
                m_curMazeCell.OnPlayerExit();
                m_tarMazeCell.OnPlayerEnter();
                m_curMazeCell = m_tarMazeCell;
                m_tarMazeCell = null;
            }
            else
            {
                transform.position += disPos.normalized * walkDist;
            }
        }

        if(m_tarDiraction != null && m_tarDiraction != m_curDiraction)
        {
            Quaternion tarRoate = m_tarDiraction.ConvertToRoate();
            m_iTurnDuration += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(m_curDiraction.ConvertToRoate(), tarRoate, Mathf.Min(1f, m_iTurnDuration / TurnSecond));
            
            if (m_iTurnDuration >= TurnSecond)
            {
                transform.rotation = tarRoate;
                m_curDiraction = m_tarDiraction;
                m_iTurnDuration = 0f;
                m_tarDiraction = null;
            }
        }
	}
}
