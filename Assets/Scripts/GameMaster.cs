using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [Tooltip("Used to set determine playable area")]
    public Camera camera;
    public GameobjectPool PipePool;
    public GameObject player;
    public GameObject Ceiling;
    //playable area
    public float MinX = -10;
    public float MaxX = 10;
    public float MinY = -20;
    public float MaxY = 20;
    public float PipeMovingSpeed = 1;
    public float PipeSpawnMargin = 2;
    [Range(0, 1)]
    public float PlayerHorizontalPos = 0.2f;

    public ScoreCounter Score;

    [Min(0), Tooltip("Distance beetwen pipes")]
    public float PipeDistance = 4;
    [Min(0), Tooltip("Size of the vertical gap between pipes")]
    public float PipeGapHeight = 2;
    [Tooltip("Maximal height at which pipe gap may appear")]
    public float PipeMaxGapHeight;
    [Tooltip("Minimal height at which pipe gap may appear")]
    public float PipeMinGapHeight;

    readonly List<GameObject> pipes = new List<GameObject>();
    GameObject lastPipeAhead = null; //closest pipe ahead of player in last frame
    bool pipeAheadChanged = false;

    void OnValidate()
    {
        if (camera != null)
            FitBordersToCamera();

        Vector3 playerPos = player.transform.position;
        playerPos.x = MinX + PlayerHorizontalPos * (MaxX-MinX);
        player.transform.position = playerPos;

        if (PipeMinGapHeight < MinY)
            PipeMinGapHeight = MinY;

        if (PipeMaxGapHeight > MaxY)
            PipeMaxGapHeight = MaxY;
    }

    void FitBordersToCamera()
    {
        Vector2 screenSize = camera.ViewportToWorldPoint(new Vector2(1, 1)) - camera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 min = camera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = camera.ViewportToWorldPoint(new Vector2(1, 1));
        MinX = min.x;
        MaxX = max.x;

        MinY = min.y;
        MaxY = max.y;
    }

    void Start()
    {
        SpawnPipe();
        lastPipeAhead = pipes[0];
        Vector3 playerPos = player.transform.position;
        playerPos.x = MinX + PlayerHorizontalPos * (MaxX - MinX);
        player.transform.position = playerPos;
    }

    void Update()
    {
        UpdatePipes();

        RecalculatePipeAhead();

        if (pipeAheadChanged)
            Score.Score++;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Game");
    }

    void MovePipes()
    {
        foreach(GameObject G in pipes)
            G.transform.position += new Vector3(-PipeMovingSpeed * Time.deltaTime, 0, 0);
    }

    //return pipes outside of playable area to pool
    void RemovePipes() 
    {
        for (int i = 0; i < pipes.Count; i++)
        {
            if (pipes[i].transform.position.x < MinX - PipeSpawnMargin)
            {
                PipePool.ReturnToPool(pipes[i]);
                pipes.RemoveAt(i);
                i--;
            }
        }

    }

    void UpdatePipes()
    {
        MovePipes();
        RemovePipes();
        if(pipes.Count == 0)
            SpawnPipe();
        else if (MaxX + PipeSpawnMargin - pipes[pipes.Count - 1].transform.position.x >= PipeDistance)
            SpawnPipe();
    }

    void RecalculatePipeAhead()
    {
        pipeAheadChanged = false;
        GameObject newPipeAhead = null;
        foreach (GameObject G in pipes)
        {
            if (G.transform.position.x > player.transform.position.x)
            {
                newPipeAhead = G;
                break;
            }
        }
        if (newPipeAhead != lastPipeAhead)
        {
            lastPipeAhead = newPipeAhead;
            pipeAheadChanged = true;
        }
        lastPipeAhead = newPipeAhead;
    }

    void SpawnPipe()
    {
        float gapY = Random.Range(PipeMinGapHeight, PipeMaxGapHeight);

        GameObject newPipe = PipePool.GetFromPool();
        newPipe.transform.position = new Vector3(0, 0, 0);

        GameObject pipeTop = newPipe.transform.GetChild(0).gameObject;
        GameObject pipeDown = newPipe.transform.GetChild(1).gameObject;

        SpriteRenderer renderer;
        float requiredSizeDown = gapY - MinY;
        float requiredSizeTop = MaxY - (gapY + PipeGapHeight);

        //position and rescale down pipe 
        renderer = pipeDown.GetComponent<SpriteRenderer>();
        float pipeSizeY = renderer.sprite.bounds.size.y;
        if (pipeSizeY < requiredSizeDown)
            pipeDown.transform.localScale = new Vector3(pipeDown.transform.localScale.x, requiredSizeDown/pipeSizeY, pipeDown.transform.localScale.z);
        pipeSizeY = pipeDown.transform.localScale.y * renderer.sprite.bounds.size.y;
        pipeDown.transform.position = new Vector3(0, gapY-pipeSizeY/2, 0);

        //position and rescale top pipe
        renderer = pipeTop.GetComponent<SpriteRenderer>();
        pipeSizeY = renderer.sprite.bounds.size.y;
        if (pipeSizeY < requiredSizeTop)
            pipeTop.transform.localScale = new Vector3(pipeTop.transform.localScale.x, requiredSizeTop/ pipeSizeY, pipeTop.transform.localScale.z);
        pipeSizeY = pipeTop.transform.localScale.y * renderer.sprite.bounds.size.y;
        pipeTop.transform.position = new Vector3(0, gapY+PipeGapHeight + pipeSizeY / 2, 0);

        //set pipe position
        newPipe.transform.position = new Vector3(MaxX + PipeSpawnMargin + PipeDistance, 0, newPipe.transform.position.z);
        newPipe.transform.SetParent(transform, true);

        pipes.Add(newPipe);
    }
}
