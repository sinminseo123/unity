using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime; //현재 진행중인 게임 플레이 시간
    public float maxGameTime = 2 * 10f; //최대 게임 플레이 시간

    [Header("# Player Info")]
    public int level; //레벨
    public int kill; //몬스터 킬
    public int exp;//경험치
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; //각 레벨의 필요 경험치
    public float health;
    public int maxHealth = 100;

    [Header("# Game Object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp; //상점 창 변수

    void Awake()
    {
        Instance = this;
    }

    public void GameStart()
    {
        health = maxHealth;
        uiLevelUp.Select(0); //임시 스크립트(첫번째 캐릭터 선택)
        isLive = true;

    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0); //0번째 씬 재시작
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            maxGameTime = gameTime;
        }
    }

    public void GetExp() //경험치 증가 함수
    {
        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) //필요 경험치 도달 시 레벨업
        {
            level++;
            exp = 0;

            uiLevelUp.Show();//레벨 업 시 상점 창 띄움
        }
    }

    public void Stop() //시간 정지
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume() //시간흐름
    {
        isLive = true;
        Time.timeScale = 1;
    }

}
