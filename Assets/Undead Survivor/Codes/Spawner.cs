using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;
    public Transform[] spawnPoint;

    float timer;
    int level;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 5f),spawnData.Length - 1); //���� �÷��̽ð� 5�� ����� ������ ������

        if (timer > spawnData[level].spawnTime)
        {
            Spawn();
            timer = 0;
        }

    }

    void Spawn()
    {
        GameObject enermy = GameManager.Instance.pool.Get(0); //Ǯ���� ���� ������ ��������
        enermy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; //������ ��ġ�� ������ ��������Ʈ�� ��ġ�� ����
        enermy.GetComponent<Enermy>().Init(spawnData[level]);//������ init �Լ� ȣ���ϸ� ���� ���� ��ȯ�������� ���ڰ� ����
    }
}

[System.Serializable] //����ȭ�� �Ǹ鼭 �ν����Ϳ��� ����
public class SpawnData //���� ������ Ŭ���� ����
{
    //���� ������(ex, ����, ���� ��)�� spawner�� �ν����Ϳ��� ����
    public float spawnTime;//���� ��ȯ �ð�
    public int spriteType; //���� ��������Ʈ Ÿ��
    public int health;//���� ü��
    public float speed;//���� �ӵ�

}