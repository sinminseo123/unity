using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health}
    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.Instance.exp; //현재 경험치
                float maxExp = GameManager.Instance.nextExp[Mathf.Min(GameManager.Instance.level, GameManager.Instance.nextExp.Length - 1)]; //최고 경험치 = 다음 레벨업시 필요 경험치
                mySlider.value = curExp/maxExp; //슬라이더에 적용
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.Instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.Instance.kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.Instance.maxGameTime - GameManager.Instance.gameTime; //남은 게임 시간 계산
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);

                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.Instance.health; //현재 경험치
                float maxHealth = GameManager.Instance.maxHealth; //최고 경험치 = 다음 레벨업시 필요 경험치
                mySlider.value = curHealth / maxHealth; //슬라이더에 적용
                break;

        }    
    }
}
