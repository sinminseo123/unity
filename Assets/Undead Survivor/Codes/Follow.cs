using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{   
    //체력 슬라이더가 플레이어를 따라가도록
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.Instance.player.transform.position);
    }

}
