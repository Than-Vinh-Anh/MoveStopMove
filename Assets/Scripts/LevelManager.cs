using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] GameObject Player;
    [SerializeField] Canvas EndGameCanvas;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float interval;
    [SerializeField] Canvas startGameCanvas;
    [SerializeField] Canvas guideCanvas;
    [SerializeField] Canvas nameCanvas;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Canvas inGameCanvas;

    PlayerController playerControllerScript;
    [SerializeField] List<GameObject> enemyList;

    public bool isWin = false;
    public bool isLoose = false;
    int index;
    private TextMeshProUGUI numberOfEnemyLeft;

    void Start()
    {
        enemyList = new List<GameObject>();
        index = 0;
        playerControllerScript = Player.GetComponent<PlayerController>();
        foreach (Transform t in spawnPoints)
        {
            GameObject enemy = Instantiate(enemyPrefab, t.position, Quaternion.identity);
            enemy.name = enemy.name + index;
            enemyList.Add(enemy);
            index++;
        }
        nameCanvas.gameObject.SetActive(false);
        CameraFollow.instance.ChangeCameraOffset(new Vector3(0, 2, -2));
        numberOfEnemyLeft = inGameCanvas.GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    void Update()
    {
        numberOfEnemyLeft.text = enemyList.Count.ToString();
        if (playerControllerScript.isDead)
        {
            EndGameCanvas.gameObject.SetActive(true);
            nameCanvas.gameObject.SetActive(false);
            isLoose = true;
            CameraFollow.instance.ChangeCameraOffset(new Vector3(0, 2, -2));
        }
        if (enemyList.Count == 0 && !PlayerController.instance.isDead)
        {
            EndGameCanvas.gameObject.SetActive(true);
            nameCanvas.gameObject.SetActive(false);
            isWin = true;
            PlayerController.instance.PlayWinAnimation();
            CameraFollow.instance.ChangeCameraOffset(new Vector3(0, 2, -2));
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RemoveEnemyFromList(string enemyName)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].name == enemyName)
            {
                enemyList.RemoveAt(i);
            }
        }
    }
    public void StartGame()
    {
        PlayerController.instance.isStart = true;
        inGameCanvas.gameObject.SetActive(true);
        startGameCanvas.gameObject.SetActive(false);
        guideCanvas.gameObject.SetActive(true);
        nameCanvas.gameObject.SetActive(true);
        mainCanvas.gameObject.SetActive(false);
        CameraFollow.instance.ChangeCameraOffset(new Vector3(0, 3, -4));
    }
}
