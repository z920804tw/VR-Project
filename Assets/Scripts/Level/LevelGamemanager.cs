using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelGamemanager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI文字設定")]
    public TMP_Text[] currentUIText;
    public TMP_Text[] highestUIText;
    public GameObject startButton;
    [Header("關卡參數設定")]
    public Transform[] enemySpawnPos;
    public GameObject[] enemyTarget;
    public GameObject mainTarget;
    public int enemyCount;
    public int money;
    public int score;
    public int round;

    [Header("Debug")]
    [SerializeField] bool roundStart;
    [SerializeField] bool isGameOver;
    [SerializeField] bool isSave;
    int count;
    float timer;

    string currentLevelName;
    void Start()
    {
        currentLevelName = SceneManager.GetActiveScene().name;
        SetPreHighestInfo(currentLevelName);
        startButton.SetActive(false);

        score = 0;
        round = 0;
        count = 0;
        timer = 0;

        roundStart = false;
        isGameOver = false;
        isSave = false;

        currentUIText[3].text = $"當前回合敵人數:{enemyCount}";
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver == false)            //遊戲沒有失敗
        {
            if (roundStart == false)        //回合還沒開始
            {
                startButton.SetActive(true);
                if (isSave == false)        //檢測一次並儲存當前的分數和回合數
                {
                    Debug.Log("回合休息");
                    isSave = true;
                    count = 0;
                    SaveCurrentLevelInfor(currentLevelName);
                }
            }
            else    //回合開始
            {
                if (count < enemyCount)
                {
                    SpawnEnemy();
                }
                else
                {
                    if (GameObject.FindGameObjectWithTag("Enemy") == null)
                    {
                        Debug.Log("當前回合敵人消滅");
                        roundStart = false;
                        isSave = false;
                    }
                }
                currentUIText[0].text = $"當前回合:{round}";
                currentUIText[1].text = $"當前分數{score}";
                currentUIText[2].text = $"當前金錢:{money}";
                currentUIText[3].text = $"當前回合敵人數:{enemyCount}";
                startButton.SetActive(false);

            }

            if (mainTarget == null)
            {
                isGameOver = true;
            }
        }
        else
        {
            SaveCurrentLevelInfor(currentLevelName);
            Debug.Log("GameOver");
        }
    }
    void SetPreHighestInfo(string level)
    {
        switch (level)
        {
            case "Level1":
                if (PlayerPrefs.HasKey("L1_HighRound") && PlayerPrefs.HasKey("L1_HighScore"))
                {
                    highestUIText[0].text = $"第一關最高回合:{PlayerPrefs.GetInt("L1_HighRound")}";
                    highestUIText[1].text = $"第一關最高分數:{PlayerPrefs.GetInt("L1_HighScore")}";
                }
                else
                {
                    PlayerPrefs.SetInt("L1_HighScore", 0);
                    PlayerPrefs.SetInt("L1_HighRound", 0);
                    PlayerPrefs.Save();
                }
                break;

            default:
                break;
        }
    }
    void SaveCurrentLevelInfor(string level)
    {
        switch (level)
        {
            case "Level1":
                int sc = PlayerPrefs.GetInt("L1_HighScore");
                int rd = PlayerPrefs.GetInt("L1_HighRound");
                if (score > sc)
                {
                    PlayerPrefs.SetInt("L1_HighScore", score);
                }
                if (round > rd)
                {
                    PlayerPrefs.SetInt("L1_HighRound", round);
                }
                PlayerPrefs.Save();
                break;
        }
    }

    void SpawnEnemy()
    {
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            timer = 0;
            count++;

            int rndE = Random.Range(0, 2);
            int rndP = Random.Range(0, 5);
            Instantiate(enemyTarget[rndE], enemySpawnPos[rndP].position, Quaternion.identity);
        }
    }
    public void StartRound()
    {
        roundStart = true;
        round++;
        enemyCount++;
        Debug.Log("回合開始");
    }

    public void AddScore(int score1, int money1)
    {
        score += score1;
        money += money1;
    }

}
