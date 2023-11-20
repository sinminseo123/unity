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

        Vector3 playerPos = GameManager.Instance.player.transform.position; //�÷��̾� ��ġ�� ���ӸŴ������� ������
        Vector3 myPos = transform.position; //Ÿ�ϸ� ��ġ
        float diffX = Mathf.Abs(playerPos.x - myPos.x); //�÷��̾�� Ÿ�ϸ��� x�� ���밪 �Ÿ� ���ϱ�
        float diffY = Mathf.Abs(playerPos.y - myPos.y); //�÷��̾�� Ÿ�ϸ��� y�� ���밪 �Ÿ� ���ϱ�

        Vector3 playerDir = GameManager.Instance.player.inputVec; //�÷��̾��� �̵� ������ �����ϴ� ����
        //nomalized�� ���� �밢�� �̵� ������ 1���� �۱⿡ ��ȯ����
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":

                if (diffX > diffY) //�÷��̾ x������ �־����� ��
                {
                    transform.Translate(Vector3.right * dirX * 40); //�÷��̾� �̵��������� Ÿ�ϸ� �̵�
                }else if (diffX < diffY) //�÷��̾ y������ �־����� ��
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enermy":

                if (coll.enabled) //������ �ݶ��̴��� ��Ȱ��ȭ�Ͻ�
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f,3f), Random.Range(-3f, 3f), 0f));//�÷��̾��� �̵� ���⿡ ���� ���� ���� �����ϵ��� �̵�
                }

                break;
        }
    }
}
