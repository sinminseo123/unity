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
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 5f),spawnData.Length - 1); //게임 플레이시간 5초 경과할 때마다 레벨업

        if (timer > spawnData[level].spawnTime)
        {
            Spawn();
            timer = 0;
        }

    }

    void Spawn()
    {
        GameObject enermy = GameManager.Instance.pool.Get(0); //풀에서 몬스터 프리펩 가져오기
        enermy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; //몬스터의 위치를 랜덤한 스폰포인트의 위치로 생성
        enermy.GetComponent<Enermy>().Init(spawnData[level]);//몬스터의 init 함수 호출하며 레벨 별로 소환데이터의 인자값 전달
    }
}

[System.Serializable] //직렬화가 되면서 인스펙터에서 보임
public class SpawnData //스폰 데이터 클래스 선언
{
    //스폰 데이터(ex, 몬스터, 무기 등)는 spawner의 인스펙터에서 설정
    public float spawnTime;//몬스터 소환 시간
    public int spriteType; //몬스터 스프라이트 타입
    public int health;//몬스터 체력
    public float speed;//몬스터 속도

}