using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text itemNameText;
    public Text itemCountText;
    public GameObject selectedItem;

    public void AddItem(Item _item)
    {
        itemNameText.text = _item.itemName;
        icon.sprite = _item.itemIcon;

        if(Item.ItemType.Use == _item.itemType)
        {
            if (_item.itemCount > 0)
            {
                StringBuilder sb = new StringBuilder("x");
                sb.Append(_item.itemCount);
                itemCountText.text = sb.ToString();
            }
            else
            {
                itemCountText.text = "";
            }
        }
    }

    public void RemoveItem()
    {
        itemNameText.text = "";
        itemCountText.text = "";
        icon.sprite = null;
    }
}
