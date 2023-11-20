using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;


    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }


    public void Show() //상점 창 띄우기
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.Instance.Stop();
    }

    public void Hide() //상점 창 지우기
    {
        rect.localScale = Vector3.zero;
        GameManager.Instance.Resume();
    }

    public void Select(int index) //기본 무기 지급
    {
        items[index].OnClick();
    }

    void Next() //랜덤 3개 표출
    {
        //모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        //그 중 랜덤 3개 아이템 활성화
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0]!= ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for(int index=0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];

            //만렙 아이템의 경우는 소비아이템으로 대체
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);   // 소비아이템이 3개인경우 items[Random.Range(4,7)].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }            
        }


    }
}
