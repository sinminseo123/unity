using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage; //데미지
    public int per; //관통

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir) //데미지, 관통, 방향
    {
        this.damage = damage;
        this.per = per;

        if (per > -1) //관통이 -1이상인 무기(원거리 총알)
        {
            rigid.velocity = dir * 15; //* 속력
        }
    }

    void OnTriggerEnter2D(Collider2D collision) //원거리 무기 관통
    {
        if (!collision.CompareTag("Enermy") || per == -1) //근접무기인 경우
            return;

        per--;
        if (per == -1) //원거리무기 관통 0시
        {
            rigid.velocity = Vector3.zero;
            gameObject.SetActive(false); //원거리 무기 비활성화
        }
    }
}
