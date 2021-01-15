using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerManager : Player
{
    static public PlayerManager instance;


    public string currentMapName; // transferMap 스크립트에 있는 transferMapName의 변수를 가짐.

    AudioManager theAudio;

    RaycastHit2D interactionHit;
    WaitForSeconds waitTime = new WaitForSeconds(0.2f);



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    private void Start()
    {
        queue = new Queue<string>();
        moveDirection = new Vector3(0, -1);
        playerRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        theAudio = FindObjectOfType<AudioManager>();
        col2D = GetComponent<BoxCollider2D>();
        canWalk = true;
        StartCoroutine(InteractionCoroutine());
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(interactionHit.transform != null)
            {
                ItemPickUp itemPick = interactionHit.transform.GetComponent<ItemPickUp>();
                itemPick.GetItem();
            }
        }
    }

    private void FixedUpdate()
    {
        base.SetLayer();
    }

    void Move()
    {
        if (!canWalk)
        {
            return;
        }

        Vector3 realTimeKey = new Vector3(Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"), 0);

        if (realTimeKey.sqrMagnitude != 0) // 이동중
        {
            moveDirection = realTimeKey;
            animator.SetBool("IsWalking", true);
            theAudio.Play("GrassStep");

        }
        else // 정지
        {
            animator.SetBool("IsWalking", false);
            theAudio.Stop("GrassStep");
        }

        // 움직이는 방향이 애니메이션 방향
        animator.SetFloat("DirX", moveDirection.x);
        animator.SetFloat("DirY", moveDirection.y);

        // 플레이어 이동
        if (realTimeKey.x != 0 && realTimeKey.y != 0)
        {
            playerRigid.velocity = realTimeKey * speed * 0.72f;
        }
        else
        {
            playerRigid.velocity = realTimeKey * speed;
        }
    }

    public void StopMove()
    {
        playerRigid.velocity = Vector3.zero;
        animator.SetBool("IsWalking", false);
        theAudio.Stop("GrassStep");
    }

    IEnumerator InteractionCoroutine()
    {
        while (true)
        {
            interactionHit = base.RaycastOn();
            yield return waitTime;
        }
    }
    
}
