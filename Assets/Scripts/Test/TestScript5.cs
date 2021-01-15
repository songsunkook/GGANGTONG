using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript5 : MonoBehaviour
{
    PlayerManager thePlayer;
    NumberSystem theNumber;

    public bool flag;
    public int correctNumber;

    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theNumber = FindObjectOfType<NumberSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag)
        {
            StartCoroutine(ACoroutine());
        }
    }

    IEnumerator ACoroutine()
    {
        flag = true;
        thePlayer.canWalk = false;
        thePlayer.StopMove();
        theNumber.ShowNumber(correctNumber);
        yield return new WaitUntil(() => !theNumber.activated);
        thePlayer.canWalk = true;
        Debug.Log(theNumber.GetResult());
    }

}
