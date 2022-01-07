using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameCam;
    public PlayerController player;
    public GameObject startZone;
    public GameObject wheelPrefabs;
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public AudioSource stageClear;


    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCnt1;
    public int enemyCnt2;
    public int enemyCnt3;
    public int enemyCnt4;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public GameObject gamePanel;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerWheelTxt;
    public Text enemy1Txt;
    public Text enemy2Txt;
    public Text enemy3Txt;
    public Text enemy4Txt;

    void Start()
    {
        enemyList = new List<int>();
        if (PlayerPrefs.GetInt("ScoreInt") == 0)
        {
            player.score = 0;
        }
        else
        {
            player.score = PlayerPrefs.GetInt("ScoreInt");
        }
    }

    public void GameStart()
    {
        gameCam.SetActive(true);

        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void StageStart()
    {
        startZone.SetActive(false);

        foreach(Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(true);
        }

        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        player.transform.position = Vector3.up * 0.8f;

        startZone.SetActive(true);

        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(false);
        }

        isBattle = false;
        stage++;
        if(stage == 10)
        {
            SceneManager.LoadScene("Project3.1");
        }
        else if(stage == 20)
        {
            SceneManager.LoadScene("Project3.2");
        }
    }

    IEnumerator InBattle()
    {


        for (int index=0; index < stage; index++)
        {
            int ran = Random.Range(0, 4);
            enemyList.Add(ran);

            switch (ran)
            {
                case 0:
                    enemyCnt1++;
                    break;
                case 1:
                    enemyCnt2++;
                    break;
                case 2:
                    enemyCnt3++;
                    break;
                case 3:
                    enemyCnt4++;
                    break;
            }
        }

        while(enemyList.Count > 0)
        {
            int ranZone = Random.Range(0, 4);
            GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            enemyList.RemoveAt(0);
            yield return new WaitForSeconds(4f);
        }

        while (enemyCnt1 + enemyCnt2 + enemyCnt3 + enemyCnt4 > 0)
        {
            yield return null;
        }

        Instantiate(wheelPrefabs, GenerateSpawnPosition(), wheelPrefabs.transform.rotation);
        stageClear.Play();
        yield return new WaitForSeconds(4f);
        StageEnd();
    }

    void Update()
    {
        if (isBattle)
        {
            playTime += Time.deltaTime;

        }
        enemy1Txt.text = enemyCnt1.ToString();
        enemy2Txt.text = enemyCnt2.ToString();
        enemy3Txt.text = enemyCnt3.ToString();
        enemy4Txt.text = enemyCnt4.ToString();

    }

    void LateUpdate()
    {
        scoreTxt.text = string.Format("{0:n0}", player.score);
        PlayerPrefs.SetInt("ScoreInt", player.score);
        stageTxt.text = "STAGE " + stage;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);

        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);

        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerWheelTxt.text = string.Format("{0:n0}", player.wheel);

    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-50, 50);
        float spawnPosZ = Random.Range(-50, 50);
        Vector3 randomPos = new Vector3(spawnPosX, 1, spawnPosZ);
        return randomPos;
    }
}