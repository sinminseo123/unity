using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType {Melle, Range, Glove, Shoe, Heal} //아이템 타입 설정: 근접,원거리, 장갑, 신발, 힐

    [Header("# Main Info")]
    public ItemType itemtype;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage; //기본(0레벨) 데미지
    public int baseCount; //기본 (0레벨) (근)개수, (원)관통
    public float[] damages;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;

}
