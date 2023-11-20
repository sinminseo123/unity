using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.Instance.player.transform.position; //플레이어 위치를 게임매니저에서 가져옴
        Vector3 myPos = transform.position; //타일맵 위치
        float diffX = Mathf.Abs(playerPos.x - myPos.x); //플레이어와 타일맵의 x축 절대값 거리 구하기
        float diffY = Mathf.Abs(playerPos.y - myPos.y); //플레이어와 타일맵의 y축 절대값 거리 구하기

        Vector3 playerDir = GameManager.Instance.player.inputVec; //플레이어의 이동 방향을 저장하는 변수
        //nomalized로 인해 대각선 이동 방향이 1보다 작기에 변환해줌
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":

                if (diffX > diffY) //플레이어가 x축으로 멀어졌을 시
                {
                    transform.Translate(Vector3.right * dirX * 40); //플레이어 이동방향으로 타일맵 이동
                }else if (diffX < diffY) //플레이어가 y축으로 멀어졌을 시
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enermy":

                if (coll.enabled) //몬스터의 콜라이더가 비활성화일시
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f,3f), Random.Range(-3f, 3f), 0f));//플레이어의 이동 방향에 따라 맞은 편에서 등장하도록 이동
                }

                break;
        }
    }
}
