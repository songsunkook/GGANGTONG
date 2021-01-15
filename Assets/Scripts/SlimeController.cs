using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : Player
{
    public string attackSound;

    public int attack; // 공격력
    public float attackDelay; // 공격 유예
    public float interMoveWaitTime; // 대기 시간


    string direction;
    float currentInterMWT;
    int randomInt;
    Vector3 playerPos; // 플레이어의 좌표값.

    private void Start()
    {
        playerRigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        col2D = GetComponent<BoxCollider2D>();
        queue = new Queue<string>();
        currentInterMWT = interMoveWaitTime;
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);
        AudioManager.instance.Play(attackSound);
        if (NearPlayer())
        {
            Debug.Log(characterName + "이가 " + PlayerManager.instance.name + "에게 " + attack + "만큼 피해를 입혔습니다.");
//            PlayerStat.instance.Hit(attack);
        }
    }

    private void Update()
    {
        currentInterMWT -= Time.deltaTime;

        if(currentInterMWT <= 0)
        {
            currentInterMWT = interMoveWaitTime;

            if (NearPlayer())
            {
                animator.SetTrigger("Attack");
                StartCoroutine(WaitCoroutine());
                return;
            }

            RandomDirection();
            base.Move(direction);
        }
    }

    private void FixedUpdate()
    {
        RaycastOn();
        base.SetLayer();
    }

    bool NearPlayer()
    {
        playerPos = PlayerManager.instance.transform.position;

        if((playerPos - gameObject.transform.position).sqrMagnitude < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void RandomDirection()
    {
        randomInt = Random.Range(0, 4);

        switch (randomInt)
        {
            case 0:
                direction = "Up";
                break;
            case 1:
                direction = "Down";
                break;
            case 2:
                direction = "Left";
                break;
            case 3:
                direction = "Right";
                break;
        }
    }

}
