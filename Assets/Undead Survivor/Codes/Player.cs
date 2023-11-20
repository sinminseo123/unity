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
        rigid = GetComponent<Rigidbody2D>(); //오브젝트에서 컴포넌트 가져오기    //민서
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnMove(InputValue value)
    {
        //if (!GameManager.Instance.isLive)
          //  return;

        inputVec = value.Get<Vector2>(); //뉴 인풋 시스템 적용
    }

    private void FixedUpdate() //물리연산 프레임마다 호출되는 함수
    {
        if (!GameManager.Instance.isLive)
            return;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // 평준화
        // 위치 이동
        rigid.MovePosition(rigid.position + nextVec);//현재 위치 + 이동 방향
    }

    private void LateUpdate() //프레임이 종료되기 전 실행되는 생명주기 함수
    {
        if (!GameManager.Instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude); //캐릭터 이동시, 애니메이터의 Speed값 변경으로 애니메이션 작동

        if(inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision) //플레이어 피격 관련 함수
    {
        if (!GameManager.Instance.isLive)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10; //체력 감소

        if (GameManager.Instance.health < 0) //플레이어 체력이 0일 시
        {
            for (int index =2;index < transform.childCount; index++) //플레이어 자식 오브젝트 비활성화
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead"); //플레이어 dead트리거 설정-> DEAD 애니메이션 실행
        }
    }
}
