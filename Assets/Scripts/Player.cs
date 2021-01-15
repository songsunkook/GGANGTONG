using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{
    public string characterName;
    public LayerMask layerMask;
    public float speed;
    public Rigidbody2D playerRigid;
    public BoxCollider2D col2D;
    public Animator animator;
    public Queue<string> queue; //
    public bool canWalk;

    bool notCoroutine = false;
    protected Vector3 moveDirection;

    protected void SetLayer()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100f);
    }

    protected RaycastHit2D RaycastOn()
    {
        // 레이캐스트 충돌
        RaycastHit2D hit;
        Vector2 start = transform.position; // 현재 위치
        Vector2 end = new Vector2(start.x + moveDirection.x,
            start.y + moveDirection.y); // 캐릭터가 이동하고자 하는 위치값.
        hit = Physics2D.Linecast(start, end, layerMask);
        RaycastHit2D boxHit = Physics2D.BoxCast(start, col2D.bounds.size, 0f, moveDirection, 0.08f, layerMask);
        
        Debug.DrawLine(start, end, Color.red);
        if (boxHit.transform != null)
        {
            playerRigid.velocity = Vector2.zero;
            Debug.Log("충돌");
        }

        return hit;
    }

    public void Move(string _dir, int _frequency = 5)
    {
        queue.Enqueue(_dir);
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCorutine(_dir, _frequency));
        }
        
    }

    IEnumerator MoveCorutine(string _dir,int _frequency)
    {
        while(queue.Count != 0)
        {
            switch (_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4f);
                    break;
                case 2:
                    yield return new WaitForSeconds(3f);
                    break;
                case 3:
                    yield return new WaitForSeconds(2f);
                    break;
                case 4:
                    yield return new WaitForSeconds(1f);
                    break;
                case 5:
                    break;
            }
            string direction = queue.Peek();
            switch (direction)
            {
                case "Up":
                    animator.SetBool("IsWalking", true);
                    moveDirection.Set(0, 1, moveDirection.z);
                    break;

                case "Down":
                    animator.SetBool("IsWalking", true);
                    moveDirection.Set(0, -1, moveDirection.z);
                    break;

                case "Right":
                    animator.SetBool("IsWalking", true);
                    moveDirection.Set(1, 0, moveDirection.z);
                    break;

                case "Left":
                    animator.SetBool("IsWalking", true);
                    moveDirection.Set(-1, 0, moveDirection.z);
                    break;

                case "Stop":
                    animator.SetBool("IsWalking", false);
                    break;
            }
            // NPC의 애니메이션 방향 설정
            animator.SetFloat("DirX", moveDirection.x);
            animator.SetFloat("DirY", moveDirection.y);


            if (animator.GetBool("IsWalking")) // 걷는 경우
            {
                playerRigid.velocity = moveDirection * speed;
                yield return new WaitForSeconds(0.5f);
                playerRigid.velocity = Vector2.zero;
                if (_frequency != 5)
                {
                    animator.SetBool("IsWalking", false);
                }
            }
            else // 멈춤
            {
                yield return new WaitForSeconds(0.5f);
            }

            queue.Dequeue();
        }

        animator.SetBool("IsWalking", false);
        notCoroutine = false;
    }

    private void OnDrawGizmos()
    {
        Handles.color = new Color(1, 0, 0, 0.2f);
        Handles.DrawSolidArc(transform.position, Vector3.forward, moveDirection.normalized, 22.5f, 5f);
        Handles.DrawSolidArc(transform.position, Vector3.forward, moveDirection.normalized, -22.5f, 5f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        playerRigid.velocity = Vector3.zero;
        animator.SetBool("IsWalking", false);
    }
}
