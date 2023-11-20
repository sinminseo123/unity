using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType {Melle, Range, Glove, Shoe, Heal} //������ Ÿ�� ����: ����,���Ÿ�, �尩, �Ź�, ��

    [Header("# Main Info")]
    public ItemType itemtype;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage; //�⺻(0����) ������
    public int baseCount; //�⺻ (0����) (��)����, (��)����
    public float[] damages;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;

}
