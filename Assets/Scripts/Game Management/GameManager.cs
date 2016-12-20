using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public bool isGameOver;
    public bool openExit;
    public GameObject exitDoor;
    private Vector3 exitDoorMoveTowards;
    private const string LEVEL1 = "L01-Tower Entrance";
    private const string LEVEL2 = "L02-Forest";


    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("Game Manager").AddComponent<GameManager>();
            }
            return instance;
        }
    }

    public void Awake()
    {

        if (SceneManager.GetActiveScene().name.Equals(LEVEL1))
        {
            exitDoor = GameObject.FindGameObjectWithTag("Exit Door");
            exitDoorMoveTowards = new Vector3(exitDoor.transform.position.x, 25.0F, exitDoor.transform.position.z);
        }

        isGameOver = false;
        openExit = false;


    }

    public void FixedUpdate()
    {
        if (!isGameOver)
        {
            if (openExit)
            {
                exitDoor.transform.position = Vector3.MoveTowards(exitDoor.transform.position, exitDoorMoveTowards, 8.0f * Time.deltaTime);
            }
        }
    }

    public void OnApplicationQuit()
    {
        instance = null;
    }
}
