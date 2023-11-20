using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩 보관 변수 
    public GameObject[] prefabs;

    // 풀을 담당하는 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];//prefabs와 pools는 1:1관계)

        for(int i = 0; i < pools.Length; i++) //리스트 초기화
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index) //게임 오브젝트 반환 함수
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            // 선택한 풀의 비활성화된 게임 오브젝트 접근
            if (!item.activeSelf)
            {
                // 발견 시 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
            // 다 활성화 혹은 없을 시
            if (!select)
            {
                // poolmanager의 자식요소로 새롭게 생성하고 select 변수에 할당
                select = Instantiate(prefabs[index],transform);
                pools[index].Add(select); // 생성된 오브젝트를 해당 오브젝트 풀 리스트에 추가
            }
            return select;                
    }
}
