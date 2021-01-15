using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public int itemID;
    public int _count;
    public string pickUpSound;

    public void GetItem()
    {
        AudioManager.instance.Play(pickUpSound);
        Inventory.instance.GetAnItem(itemID, _count);
        Destroy(gameObject);
    }
}
