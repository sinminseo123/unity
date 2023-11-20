using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime; //���� �������� ���� �÷��� �ð�
    public float maxGameTime = 2 * 10f; //�ִ� ���� �÷��� �ð�

    [Header("# Player Info")]
    public int level; //����
    public int kill; //���� ų
    public int exp;//����ġ
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; //�� ������ �ʿ� ����ġ
    public float health;
    public int maxHealth = 100;

    [Header("# Game Object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp; //���� â ����

    void Awake()
    {
        Instance = this;
    }

    public void GameStart()
    {
        health = maxHealth;
        uiLevelUp.Select(0); //�ӽ� ��ũ��Ʈ(ù��° ĳ���� ����)
        isLive = true;

    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0); //0��° �� �����
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

    public void GetExp() //����ġ ���� �Լ�
    {
        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) //�ʿ� ����ġ ���� �� ������
        {
            level++;
            exp = 0;

            uiLevelUp.Show();//���� �� �� ���� â ���
        }
    }

    public void Stop() //�ð� ����
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume() //�ð��帧
    {
        isLive = true;
        Time.timeScale = 1;
    }

}
