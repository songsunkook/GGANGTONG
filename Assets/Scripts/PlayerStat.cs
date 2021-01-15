using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;
    public GameObject prefabFloatingText;
    public CameraManager theCamera;
    public GameObject parent;
    public string dmgSound;

    public int characterLevel;
    public int[] needExp;
    public int currentExp;

    public int hp;
    public int currentHp;

    public int mp;
    public int currentMp;

    public int atk;
    public int def;

    private void Start()
    {
        instance = this;
    }

    public void Hit(int _enemyAtk)
    {
        int dmg;

        if(def >= _enemyAtk)
        {
            dmg = 1;
        }
        else
        {
            dmg = _enemyAtk - def;
        }

        currentHp -= dmg;

        if (currentHp <= 0)
        {
            Debug.Log("체력 0이하, 게임오버");
        }
        AudioManager.instance.Play(dmgSound);

        Vector3 vector = this.transform.position;

        GameObject clone = Instantiate(prefabFloatingText,
                    Vector3.zero,
                    Quaternion.identity);
        clone.transform.SetParent(parent.transform);
        clone.GetComponent<RectTransform>().anchoredPosition = theCamera.GetVector3PlayerperCanvas();

//        StringBuilder sb = new StringBuilder();
//        sb.Append(theDatabase.itemList[i].itemName);
//        sb.Append(" ").Append(_count).Append("개 획득 +");
//       clone.GetComponent<Text>().text = sb.ToString();

    }

}
