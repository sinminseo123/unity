using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // ������ ���� ���� 
    public GameObject[] prefabs;

    // Ǯ�� ����ϴ� ����Ʈ��
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];//prefabs�� pools�� 1:1����)

        for(int i = 0; i < pools.Length; i++) //����Ʈ �ʱ�ȭ
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index) //���� ������Ʈ ��ȯ �Լ�
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            // ������ Ǯ�� ��Ȱ��ȭ�� ���� ������Ʈ ����
            if (!item.activeSelf)
            {
                // �߰� �� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }
            // �� Ȱ��ȭ Ȥ�� ���� ��
            if (!select)
            {
                // poolmanager�� �ڽĿ�ҷ� ���Ӱ� �����ϰ� select ������ �Ҵ�
                select = Instantiate(prefabs[index],transform);
                pools[index].Add(select); // ������ ������Ʈ�� �ش� ������Ʈ Ǯ ����Ʈ�� �߰�
            }
            return select;                
    }
}
