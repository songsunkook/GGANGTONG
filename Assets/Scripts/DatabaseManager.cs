using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public DatabaseManager instance;

    public string[] varName;
    public float[] var;

    public string[] switchName;
    public bool[] switches;

    public List<Item> itemList = new List<Item>();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        itemList.Add(new Item(10001, "빨간 포션", "체력을 50 회복시켜주는 마법의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10002, "파란 포션", "마나를 15 회복시켜주는 마법의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10003, "농축 빨간 포션", "체력을 350 회복시켜주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(10004, "농축 빨간 포션", "마나를 80 회복시켜주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(20001, "용사의 검", "용사가 사용하던 옛 검", Item.ItemType.Equip));
    }

    public void UseItem(int _itemID)
    {
        switch (_itemID)
        {
            case 10001:
                Debug.Log("HP가 50 회복 되었습니다.");
                break;
            case 10002:
                Debug.Log("MP가 15 회복 되었습니다.");
                break;
            case 10003:
                break;
        }
    }
}
