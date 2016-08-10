using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Maze mazePrefab;
    public Player hostPrefab;
    public Camera mainCamera;

    private Maze mazeInstance;
    private Player hostInstance;
	// Use this for initialization
	void Start () {
        StartCoroutine(BeginGame());

    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Escape))
        {
            RestartGame();
        }
	}

    IEnumerator BeginGame()
    {
        mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mazeInstance = Instantiate(mazePrefab);
        yield return StartCoroutine(mazeInstance.GenerateMaze());
        mainCamera.rect = new Rect(0f, 0f, 0.4f, 0.4f);
        hostInstance = Instantiate(hostPrefab);
        hostInstance.Init(mazeInstance.transform.rotation);
        hostInstance.SetPosTo(mazeInstance.GetCell(new IntVector2(0, 0)));
        mainCamera.clearFlags = CameraClearFlags.Depth;
    }

    void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        if (hostInstance)
            Destroy(hostInstance.gameObject);
        StartCoroutine(BeginGame());
    }
}
