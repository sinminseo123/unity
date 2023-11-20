using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;//무기 아이디
    public int prefabId; //프리펩 아이디
    public float damage; //무기 데미지
    public int count; //무기 개수
    public float speed; //무기 속도

    float timer; 
    Player player;

    void Awake()
    {
        player = GameManager.Instance.player;
    }


    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); //근접무기 회전
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        //테스트 코드
        if (Input.GetButtonDown("Jump"))
            LevelUp(10,1);
    }

    public void LevelUp(float damage, int count) //무기 레벨업 함수
    {
        this.damage = damage; 
        this.count += count;

        if (id == 0)
        {
            Batch();
        }
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // 기본 세팅
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;


        // 프로퍼티 세팅
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int i = 0; i < GameManager.Instance.pool.prefabs.Length; i++)
        {
            if(data.projectile == GameManager.Instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;
            default:
                speed = 0.3f; //원거리 무기에서 speed는 연사속도 의미
                break;
        }

        //hand 초기화
        Hand hand = player.hands[(int)data.itemtype];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver); //플레이어의 모든 자식오브젝트가 applygear 사용하도록
    }

    private void Batch() //생성된 무기를 배치하는 함수
    {
        for (int i = 0; i < count; i++) //무기 count 개수 만큼 풀링에서 가져오기
        {
            Transform bullet;
            
            //기존 무기 오브젝트 재활용하거나 풀에서 가져오기
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                bullet.parent = transform; //무기의 부모요소 변경

            }

            bullet.localPosition = Vector3.zero; //무기의 위치 초기화
            bullet.localRotation = Quaternion.identity; //무기의 회전 초기화

            Vector3 rotVec = Vector3.forward * 360 * i / count; // 근접무기 회전각도
            bullet.Rotate(rotVec); //회전
            bullet.Translate(bullet.up * 1.5f, Space.World); //근접무기 위치

            bullet.GetComponent<Bullet>().Init(damage, -1,Vector3.zero); //무기의 데미지와 관통력 전달 , -1은 무한 관통
        }
    }

    void Fire() //원거리 무기를 풀에서 꺼내오는 함수
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized; //원거리 무기 발사 방향

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); //원거리무기 회전
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
