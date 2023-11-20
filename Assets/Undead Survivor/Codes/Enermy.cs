using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enermy : MonoBehaviour
{
    public float speed; //���� �ӵ��� �޾ƿ� ����
    public RuntimeAnimatorController[] animCon; //���� �ִϸ����� ������ �޾ƿ� ���� -�ν����Ϳ��� �ʱ�ȭ
    public float health; //������ ������ ü�� ������ ����
    public float maxHealth; //������ �ִ� ü�� ������ ����

    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;

    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) //���Ͱ� �׾��ų� Hit �ִϸ��̼� �������̶�� ����
            return;

        //���Ͱ� �÷��̾ ���󰡵��� ��
        Vector2 dirVec = target.position - rigid.position; //�÷��̾�� ������ ��ġ ����
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //�÷��̾��� Ű�Է°��� ���� ������ ���� �̵���
        rigid.MovePosition(rigid.position+nextVec); //�÷��̾� ��ġ������ �̵�
        rigid.velocity = Vector2.zero; //���� �ӵ��� �̵��� ������ ���� �ʵ��� �ӵ� ����        
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive) //���Ͱ� �׾��ٸ� ����
            return;

        //���� ��������Ʈ �¿����
        spriter.flipX = target.position.x < rigid.position.x; //�÷��̾��� x�ప�� �ڽ��� x�� ���� ���Ͽ� ������ ����
    }

    void OnEnable() //���� ������Ʈ�� Ȱ��ȭ �ɶ�
    {//���� �������� Ÿ�� ����
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true; //���� ���� �ʱ�ȭ
        coll.enabled = true; //�ݶ��̴� �ʱ�ȭ
        rigid.simulated = true; //������ٵ� �ʱ�ȭ
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);//���� �ִϸ��̼� �ο� �ʱ�ȭ
        health = maxHealth; //ü�� �ʱ�ȭ
    }

    public void Init(SpawnData data) //spawner���� ������ �ʱ� ������ �޾ƿ��� �Լ�
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; //���� ������ �ִϸ��̼��� ������ �ʱ� ���� Ÿ������ ����
        speed = data.speed; //���ǵ� ����
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision) //����� �浹 �Ǵ� �Լ�
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage; //bullet ������Ʈ�� �����Ͽ� ���� �������� ������ ���� ü�� ���
        StartCoroutine(KnockBack());//���� �˹�

        if (health > 0)
        { 
            anim.SetTrigger("Hit"); // �ǰ� �ִϸ��̼� Ʈ���� ����

        }else
        {
            isLive = false;
            coll.enabled = false; //�ݶ��̴� ��Ȱ��ȭ
            rigid.simulated = false; //������ٵ� ��Ȱ��ȭ
            spriter.sortingOrder = 1;
            anim.SetBool("Dead",true);//���� �ִϸ��̼� �ο� ����

            GameManager.Instance.kill++;  //���� ��� �� ų�� ������ �Բ� ����ġ �Լ� ȣ��
            GameManager.Instance.GetExp();        
        }
            
    }

    IEnumerator KnockBack()
    {
        yield return wait; //���� �ϳ��� ���� ������ ������
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos; //�÷��̾� �ݴ����
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); //�÷��̾��� �ݴ� �������� 3��ŭ �̵�
    }

    void Dead()
    {
        gameObject.SetActive(false); //���� ������Ʈ ��Ȱ��ȭ
    }
}
