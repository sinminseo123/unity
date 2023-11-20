using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;//���� ���̵�
    public int prefabId; //������ ���̵�
    public float damage; //���� ������
    public int count; //���� ����
    public float speed; //���� �ӵ�

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
                transform.Rotate(Vector3.back * speed * Time.deltaTime); //�������� ȸ��
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

        //�׽�Ʈ �ڵ�
        if (Input.GetButtonDown("Jump"))
            LevelUp(10,1);
    }

    public void LevelUp(float damage, int count) //���� ������ �Լ�
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
        // �⺻ ����
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;


        // ������Ƽ ����
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
                speed = 0.3f; //���Ÿ� ���⿡�� speed�� ����ӵ� �ǹ�
                break;
        }

        //hand �ʱ�ȭ
        Hand hand = player.hands[(int)data.itemtype];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver); //�÷��̾��� ��� �ڽĿ�����Ʈ�� applygear ����ϵ���
    }

    private void Batch() //������ ���⸦ ��ġ�ϴ� �Լ�
    {
        for (int i = 0; i < count; i++) //���� count ���� ��ŭ Ǯ������ ��������
        {
            Transform bullet;
            
            //���� ���� ������Ʈ ��Ȱ���ϰų� Ǯ���� ��������
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                bullet.parent = transform; //������ �θ��� ����

            }

            bullet.localPosition = Vector3.zero; //������ ��ġ �ʱ�ȭ
            bullet.localRotation = Quaternion.identity; //������ ȸ�� �ʱ�ȭ

            Vector3 rotVec = Vector3.forward * 360 * i / count; // �������� ȸ������
            bullet.Rotate(rotVec); //ȸ��
            bullet.Translate(bullet.up * 1.5f, Space.World); //�������� ��ġ

            bullet.GetComponent<Bullet>().Init(damage, -1,Vector3.zero); //������ �������� ����� ���� , -1�� ���� ����
        }
    }

    void Fire() //���Ÿ� ���⸦ Ǯ���� �������� �Լ�
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized; //���Ÿ� ���� �߻� ����

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); //���Ÿ����� ȸ��
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
