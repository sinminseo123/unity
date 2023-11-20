using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; //스캔 범위
    public LayerMask targetLayer; //스캔 레이어
    public RaycastHit2D[] targets; //스캔 결과 배열
    public Transform nearestTarget; //가장 가까운 목표

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position,scanRange,Vector2.zero,0,targetLayer); //원형의 몬스터 스캐너 형성
        nearestTarget = GetNearest();
    }
    Transform GetNearest() //가장 가까운 타겟 위치 구하는 함수
    {
        Transform result = null;

        float diff = 100;

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;

            float curDiff = Vector3.Distance(myPos, targetPos); //플레이어와 타겟의 거리 구함
            if(curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
        
