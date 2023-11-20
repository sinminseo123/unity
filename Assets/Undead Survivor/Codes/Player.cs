using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;

    public Scanner scanner;
    public Hand[] hands;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); //������Ʈ���� ������Ʈ ��������    //�μ�
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnMove(InputValue value)
    {
        //if (!GameManager.Instance.isLive)
          //  return;

        inputVec = value.Get<Vector2>(); //�� ��ǲ �ý��� ����
    }

    private void FixedUpdate() //�������� �����Ӹ��� ȣ��Ǵ� �Լ�
    {
        if (!GameManager.Instance.isLive)
            return;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // ����ȭ
        // ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);//���� ��ġ + �̵� ����
    }

    private void LateUpdate() //�������� ����Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    {
        if (!GameManager.Instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude); //ĳ���� �̵���, �ִϸ������� Speed�� �������� �ִϸ��̼� �۵�

        if(inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision) //�÷��̾� �ǰ� ���� �Լ�
    {
        if (!GameManager.Instance.isLive)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10; //ü�� ����

        if (GameManager.Instance.health < 0) //�÷��̾� ü���� 0�� ��
        {
            for (int index =2;index < transform.childCount; index++) //�÷��̾� �ڽ� ������Ʈ ��Ȱ��ȭ
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead"); //�÷��̾� deadƮ���� ����-> DEAD �ִϸ��̼� ����
        }
    }
}
