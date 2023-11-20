using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage; //������
    public int per; //����

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir) //������, ����, ����
    {
        this.damage = damage;
        this.per = per;

        if (per > -1) //������ -1�̻��� ����(���Ÿ� �Ѿ�)
        {
            rigid.velocity = dir * 15; //* �ӷ�
        }
    }

    void OnTriggerEnter2D(Collider2D collision) //���Ÿ� ���� ����
    {
        if (!collision.CompareTag("Enermy") || per == -1) //���������� ���
            return;

        per--;
        if (per == -1) //���Ÿ����� ���� 0��
        {
            rigid.velocity = Vector3.zero;
            gameObject.SetActive(false); //���Ÿ� ���� ��Ȱ��ȭ
        }
    }
}
