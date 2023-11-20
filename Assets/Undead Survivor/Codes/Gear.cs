using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        //기본 세팅
        name = "Gear " + data.itemId;
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;

        //프로퍼티 세팅
        type = data.itemtype;
        rate = data.damages[0];

        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();

    }

    void ApplyGear() //기어의 타입에 따라 로직 적용 함수
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp() //플레이어의 모든 무기 연사력(공격속도) 증가 함수
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0: //삽
                    weapon.speed = 150 + (150 * rate);
                    break;
                default://엽총
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }

    void SpeedUp() //플레이어 이동속도 증가
    {
        float speed = 3;
        GameManager.Instance.player.speed = speed + (speed * rate);
    }
}
