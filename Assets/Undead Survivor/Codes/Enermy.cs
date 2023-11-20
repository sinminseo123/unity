using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enermy : MonoBehaviour
{
    public float speed; //몬스터 속도를 받아올 변수
    public RuntimeAnimatorController[] animCon; //몬스터 애니메이터 데이터 받아올 변수 -인스펙터에서 초기화
    public float health; //몬스터의 현재의 체력 데이터 변수
    public float maxHealth; //몬스터의 최대 체력 데이터 변수

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

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) //몬스터가 죽었거나 Hit 애니메이션 실행중이라면 리턴
            return;

        //몬스터가 플레이어를 따라가도록 함
        Vector2 dirVec = target.position - rigid.position; //플레이어와 몬스터의 위치 차이
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //플레이어의 키입력값을 통한 몬스터의 다음 이동값
        rigid.MovePosition(rigid.position+nextVec); //플레이어 위치쪽으로 이동
        rigid.velocity = Vector2.zero; //물리 속도가 이동에 영향을 주지 않도록 속도 제거        
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive) //몬스터가 죽었다면 리턴
            return;

        //몬스터 스프라이트 좌우반전
        spriter.flipX = target.position.x < rigid.position.x; //플레이어의 x축값과 자신의 x축 값을 비교하여 작으면 반전
    }

    void OnEnable() //몬스터 오브젝트가 활성화 될때
    {//몬스터 프리펩의 타겟 설정
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true; //생존 여부 초기화
        coll.enabled = true; //콜라이더 초기화
        rigid.simulated = true; //리지드바디 초기화
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);//데드 애니메이션 부울 초기화
        health = maxHealth; //체력 초기화
    }

    public void Init(SpawnData data) //spawner에서 몬스터의 초기 설정을 받아오는 함수
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; //현재 몬스터의 애니메이션을 가져온 초기 설정 타입으로 설정
        speed = data.speed; //스피드 설정
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision) //무기와 충돌 판단 함수
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage; //bullet 컴포넌트로 접근하여 무기 데미지를 가져와 몬스터 체력 계산
        StartCoroutine(KnockBack());//몬스터 넉백

        if (health > 0)
        { 
            anim.SetTrigger("Hit"); // 피격 애니메이션 트리거 설정

        }else
        {
            isLive = false;
            coll.enabled = false; //콜라이더 비활성화
            rigid.simulated = false; //리지드바디 비활성화
            spriter.sortingOrder = 1;
            anim.SetBool("Dead",true);//데드 애니메이션 부울 설정

            GameManager.Instance.kill++;  //몬스터 사망 시 킬수 증가와 함께 경험치 함수 호출
            GameManager.Instance.GetExp();        
        }
            
    }

    IEnumerator KnockBack()
    {
        yield return wait; //다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos; //플레이어 반대방향
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); //플레이어의 반대 방향으로 3만큼 이동
    }

    void Dead()
    {
        gameObject.SetActive(false); //몬스터 오브젝트 비활성화
    }
}
