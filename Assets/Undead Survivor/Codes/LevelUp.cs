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


    public void Show() //���� â ����
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.Instance.Stop();
    }

    public void Hide() //���� â �����
    {
        rect.localScale = Vector3.zero;
        GameManager.Instance.Resume();
    }

    public void Select(int index) //�⺻ ���� ����
    {
        items[index].OnClick();
    }

    void Next() //���� 3�� ǥ��
    {
        //��� ������ ��Ȱ��ȭ
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        //�� �� ���� 3�� ������ Ȱ��ȭ
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

            //���� �������� ���� �Һ���������� ��ü
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);   // �Һ�������� 3���ΰ�� items[Random.Range(4,7)].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }            
        }


    }
}
